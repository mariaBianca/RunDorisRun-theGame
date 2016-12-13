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
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ChatInput : MonoBehaviour {

    public InputField ChatInputFieldText;
    public Button SendButton;
    private string currentMessage = string.Empty;
    private string user = string.Empty;
    private string messageToSend = string.Empty;
    private ChatMessageObject ChatMessage;

    private MqttClient mqttClient;
    private string MqttMessage = string.Empty;
    private string MqttOldMessage = string.Empty;

    bool enterPressed = false;

    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private Text chatLine;

    public ScrollRect chatScrollRect;
    public Scrollbar chatScrollBar;
    //private Text newLine;

    // Use this for initialization
    void Start () {
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
        currentMessage = ChatInputFieldText.text;
        ChatInputFieldText.text = string.Empty;


        //currentMessage = ChatInputFieldText.text;
        if (!string.IsNullOrEmpty(currentMessage.Trim()))
        {
            
            //currentMessage = ChatInputFieldText.text;

            //temp
            user = "TheDude";
            int messageId = Random.Range(1000000, 9999999);

            //convert message to JSON format
            messageToSend = "{\"text\":\"" + currentMessage + "\"," +
                            "\"user\":\"" + user + "\"," +
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

    void showLatestMessage()
    {
        ChatMessage = JsonConvert.DeserializeObject<ChatMessageObject>(MqttMessage);
        var newLine = ((GameObject)Instantiate(chatLine.gameObject)).GetComponent<Text>();
        string receivedMessage = ChatMessage.user + ": " + ChatMessage.text;
        newLine.text = receivedMessage;
        Debug.Log(receivedMessage);
        newLine.gameObject.SetActive(true);
        newLine.rectTransform.SetParent(content);

        Canvas.ForceUpdateCanvases();
    }

    void MqttMsgGetString(object sender, MqttMsgPublishEventArgs e)
    {
        MqttMessage = System.Text.Encoding.UTF8.GetString(e.Message);
    }

    // Update is called once per frame
    void Update () {
        mqttClient.MqttMsgPublishReceived += MqttMsgGetString;
        //focus on InputField
        if (!ChatInputFieldText.isFocused)
        {
            ChatInputFieldText.Select();
        }
        Canvas.ForceUpdateCanvases();

        Debug.Log(MqttMessage);

        if (!MqttOldMessage.Equals(MqttMessage) && !MqttMessage.Equals(string.Empty))
        {
            showLatestMessage();

            chatScrollRect.verticalNormalizedPosition = 0.0f;
            chatScrollRect.verticalScrollbar.value = 0f;

            MqttOldMessage = MqttMessage;
        }

        //send message on Enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("Enter Pressed!");
            SendTextOnClick();
        }
        Canvas.ForceUpdateCanvases();
    }
}
