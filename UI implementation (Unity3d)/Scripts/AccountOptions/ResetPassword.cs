/**
Script used to reset the password of the user.
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

public class ResetPassword : MonoBehaviour {


	private MqttClient mqttClient;

	public GameObject email;
	public GameObject validationText;
	public GameObject username;


	private string ValidationText = "Insert your Email here!";
	private string tempPass = "";
	private string Email = "";
	private string MqttMailCheck = "";
	private string infoJson = "";
	private string hashedEmail = "";
	private string Username = "";

	// Use this for initialization of the MQTT client
	void Start()
	{
		MqttMailCheck = "";
		mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
		string clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId, "theHub", "theHub");
		//getMesssage subscribed
		mqttClient.Subscribe(new string[] { "thehub/rundorisrun/player/checkget" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

	}

	public void ResetPass(){
		
		bool EMV = false;   //boolean for the email validation


		//check if email is valid
		if (Email != "")
		{
			if (Email.Contains("@"))
			{
				if (Email.Contains("."))
				{
					EMV = true;
				}else
				{
					ValidationText = "Email is not valid, please try again!";
					Debug.LogWarning(ValidationText);
					validationText.GetComponent<Text>().text = ValidationText;
				}
			}else
			{
				ValidationText = "Email is not valid, please try again!";
				Debug.LogWarning(ValidationText);
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}else
		{
			ValidationText = "Email field is empty, please try again!";
			Debug.LogWarning(ValidationText);
			validationText.GetComponent<Text>().text = ValidationText;
		}

		//hash the password and send the objects to the database
		if (EMV) 
		{
			ValidationText = "Comparing information with the database...";
			validationText.GetComponent<Text> ().text = ValidationText;
			hashedEmail = Hash(Email);

			if (checkEmailExistance(hashedEmail))
			{
                tempPass = generatePass();
                string hashedPass = Hash (tempPass);

				PlayerObject playerObject = JsonConvert.DeserializeObject<PlayerObject>(MqttMailCheck.Substring(1, MqttMailCheck.Length -2));
				PlayerPrefs.SetString ("playerName", playerObject.name);
				PlayerPrefs.SetString ("playerID", playerObject._id);
				PlayerPrefs.SetString ("playerEmail", playerObject.hashedEmail);
				PlayerPrefs.Save ();
				string display = PlayerPrefs.GetString ("playerID") + ";" + PlayerPrefs.GetString ("playerName");
//				Debug.LogError (display);

				string messageJson = "{\"_id\": \"" + PlayerPrefs.GetString ("playerID") + "\" " +
										"\"name\": \"" + PlayerPrefs.GetString("playerName") + "\" " +
										",\"hashedEmail\": \"" + hashedEmail + "\" " +
										",\"hashedPass\": \"" + Hash(tempPass) + "\"}";

				//add it to the database through MQTT
				if (mqttClient.IsConnected) {
					mqttClient.Publish ("thehub/rundorisrun/player/changepw", System.Text.Encoding.UTF8.GetBytes (messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
				} else {
					Debug.LogWarning ("MQTT not connected!");
				}
					
				//clear all the registration fields 
				ValidationText = "A new password has been sent to your email address!";
				validationText.GetComponent<Text> ().text = ValidationText;
				sendEmail (Email);

			}else
			{
				ValidationText = "The email does not belong to a registered user!";
				validationText.GetComponent<Text>().text = ValidationText;
			}
		}
		
	}

    //returns a boolean that says if the email exists in the db or not
    public bool checkEmailExistance(string hashedEmail)
	{
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

	//send the HashedEmail through MQTT to the db in order to check if the hashedEmail already exists
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

	//send the Email with the new password to the player's registered email
	void sendEmail(String Email){
		//tempPass = generatePass ();
		//Debug.LogError (Email + " ;" + tempPass);
		SendEmailHandler.sendEmail (Email, tempPass);

	}

	//create a password form random alphanumerics, and some allowed nonalhpanumerics
	string generatePass(){
		//declare the valid characters for the password
		const string validChars = "abcdefghijklmnopqestuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

		//build a string of 10 characters
		int length = 10;
		StringBuilder pass = new StringBuilder ();
		for (int i = 0; i < length; i++) {
			pass.Append (validChars [Random.Range(0,61)]);
		}

		//Debug.Log (pass.ToString);
		return pass.ToString ();
	}


	// Update is called once per frame in order to update the frame
	void Update()
	{
		//get the field of the component
		Email = email.GetComponent<InputField>().text;
		ValidationText = validationText.GetComponent<Text> ().text;
		//SendHashedEmail();
		Debug.Log(MqttMailCheck + "  :  " + Hash(Email));

		if (Input.GetKeyDown(KeyCode.Return))
		{
			if (Email != "")
			{
				ResetPass();
			}
		}

	}

	//get the string from the MQTT 
	void MqttMsgGetString(object sender, MqttMsgPublishEventArgs e)
	{
		MqttMailCheck = System.Text.Encoding.UTF8.GetString(e.Message);
	}


}
