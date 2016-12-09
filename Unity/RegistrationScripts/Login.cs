
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{

    private MqttClient mqttClient;

    public GameObject email;
    public GameObject password;
    public GameObject validationText;

    private string MqttMailCheck = "";
    private string Email = "";
    private string Password = "";
    private string ValidationText = "";



    // Use this for initialization
    void Start()
    {
        MqttMailCheck = "";
        mqttClient = new MqttClient(IPAddress.Parse("127.0.0.1"), 1883, false, null);
        string clientId = Guid.NewGuid().ToString();
        mqttClient.Connect(clientId);
        //getMesssage subscribed
        mqttClient.Subscribe(new string[] { "check" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        mqttClient.MqttMsgPublishReceived += MqttMsgGetString;

    }

    public void LoginUser()
    {

        string hashedEmail = "";
        string hashedPassword = "";

        //check if email and the password already exist
        if (Email != "")
        {
            hashedEmail = Hash(Email);
            hashedPassword = Hash(Password);
            Debug.Log(hashedEmail);
            Debug.Log(hashedPassword);
            if (checkEmail(hashedEmail, hashedPassword))
            {
                Debug.Log("ITS works");
                SceneManager.LoadScene("Game Options");
            }
            else
            {
                ValidationText = "The fields do not match with the database.";
                Debug.LogWarning(ValidationText);
                validationText.GetComponent<Text>().text = ValidationText;
            }
        }
        else
        {
            ValidationText = "Email field is empty, please try again!";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;
        }

    }

    //returns a boolean that says if the email exists in the db or not
    bool checkEmail(string hashedEmail, string hashedPassword)
    {

        if ((MqttMailCheck.Contains(hashedEmail)) && (MqttMailCheck.Contains(hashedPassword)))
        {
            return true;
        }

        string messageJson = "{\"hashedEmail\": \"" + hashedEmail + "\" }";
        if (mqttClient.IsConnected)
        {
            mqttClient.Publish("check", System.Text.Encoding.UTF8.GetBytes(messageJson), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }
        else
        {
            Debug.Log("MQTT connection does not work.");
        }
        return false;
    }

    //hash the password, email or any other data inputted using sha512
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

    // Update is called once per frame
    void Update()
    {
        // Move to the next button with TAB.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (email.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
        }

        //get the field of the components
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Email != "" && Password != "")
            {
                LoginUser();
            }
        }

    }

    void MqttMsgGetString(object sender, MqttMsgPublishEventArgs e)
    {
        MqttMailCheck = System.Text.Encoding.UTF8.GetString(e.Message);
    }

}
