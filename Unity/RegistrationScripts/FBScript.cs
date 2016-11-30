/**
*Script used to connect to Facebook.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System;
using System.Text;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;


public class FBScript : MonoBehaviour
{
	private MqttClient mqttClient;


	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public GameObject DialogFirstName;
	public GameObject DialogLastName;
	public GameObject DialogProfilePic;
	public GameObject DialogEmail;
	public GameObject DialogId;

	//variables used to transfer data to db from facebook
	private string username = "";
	private string email = "";
	private string id = "";
	private string infoJson = "";
	private Text LastName ;
	private Text FirstName ;
	private Text Email;
	private Text Id;

	//Initialize the function
	void Awake()
	{
		FB.Init(setInit, onHideUnity);

		//mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
		mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

		string clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId);

		//subscribe to the topic "test" with Qos2
		mqttClient.Subscribe(new string[]{"test"}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});

	}

	//set the Facebook initialization and log accordingly
	void setInit()
	{
		if (FB.IsLoggedIn)
		{
			Debug.Log("Facebook is logged in.");
		}
		else
		{
			Debug.Log("Facebook is not logged in.");
		}
		DealWithFBMenus(FB.IsLoggedIn);

	}

	//if the game is shown continue, otherwise pause the game
	void onHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	//Login function
	public void FBlogin()
	{
		List<string> permissions = new List<string>();
		//Facebook in their documentations recommends to not ask too many permissions when you want to login
		//when you want to extra things down the line, thus only few permissions have been added
		permissions.Add("public_profile");
		permissions.Add("email");

		FB.LogInWithReadPermissions(permissions, AuthCallBack);
	}

	//deal with errors
	void AuthCallBack(IResult result)
	{
		if (result.Error != null)
		{
			Debug.Log(result.Error);
		}
		else
		{
			if (FB.IsLoggedIn)
			{
				Debug.Log("Facebook is logged in.");
			}
			else
			{
				Debug.Log("Facebook is not logged in.");
			}
			DealWithFBMenus(FB.IsLoggedIn);
		}
	}

	//change Facebook menus accordingly
	void DealWithFBMenus(bool isLoggedIn)
	{
		if (isLoggedIn)
		{
			DialogLoggedIn.SetActive(true);
			DialogLoggedOut.SetActive(false);
			FB.API("/me?fields=first_name", HttpMethod.GET, DisplayFirstName);
			FB.API ("/me?fields=last_name", HttpMethod.GET, DisplayLastName);
			FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
			FB.API("/me?fields=email", HttpMethod.GET, DisplayEmail);
			FB.API("/me?fields=id", HttpMethod.GET, DisplayId);
		}
		else
		{
			DialogLoggedIn.SetActive(false);
			DialogLoggedOut.SetActive(true);
		}
	}

	//method that displays the grabbed first name
	void DisplayFirstName(IResult result)
	{
		FirstName = DialogFirstName.GetComponent<Text>();
		if (result.Error == null)
		{
			FirstName.text = "" + result.ResultDictionary ["first_name"];
		}
		else
		{
			Debug.Log(result.Error);
		}
	}

	//method that displays the grabbed last name
	void DisplayLastName(IResult result)
	{
		LastName = DialogLastName.GetComponent<Text>();
		if (result.Error == null)
		{
			LastName.text = "" + result.ResultDictionary ["last_name"];
		}
		else
		{
			Debug.Log(result.Error);
		}
	}

	//method that displays the grabbed email
	void DisplayEmail(IResult result)
	{
		Email = DialogEmail.GetComponent<Text>();
		if (result.Error == null)
		{
			Email.text = "" + result.ResultDictionary["email"];
		}
		else
		{
			Debug.Log(result.Error);
		}

	}

	//method that displays the grabbed Id
	void DisplayId(IResult result)
	{
		Id = DialogId.GetComponent<Text>();
		if (result.Error == null)
		{
			Id.text = "" + result.ResultDictionary["id"];
		}
		else
		{
			Debug.Log(result.Error);
		}

	}

	//method that tests the login button functionality
	public void TestLoginButton()
	{
		Console.WriteLine("It works!");
	}

	//method that displays the profile picture
	void DisplayProfilePic(IGraphResult result)
	{
		if (result.Texture != null)
		{
			Image ProfilePic = DialogProfilePic.GetComponent<Image>();
			ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
		}
		RegisterUser ();
	}

	public string Hash(string data){

		var bytes = new UTF8Encoding ().GetBytes (data);
		byte[] hashBytes;
		using (var algorithm = new System.Security.Cryptography.SHA512Managed ()) {
			hashBytes = algorithm.ComputeHash (bytes);
		}
		return Convert.ToBase64String(hashBytes);
	}

	//register the user into the database
	void RegisterUser(){
		string hashedEmail = Hash (email);
		username = FirstName.text.ToString () + " " + LastName.text.ToString();
		id = Id.text.ToString ();
		email = Email.text.ToString ();
		hashedEmail = Hash (email);
		string hashedPass = Hash ("" + Random.Range(0, 10000));
		Debug.Log (username);
		infoJson = "{\"name\": \"" + username + "\" " +
			",\"user_ID\": " + id +
			",\"hashedEmail\": \"" + hashedEmail + "\" " +
			",\"hashedPass\": \"" + hashedPass + "\" " +
			",\"typeOfClient\": \"RunDorisRun\" }";      

		//add it to the database through MQTT
		if (mqttClient.IsConnected) {
			Debug.Log (infoJson);
			mqttClient.Publish ("testStore", System.Text.Encoding.UTF8.GetBytes (infoJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
		} else {
			Debug.Log ("MQTT connection does not work.");
		}

	}
}