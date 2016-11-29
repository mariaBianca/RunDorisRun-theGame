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

public class MqttChatTest : MonoBehaviour
{
	private MqttClient mqttClient;
	private string messageJson = "";
	public string username = "username";
	private string msgText; 
	public InputField msgField;
	// Use this for initialization of the MQTT client
	void Start()
	{
		//mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
		mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);
		string clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId);
	}


	public void doClick()
	{
		msgText = msgField.text;

		messageJson = "{\"name\": \" " + username + "\" " +
			",\"text\": \"" + msgText + "\"}";
		Debug.Log ("hey " + msgText);

		//add it to the database through MQTT
		if (mqttClient.IsConnected)
		{
			mqttClient.Publish("test", System.Text.Encoding.UTF8.GetBytes(messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
		}
		else
		{
			Debug.Log("MQTT connection does not work.");
		}
	}

}
