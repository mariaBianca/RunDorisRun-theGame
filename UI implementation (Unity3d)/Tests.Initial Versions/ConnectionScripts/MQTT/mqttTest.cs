/**
*Script used to connect test the Unity3D to MongoDB connection.
*This script utilises the M2Mqtt client library for the MQTT protocol
*
*This is a modified script from https://github.com/vovacooper/Unity3d_MQTT/
*
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/
using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class mqttTest : MonoBehaviour {
	private MqttClient client;
	// This function initializes the script
	void Start () {
        // create an instance of the client 
        //client = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
        client = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

        // register to received messages
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
		
		string clientId = Guid.NewGuid().ToString();
        //client.Connect(clientId, "owntracks", "theHub");
        client.Connect(clientId);

        // subscribe to the topic "test/test" with QoS 2 (which sends messages exactly once)
        client.Subscribe(new string[] { "test/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });


    }
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{ 

		Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message)  );
	} 
	
	//creates an onGUI button that publishes a message through the MQTT protocol
	void OnGUI(){
		if ( GUI.Button (new Rect (20,40,80,20), "Level 1")) {
			Debug.Log("sending...");
            client.Publish("test/test", System.Text.Encoding.UTF8.GetBytes("{\"Score_ID\":\"NewPlayer25\", \"Score\":1556, \"Player_ID\":88798765, \"Level_ID\":11}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("sent");
		}
	}
	// the function Update is called once per frame in Unity
	void Update () {



	}
}
