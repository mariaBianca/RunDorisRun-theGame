/**
Script used to register the user.
@author TheHub
DIT029 H16 Project: Software Architecture for Distributed Systems
University of Gothenburg, Sweden 2016

This file is part of "Run Doris Run!" game.
"Run Doris Run!" game is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Run Doris Runis distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with "Run Doris Run!" game.  If not, see <http://www.gnu.org/licenses/>.

*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Text;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class EmailRegistration : MonoBehaviour
{
	private MqttClient mqttClient;

	public GameObject email;
	public GameObject password;
	public GameObject confirmPassword;
	public GameObject username;
	public Text validationText;

	private string infoJson = "";
	private string Email = "";
	private string Username = "";
	private string Password = "";
	private string ConfirmPassword = "";
	private string ValidationText = "";
	private string MqttMailCheck = "";

	// Use this for initialization of the MQTT client
	void Start()
	{
        validationText.text = "";
		MqttMailCheck = "";
		mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
		string clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId, "theHub", "theHub");

		//getMesssage subscribed
		mqttClient.Subscribe(new string[] { "thehub/rundorisrun/player/checkget" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

	}

	// Adds the register button and its functionalities
	public void RegisterButton()
	{
		//print ("Registration Successful");
		bool UN = false;    //boolean for the username
		bool EMV = false;   //boolean for the email validation
		bool EM = false;    //boolean for the email existance
		bool PW = false;    //boolean for the password
		bool CPW = false;   //boolean for the confirm of password
		string hashedEmail = "";

		//check if username is valid
		if (Username != "")
		{
			UN = true;
		}
		else
		{
			ValidationText = "username field is empty, please try again!";
			Debug.LogWarning(ValidationText);
			validationText.GetComponent<Text>().text = ValidationText;
		}

		//check if email is valid
		if (Email != "")
		{
			if (Email.Contains("@"))
			{
				if (Email.Contains("."))
				{
					EMV = true;
				}
				else
				{
					ValidationText = "email is not valid, please try again!";
					Debug.LogWarning(ValidationText);
					validationText.GetComponent<Text>().text = ValidationText;
				}
			}
			else
			{
				ValidationText = "email is not valid, please try again!";
				Debug.LogWarning(ValidationText);
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}
		else
		{
			ValidationText = "email field is empty, please try again!";
			Debug.LogWarning(ValidationText);
			validationText.GetComponent<Text>().text = ValidationText;
		}

		//check if the password is valid
		if (Password != "")
		{
			if (Password.Length > 5)
			{
				PW = true;
			}
			else
			{
				ValidationText = "the password must be at least 6 characters long. please try again!";
				Debug.LogWarning(ValidationText);
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}
		else
		{
			ValidationText = "password field is empty, please try again!";
			Debug.LogWarning(ValidationText);
			validationText.GetComponent<Text>().text = ValidationText;
		}

		//check if password confirmation is valid (same with the password
		if (ConfirmPassword != "")
		{
			if (ConfirmPassword == Password)
			{
				CPW = true;
			}
			else
			{
				ValidationText = "the password fields do not match. please, try again!";
				Debug.LogWarning(ValidationText);
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}
		else
		{
			ValidationText = "password confirmation field is empty. please, try again!";
			Debug.LogWarning(ValidationText);
			validationText.GetComponent<Text>().text = ValidationText;
		}

		//check if email already exists
		if (UN && EMV && PW && CPW)
		{
			hashedEmail = Hash(Email);

			if (!checkEmailExistance(hashedEmail))
			{
				EM = true;
			}
			else
			{
				ValidationText = "email is already in use, please try again!";
				Debug.LogWarning(ValidationText);
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}

		//hash the password and send the objects to the database
		if (UN == true && EM == true && PW == true && CPW == true && EMV == true) 
		{
			string hashedPass = Hash (Password);

			PlayerPrefs.SetString ("playerName", Username);
			PlayerPrefs.Save ();

			//send the user JSON object through MQTT
			infoJson = "{\"name\": \"" + Username + "\" " +
				",\"hashedEmail\": \"" + hashedEmail + "\" " +
				",\"hashedPass\": \"" + hashedPass + "\"}";

			//add it to the database through MQTT
			if (mqttClient.IsConnected) {
				mqttClient.Publish ("thehub/rundorisrun/player/register", System.Text.Encoding.UTF8.GetBytes (infoJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
			} else {
				Debug.Log ("MQTT connection does not work.");
			}

			//clear all the registration fields 
			email.GetComponent<InputField> ().text = "";
			password.GetComponent<InputField> ().text = "";
			confirmPassword.GetComponent<InputField> ().text = "";
			username.GetComponent<InputField> ().text = "";
			validationText.GetComponent<Text> ().text = "";
			ValidationText = "";
			UN = false;
			EM = false;
			PW = false;
			CPW = false;
			ValidationText = "registration completed!";
			validationText.GetComponent<Text> ().text = ValidationText;
			Debug.Log (ValidationText);

			//load the options scene
			//SceneManager.LoadScene("Game Options");

		}
	}

	//returns a boolean that says if the email exists in the db or not
	public bool checkEmailExistance(string hashedEmail)
	{
		//bool check = false;
		if ((MqttMailCheck.Contains(hashedEmail)))
		{
			return true;
		}

		string messageJson = "{\"hashedEmail\": \"" + hashedEmail + "\" }";
		if (mqttClient.IsConnected)
		{
			mqttClient.Publish("thehub/rundorisrun/player/checkset", System.Text.Encoding.UTF8.GetBytes(messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
		}
		else
		{
			Debug.Log("MQTT connection does not work.");
		}
		return false;
	}

	//hash the password, email or any other data inputted using sha512
	string Hash(string data)
	{

		var bytes = new UTF8Encoding().GetBytes(data);
		byte[] hashBytes;
		using (var algorithm = new System.Security.Cryptography.SHA512Managed())
		{
			hashBytes = algorithm.ComputeHash(bytes);
		}
		return Convert.ToBase64String(hashBytes);
	}

	void SendHashedEmail()
	{
		string hashedEmailTemp;
		Email = email.GetComponent<InputField>().text;
		hashedEmailTemp = Hash(Email);
		string messageJsonTemp = "{\"hashedEmail\": \"" + hashedEmailTemp + "\"}";
		if (mqttClient.IsConnected)
		{
			mqttClient.Publish("thehub/rundorisrun/player/checkset", System.Text.Encoding.UTF8.GetBytes(messageJsonTemp), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);

		}
		else
		{
			Debug.Log("MQTT connection does not work.");
		}
	}

	// Update is called once per frame
	void Update()
	{
		//SendHashedEmail();
		// Move to the next button with TAB.
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (email.GetComponent<InputField>().isFocused)
			{
				username.GetComponent<InputField>().Select();
			}
			if (username.GetComponent<InputField>().isFocused)
			{
				password.GetComponent<InputField>().Select();
			}
			if (password.GetComponent<InputField>().isFocused)
			{
				confirmPassword.GetComponent<InputField>().Select();
			}
		}

		//get the field of the components
		Email = email.GetComponent<InputField>().text;
		Password = password.GetComponent<InputField>().text;
		ConfirmPassword = confirmPassword.GetComponent<InputField>().text;
		Username = username.GetComponent<InputField>().text;
        //SendHashedEmail();
        //Debug.Log(MqttMailCheck + "  :  " + "Email: " + Hash(Email) + "Pass: "+ Hash(Password));

		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (Email != "" && Username != "" && Password != "" && ConfirmPassword != "")
			{
				RegisterButton();
			}
		}

	}

	//get the string from the MQTT 
	void MqttMsgGetString(object sender, MqttMsgPublishEventArgs e)
	{
		MqttMailCheck = System.Text.Encoding.UTF8.GetString(e.Message);
	}
}