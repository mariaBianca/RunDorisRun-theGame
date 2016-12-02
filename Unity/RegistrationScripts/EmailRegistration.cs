/**
*Script used to connect to Facebook.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
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



public class EmailRegistration : MonoBehaviour
{
	private MqttClient mqttClient;

	public GameObject email;
	public GameObject password;
	public GameObject confirmPassword;
	public GameObject username;
	public GameObject validationText;

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
		MqttMailCheck = "";
		mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);
		string clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId);
		//getMesssage subscribed
		mqttClient.Subscribe(new string[] { "check" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
		mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

	}

	// Adds the register button and its functionalities
	public void RegisterButton()
	{
		//print ("Registration Successful");
		bool UN = false; //boolean for the username
		bool EMV = false; //boolean for the email validation
		bool EM = false; //boolean for the email existance
		bool PW  = false; //boolean for th	e password
		bool CPW = false; //boolean for the confirm of password
		string hashedEmail = "";

		//check if username is valid
		if (Username != "") {
			UN = true;
		} else {
			ValidationText = "Username field is empty!";
			Debug.LogWarning (ValidationText);
			validationText.GetComponent<Text> ().text = ValidationText;
		}

		//check if email already exists
		if (Email != "") {
			hashedEmail = Hash (Email);

			if (!checkEmailExistance(hashedEmail)) {
				EM = true;
			} else {
				ValidationText = "Email is already in use.";
				Debug.LogWarning (ValidationText);
				validationText.GetComponent<Text> ().text = ValidationText;
			}
		} else {
			ValidationText = "Email field is empty.";
			Debug.LogWarning (ValidationText);
			validationText.GetComponent<Text> ().text = ValidationText;
		}

		//check if email is valid
		if (Email != "") {
			if (Email.Contains ("@")) {
				if (Email.Contains (".")) {
					EMV = true;
				} else {
					ValidationText = "Email is not valid.";
					Debug.LogWarning (ValidationText);	
					validationText.GetComponent<Text> ().text = ValidationText;
				}
			} else {
				ValidationText = "Email is not valid.";
				Debug.LogWarning (ValidationText);
				validationText.GetComponent<Text> ().text = ValidationText;
			}
		} else {
			ValidationText = "Email field is empty!";
			Debug.LogWarning (ValidationText);
			validationText.GetComponent<Text> ().text = ValidationText;
		}

		//check if the password is valid
		if (Password != "") {
			if (Password.Length > 5) {
				PW = true;
			} else {
				ValidationText = "The password must be at least 6 characters.";
				Debug.LogWarning (ValidationText);
				validationText.GetComponent<Text> ().text = ValidationText;
			}
		} else {
			ValidationText = "Password field is empty!";
			Debug.LogWarning (ValidationText);
			validationText.GetComponent<Text> ().text = ValidationText;
		}

		//check if password confirmation is valid (same with the password
		if (ConfirmPassword != "") {
			if (ConfirmPassword == Password) {
				CPW = true;
			} else {
				ValidationText = "Password and password confirmation field do not match!";
				Debug.LogWarning (ValidationText);
				validationText.GetComponent<Text> ().text = ValidationText;
			}
		} else {
			ValidationText = "Password confirmation field is empty!"; 
			Debug.LogWarning (ValidationText);
			validationText.GetComponent<Text> ().text = ValidationText;
		}

		//hash the password and send the objects to the database
		if (UN == true && EM == true && PW == true && CPW == true && EMV == true){
			string hashedPass = Hash (Password);

			//send the user JSON object through MQTT
			infoJson = "{\"name\": \"" + Username + "\" " +
				",\"hashedEmail\": \"" + hashedEmail + "\" " +
				",\"hashedPass\": \"" + hashedPass + "\"}";

			//add it to the database through MQTT
			if (mqttClient.IsConnected) {
				mqttClient.Publish ("register", System.Text.Encoding.UTF8.GetBytes (infoJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
			} else {
				Debug.Log ("MQTT connection does not work.");
			}

			//clear all the registration fields 
			email.GetComponent<InputField>().text = "";
			password.GetComponent<InputField>().text = "";
			confirmPassword.GetComponent<InputField>().text = "";
			username.GetComponent<InputField> ().text = "";
			validationText.GetComponent<Text> ().text = "";
			ValidationText = "";
			UN = false;
			EM = false;
			PW = false;
			CPW = false;
			ValidationText = "Registration Completed!";
			Debug.Log (ValidationText);
			validationText.GetComponent<Text> ().text = ValidationText;

		} 
	}

	//returns a boolean that says if the email exists in the db or not
	bool checkEmailExistance(string hashedEmail){
		//bool check = false;
		if((MqttMailCheck.Contains(hashedEmail)) )
		{
			return true;
		}

		string messageJson = "{\"hashedEmail\": \"" + hashedEmail + "\" }";
		if (mqttClient.IsConnected) {
			mqttClient.Publish ("check", System.Text.Encoding.UTF8.GetBytes (messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
		} else {
			Debug.Log ("MQTT connection does not work.");
		}
		return false;
	}

	//hash the password, email or any other data inputted using sha512
	string Hash(string data){

		var bytes = new UTF8Encoding ().GetBytes (data);
		byte[] hashBytes;
		using (var algorithm = new System.Security.Cryptography.SHA512Managed ()) {
			hashBytes = algorithm.ComputeHash (bytes);
		}
		return Convert.ToBase64String(hashBytes);
	}

	// Update is called once per frame
	void Update()
	{
		// Move to the next button with TAB.
		if (Input.GetKeyDown(KeyCode.Tab)){
			if (email.GetComponent<InputField>().isFocused)
			{
				username.GetComponent<InputField>().Select();
			}
			if (username.GetComponent<InputField> ().isFocused) 
			{
				password.GetComponent<InputField> ().Select ();
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
		Username = username.GetComponent<InputField> ().text;

		if (Input.GetKeyDown (KeyCode.Return)) {
			if (Email != "" && Username != "" && Password != "" && ConfirmPassword != "") {
				RegisterButton ();
			} 
		}

	}

	void MqttMsgGetString(object sender, MqttMsgPublishEventArgs e) 
	{ 
		MqttMailCheck = System.Text.Encoding.UTF8.GetString(e.Message);
	}
}