/**
Script used to login.
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

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class Login : MonoBehaviour
{

	private MqttClient mqttClient;

	public InputField email;
	public InputField password;
	public GameObject validationText;

	private string MqttMailCheckLogin = "";
	private string Email = "";
	private string Password = "";
	private string ValidationText = "";

	private string hashedEmail = "";
	private string hashedPassword = "";



	// Use this for initialization
	void Start()
	{
		MqttMailCheckLogin = "";
		mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
		string clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId, "theHub", "theHub");
		//getMesssage subscribed
		mqttClient.Subscribe(new string[] { "thehub/rundorisrun/player/checkget" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

	}

	public void LoginUser()
	{

		//check if email and the password already exist
		if (Email != "")
		{
			hashedEmail = Hash(Email);
			hashedPassword = Hash(Password);
			Debug.Log(hashedEmail + " : " + MqttMailCheckLogin);
			Debug.Log(hashedPassword);

			string messageJson = "{\"hashedEmail\": \"" + hashedEmail + "\"}";
			if (mqttClient.IsConnected)
			{
				mqttClient.Publish("thehub/rundorisrun/player/checkset", System.Text.Encoding.UTF8.GetBytes(messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);

			}
			else
			{
				Debug.Log("MQTT connection does not work.");
			}


			Boolean checkEmailbool = checkEmail(hashedEmail, hashedPassword);

			if (checkEmailbool)
			{   
				Debug.Log("ITS works");
				SceneManager.LoadScene("Game Options");
			}
			else
			{
				ValidationText = "Invalid email or password, please try again";
				email.text = "";
				password.GetComponent<InputField> ().text = "";
				Debug.LogWarning(ValidationText);
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}
		else
		{
			ValidationText = "Email field is empty, please try again!";
			email.text = "";
			password.GetComponent<InputField> ().text = "";
			Debug.LogWarning(ValidationText);
			validationText.GetComponent<Text>().text = ValidationText;
		}

	}

	//returns a boolean that says if the email exists in the db or not
	bool checkEmail(string hashedEmail, string hashedPassword)
	{
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

		if ((MqttMailCheckLogin.Contains(hashedEmail)) && (MqttMailCheckLogin.Contains(hashedPassword)))
		{
			Debug.Log(MqttMailCheckLogin.Substring(1, (MqttMailCheckLogin.Length - 2)));
			PlayerObject playerObject = JsonConvert.DeserializeObject<PlayerObject>(MqttMailCheckLogin.Substring(1, (MqttMailCheckLogin.Length - 2)));
			PlayerPrefs.SetString("playerName", playerObject.name);
			PlayerPrefs.SetString("playerID", playerObject._id);
            PlayerPrefs.SetString("hashedEmail", playerObject.hashedEmail);
            PlayerPrefs.SetString("hashedPass", playerObject.hashedPass);
            PlayerPrefs.Save();
			return true;

		}
		return false;
	}

	public void SendHashedEmail()
	{

		hashedEmail = Hash(Email);
		string messageJson = "{\"hashedEmail\": \"" + hashedEmail + "\"}";
		if (mqttClient.IsConnected)
		{
			mqttClient.Publish("thehub/rundorisrun/player/checkset", System.Text.Encoding.UTF8.GetBytes(messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);

		}
		else
		{
			Debug.Log("MQTT connection does not work.");
		}
	}

	//hash the password, email or any other data inputted using sha512


	// Update is called once per frame
	void Update()
	{
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

        //get the field of the components
        Email = email.text;
        Password = password.text;
        Debug.Log(MqttMailCheckLogin + "  :  " + "Email: " + Email + " : " + hashedEmail + " - Pass: " + hashedPassword);

        // Move to the next button with TAB.
        if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (email.isFocused)
			{
				password.GetComponent<InputField>().Select();
			}
		}

        if (email.isFocused)
        {
            password.GetComponent<InputField>().text = "";
        }

        

        if (Input.GetKeyDown(KeyCode.Return))
		{
			if (Email != "" && Password != "")
			{
				LoginUser();
			}
		}

	}

    void ClearPassword()
    {
        password.text = "";
    }

	void MqttMsgGetString(object sender, MqttMsgPublishEventArgs e)
	{
		MqttMailCheckLogin = System.Text.Encoding.UTF8.GetString(e.Message);
	}

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


}