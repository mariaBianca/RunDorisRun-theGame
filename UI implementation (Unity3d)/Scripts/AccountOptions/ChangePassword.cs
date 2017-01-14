/**
Script used to change the password.
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
using UnityEngine.UI;
using System.Text;
using System;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;

public class ChangePassword : MonoBehaviour {

    //SerializeField makes private vars visible to Unity
    [SerializeField]
    private InputField currentPass;
    [SerializeField]
    private InputField newPass;
    [SerializeField]
    private InputField confirmNewPass;
    [SerializeField]
    private Text notificationText;

    private Color warningColor;     //104, 1, 1
    //(680101FF) - hex
    private Color approvalColor;    //240, 255, 0
    //(F0FF00FF) - hex

    private MqttClient mqttClient;

    private string hashedPass;

	// Use this for initialization
	void Start () {

        notificationText.text = "";
        //hashedPass = PlayerPrefs.GetString("hashedPass");

        mqttClient = new MqttClient(IPAddress.Parse("129.16.155.34"), 1883, false, null);
        string clientId = Guid.NewGuid().ToString();
        mqttClient.Connect(clientId, "theHub", "theHub");

        ////creating the warning reddish color
        //warningColor.r = 104;
        //warningColor.g = 1;
        //warningColor.b = 1;
        ////creating the approval yellowish color
        //approvalColor.r = 240;
        //approvalColor.g = 255;
        //approvalColor.b = 0;
	
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentPass.text != "" && newPass.text != "" && confirmNewPass.text != "")
            {
                changePw();
            }
            else
            {
                notificationText.text = "please fill out all fields!";
            }
        }
    }

    string Hash(string data)
    {

        var bytes = new UTF8Encoding().GetBytes(data);
        byte[] hashBytes;
        using (var algorithm = new System.Security.Cryptography.SHA512Managed())
        {
            hashBytes = algorithm.ComputeHash(bytes);
        }
        return Convert.ToBase64String(hashBytes);
    }

    //a method for changing the password
    public void changePw()
	{
        if (currentPass.text != "" && newPass.text != "" && confirmNewPass.text != "")
        {
            if (Hash(currentPass.text) == PlayerPrefs.GetString("hashedPass"))
            {
                if (newPass.text.Length >= 6)
                {
                    if (newPass.text == confirmNewPass.text)
                    {
                        string newHashedPass = Hash(newPass.text);
                        string messageJson = "{\"_id\": \"" + PlayerPrefs.GetString("playerID") + "\" " +
                                            "\"name\": \"" + PlayerPrefs.GetString("playerName") + "\" " +
                                            ",\"hashedEmail\": \"" + PlayerPrefs.GetString("hashedEmail") + "\" " +
                                            ",\"hashedPass\": \"" + newHashedPass + "\"}";

                        //add it to the database through MQTT
                        if (mqttClient.IsConnected)
                        {
                            PlayerPrefs.SetString("hashedPass", newHashedPass);
                            PlayerPrefs.Save();
                            mqttClient.Publish("thehub/rundorisrun/player/changepw", System.Text.Encoding.UTF8.GetBytes(messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
                            
                        }
                        else
                        {
                            Debug.LogWarning("MQTT not connected!");
                        }

                        //notificationText.color = approvalColor;
                        notificationText.text = "password successfully changed!";
                    }
                    else
                    {
                        //notificationText.color = warningColor;
                        notificationText.text = "new password fields don't match";
                    }
                }
                else
                {
                    //notificationText.color = warningColor;
                    notificationText.text = "new password needs to be at least 6 characters long";

                }
            }
            else
            {
                //notificationText.color = warningColor;
                notificationText.text = "old password is invalid!";
            }
        }
        else
        {
            notificationText.text = "please fill out all fields!";
        }
    }
        
}
