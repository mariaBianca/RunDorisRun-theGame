/**
Script used for fetching the high scores.
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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Text;
using System.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class HighScore : MonoBehaviour
{
    public Text Score1Text;
    public Text Score2Text;
    public Text Score3Text;
    public Text UserName;

    private string fetchedScore1 = "";
    private string fetchedScore2 = "";
    private string fetchedScore3 = "";

    private MqttClient mqttClient;
    private string MqttSubscribedMessage = "";

    List<ScoreObject> scoreObject;
    Boolean scoreget = true;

    // Use this for initialization
    void Start()
    {
        UserName.text = "logged in as:" + PlayerPrefs.GetString("playerName");

        //create an mqtt client instance
        //mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
        mqttClient = new MqttClient(IPAddress.Parse("54.154.153.243"), 1883, false, null); //PRATA

        string clientId = Guid.NewGuid().ToString();

        //connect with user, password
        //mqttClient.Connect(clientId, "theHub", "theHub");
        mqttClient.Connect(clientId);

        //Send mqtt message that asks for the scores
        mqttClient.Publish("thehub/rundorisrun/score/gethighscores", System.Text.Encoding.UTF8.GetBytes("{\"Level\":1}"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);

        mqttClient.Subscribe(new string[] { "thehub/rundorisrun/score/gethighscores" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        //subscribe and get the latest MQTT messages
        if (scoreget)
        {
            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }

        //string jsonScoreArray = "[{\"Score_ID\":\"the_best12313\",\"Score\":790,\"Player_ID\":\"985454118\",\"Level_ID\":1}," +
        //    "{\"Score_ID\":\"best!!!player:D\",\"Score\":560,\"Player_ID\":\"987654988\",\"Level_ID\":1}," +
        //    "{\"Score_ID\":\"Player_11\",\"Score\":550,\"Player_ID\":\"984654987\",\"Level_ID\":1}]";


        //scoreObject = JsonConvert.DeserializeObject<List<ScoreObject>>(MqttSubscribedMessage);




    }

    // Update is called once per frame
    void Update()
    {

        //when the array of Json Score object is received, modify the score text fields
        if (scoreget == true && MqttSubscribedMessage.StartsWith("[") && MqttSubscribedMessage.EndsWith("]") && MqttSubscribedMessage.Contains("Score_ID"))
        {
            scoreget = false;
            //var obj = JToken.Parse(MqttSubscribedMessage);
            scoreObject = JsonConvert.DeserializeObject<List<ScoreObject>>(MqttSubscribedMessage);
            Score1Text.text = scoreObject[0].Score_ID + "  :  " + scoreObject[0].Score;
            Score2Text.text = scoreObject[1].Score_ID + "  :  " + scoreObject[1].Score;
            Score3Text.text = scoreObject[2].Score_ID + "  :  " + scoreObject[2].Score;
            Debug.Log("Scores fetched and set sucessfully!");
        }

    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        MqttSubscribedMessage = "" + System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
    }
}
