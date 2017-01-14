/**
Script used to control the settings of the game when the player is dead.
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

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private MqttClient mqttClient;

    private string scoreJson;
    private Boolean scoreTrigger = false;

    Animator anim;

    void Start()
    {
        //mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
        mqttClient = new MqttClient(IPAddress.Parse("54.154.153.243"), 1883, false, null); //PRATA

        string clientId = Guid.NewGuid().ToString();
        //mqttClient.Connect(clientId, "theHub", "theHub");
        mqttClient.Connect(clientId);
    }


    void Awake()
    {
        anim = GetComponent<Animator>();

    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
			anim.SetTrigger("GameOver");
			int score = ScoreManager.score;

            //temp
            //int i = Random.Range(1000000, 9999999);

            scoreJson = "{\"Score_ID\": \"" + PlayerPrefs.GetString("playerName") + "\"" +
                       ",\"Score\": " + score +
                         ",\"Player_ID\": \"" + PlayerPrefs.GetString("playerID") + "\"" +
                        ",\"Level_ID\": " + 1 + "}";
            if (!scoreTrigger)
            {
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish("thehub/rundorisrun/score/setscore", System.Text.Encoding.UTF8.GetBytes(scoreJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
                    scoreTrigger = true;
                }
                else
                {
                    Debug.Log("Score could not be sent through MQTT.");
                } 
            }
           
        }
    }
}
