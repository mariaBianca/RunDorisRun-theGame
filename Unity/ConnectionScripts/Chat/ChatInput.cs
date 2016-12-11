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

public class ChatInput : MonoBehaviour {

    public InputField ChatInputFieldText;
    public Button SendButton;
    private string currentMessage = string.Empty;
    private string user = string.Empty;
    private string messageToSend = string.Empty;

    private MqttClient mqttClient;
    private string MqttSubscribedMessage;

    bool enterPressed = false;

    // Use this for initialization
    void Start () {
        //create an mqtt client instance
        mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);

        string clientId = Guid.NewGuid().ToString();

        //connect with user, password
        mqttClient.Connect(clientId, "theHub", "theHub");
    }

    public void SendTextOnClick()
    {
        //get message from input field
        currentMessage = ChatInputFieldText.text;
        ChatInputFieldText.text = string.Empty;


        //currentMessage = ChatInputFieldText.text;
        if (!string.IsNullOrEmpty(currentMessage.Trim()))
        {
            
            //currentMessage = ChatInputFieldText.text;

            //temp
            user = "TheDude";

            //convert message to JSON format
            messageToSend = "{\"text\":\"" + currentMessage + "\"," +
                            "\"user\":\"" + user + "\"}";

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
        if (!ChatInputFieldText.isFocused)
        {
            ChatInputFieldText.Select();
        }

        //send message on Enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter Pressed!");
            SendTextOnClick();
        }
    }
}
