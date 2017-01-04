/**
Script used to define the inputs for the chat.
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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ChatInput : MonoBehaviour {

    public InputField ChatInputField;
    public Button SendButton;
    private string currentMessage = string.Empty;
    private string user = string.Empty;
    private string messageToSend = string.Empty;

    private MqttClient mqttClient;
    private string MqttMessage = string.Empty;

    bool enterPressed = false;

    [SerializeField]
    private Text UserName;

    //private Text newLine;

    // Use this for initialization
    void Start () {
        UserName.text = "logged in as:" + PlayerPrefs.GetString("playerName");
        //create an mqtt client instance
        mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
        //mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

        string clientId = Guid.NewGuid().ToString();

        //connect with user, password
        mqttClient.Connect(clientId, "theHub", "theHub");
        //mqttClient.Connect(clientId);


        mqttClient.Subscribe(new string[] { "RunDorisRun/chatroom/potatochat" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        //mqttClient.Subscribe(new string[] { "test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    public void SendTextOnClick()
    {
        //get message from input field
        currentMessage = ChatInputField.text;
        ChatInputField.text = string.Empty;


        //currentMessage = ChatInputFieldText.text;
        if (!string.IsNullOrEmpty(currentMessage.Trim()))
        {
            
            //currentMessage = ChatInputFieldText.text;

            //temp
            //user = "TheDude";
            int messageId = Random.Range(1000000, 9999999);

            //convert message to JSON format
            messageToSend = "{\"text\":\"" + currentMessage + "\"," +
                            "\"user\":\"" + PlayerPrefs.GetString("playerName") + "\"," +
                            "\"messageId\":\"" + messageId + "\"}";

            //sent through mqtt here

            if (mqttClient.IsConnected)
            {
                Debug.Log(messageToSend);
                //Send mqtt message that asks for the scores
                mqttClient.Publish("thehub/rundorisrun/chat/send", System.Text.Encoding.UTF8.GetBytes(messageToSend), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            }
            currentMessage = string.Empty;
        }
    } 

    // Update is called once per frame
    void Update () {
        //focus on InputField
        if (!ChatInputField.isFocused)
        {
            ChatInputField.Select();
        }


        //send message on Enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter Pressed!");
            SendTextOnClick();
        }
    }
}
