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



public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private MqttClient mqttClient;

    private string scoreJson;
    private string playerTag = "Player_1";
    private Boolean scoreTrigger = false;

    Animator anim;

    void Start()
    {
        mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);
        string clientId = Guid.NewGuid().ToString();
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
            scoreJson = "{\"Score_ID\": \"" + playerTag + "\"" +
                       ",\"Score\": " + score +
                         ",\"Player_ID\": \"" + 111 + "\"" +
                        ",\"Level_ID\": " + 1 + "}";
            if (!scoreTrigger)
            {
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish("test", System.Text.Encoding.UTF8.GetBytes(scoreJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
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
