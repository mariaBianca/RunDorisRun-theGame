/**
Script used to setup the method of receiving a message in the chat.
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
using System.Collections;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ChatReceive : MonoBehaviour {
    MqttClient mqttClient;
    string MqttMessage = "";
    private string MqttOldMessage = string.Empty;

    private ChatMessageObject ChatMessage;


    [SerializeField]
    private Text userLine;
    [SerializeField]
    private Text chatLine;
    [SerializeField]
    private RectTransform content;

    public ScrollRect chatScrollRect;
    public Scrollbar chatScrollBar;

    // Use this for initialization
    void Start () {
        mqttClient = new MqttClient(IPAddress.Parse("54.154.153.243"), 1883, false, null); //PRATA
        //mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);

        string clientId = Guid.NewGuid().ToString();

        //connect with user, password
        //mqttClient.Connect(clientId, "theHub", "theHub");
        mqttClient.Connect(clientId);


        mqttClient.Subscribe(new string[] { "RunDorisRun/chatroom/potatochat" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    }

    void showLatestMessage()
    {
        ChatMessage = JsonConvert.DeserializeObject<ChatMessageObject>(MqttMessage);

        var newLine = ((GameObject)Instantiate(chatLine.gameObject)).GetComponent<Text>();

        if (ChatMessage.user == PlayerPrefs.GetString("playerName"))
        {
            newLine = ((GameObject)Instantiate(userLine.gameObject)).GetComponent<Text>();
            ChatMessage.user = "Me";
        }
        
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


        Canvas.ForceUpdateCanvases();

        Debug.Log(MqttMessage);

        if (!MqttOldMessage.Equals(MqttMessage) && !MqttMessage.Equals(string.Empty))
        {
            showLatestMessage();

            chatScrollRect.verticalNormalizedPosition = 0.0f;
            chatScrollRect.verticalScrollbar.value = 0f;

            MqttOldMessage = MqttMessage;
        }

        Canvas.ForceUpdateCanvases();
    }

}
