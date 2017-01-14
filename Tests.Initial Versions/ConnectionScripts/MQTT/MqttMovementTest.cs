/**
*Script used to connect test the Unity3D to MongoDB connection.
*This script utilises the M2Mqtt client library for the MQTT protocol
*
*@author TheHub
*
*This script uses some preincluded Unity3D Assets (such as libraries) in order to work.
*
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class MqttMovementTest : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private mqttTest mqttTest;
        private String mqttCommand;
        private MqttMsgPublishEventArgs mqttReceived;
        private MqttClient mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
        private string clientId = Guid.NewGuid().ToString();
        static String mqttMessageRec = "";

        private void Awake()
        {
            mqttClient.Connect(clientId, "owntracks", "theHub");
            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            mqttClient.Subscribe(new string[] { "test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            m_Character = GetComponent<PlatformerCharacter2D>();

        }

        String getMqttCommand()
        {
            return mqttMessageRec;
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //mqttMessageRec = System.Text.Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(mqttMessageRec);
            //Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
        }

        void jumpOnMqtt(object sender, MqttMsgPublishEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetString(e.Message) == "Jump")
            {
                m_Jump = true;
            }
            //m_Jump = false;
        }


        private void Update()
        {
            //mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            Console.WriteLine(mqttMessageRec);
            if (!m_Jump)
            {
                mqttClient.MqttMsgPublishReceived += jumpOnMqtt;
                // Read the jump input in Update so that MQTT signals aren't missed.
                if (mqttMessageRec == "Jump")
                {
                    //m_Jump = true;
                    //Debug.Log("Received: " + mqttMessageRec);
                }
                
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs (refers to the keyboard inputs).
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}