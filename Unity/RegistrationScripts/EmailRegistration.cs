/**
*Script used to connect to Facebook.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class EmailRegistration : MonoBehaviour
{
    public GameObject email;
    public GameObject password;
    public GameObject confirmPassword;
    public GameObject username;
    public GameObject validationText;

    private string Email;
    private string Username = "";
    private string Password = "";
    private string ConfirmPassword = "";
    private string form = "";
    private string ValidationText;
    // private string[] Characters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
    //                                "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
    //                                "1","2","3","4","5","6","7","8","9","0","_","-"};


    // Use this for initialization
    void Start()
    {

    }


    // Adds the register button and its functionalities
    public void RegisterButton()
    {
        //print ("Registration Successful");
        bool UN = false; //boolean for the username
        bool EMV = false; //boolean for the email validation
        bool EM = false; //boolean for the email existance
        bool PW = false; //boolean for th	e password
        bool CPW = false; //boolean for the confirm of password


        //check if username is valid
        if (Username != "")
        {
            UN = true;
        }
        else
        {
            ValidationText = "Username field is empty!";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;
        }

        //check if email already exists
        if (Email != "")
        {
            if (!System.IO.File.Exists(@"C:/Users/lenovo/Desktop/UnityTest/" + Email + ".txt"))
            {
                EM = true;
            }
            else
            {
                ValidationText = "Email is already in use.";
                Debug.LogWarning(ValidationText);
                validationText.GetComponent<Text>().text = ValidationText;
            }
        }
        else
        {
            ValidationText = "Email field is empty.";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;
        }

        //check if email is valid
        if (Email != "")
        {
            if (Email.Contains("@"))
            {
                if (Email.Contains("."))
                {
                    EMV = true;
                }
                else
                {
                    ValidationText = "Email field is incorrect.";
                    Debug.LogWarning(ValidationText);
                    validationText.GetComponent<Text>().text = ValidationText;

                }
            }
            else
            {
                ValidationText = "Email field is incorrect.";
                Debug.LogWarning(ValidationText);
                validationText.GetComponent<Text>().text = ValidationText;
            }
        }
        else
        {
            ValidationText = "Email field is empty!";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;
        }

        //check if the password is valid
        if (Password != "")
        {
            if (Password.Length > 5)
            {
                PW = true;
            }
            else
            {
                ValidationText = "The password must be at least 6 characters.";
                Debug.LogWarning(ValidationText);
                validationText.GetComponent<Text>().text = ValidationText;
            }
        }
        else
        {
            ValidationText = "Password field is empty!";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;
        }

        //check if password confirmation is valid (same with the password
        if (ConfirmPassword != "")
        {
            if (ConfirmPassword == Password)
            {
                CPW = true;
            }
            else
            {
                ValidationText = "Password and password confirmation field do not match!";
                Debug.LogWarning(ValidationText);
                validationText.GetComponent<Text>().text = ValidationText;
            }
        }
        else
        {
            ValidationText = "Password confirmation field is empty!";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;
        }

        //encode the password
        if (UN == true && EM == true && PW == true && CPW == true && EMV == true)
        {
            bool clear = true;
            int i = 1;
            foreach (char c in Password)
            {
                if (clear)
                {
                    Password = "";
                    clear = false;
                }
                i++;
                char Encrypted = (char)(c * i);
                Password += Encrypted.ToString();
            }
            form = (Username + "  " + Email + "  " + Password);
            //add it to a form
            System.IO.File.WriteAllText(@"C:/Users/lenovo/Desktop/UnityTest/" + Email + ".txt", form);

            //clear all the registration fields 
            email.GetComponent<InputField>().text = "";
            password.GetComponent<InputField>().text = "";
            confirmPassword.GetComponent<InputField>().text = "";
            email.GetComponent<InputField>().text = "";
            validationText.GetComponent<Text>().text = "";
            ValidationText = "";
            UN = false;
            EM = false;
            PW = false;
            CPW = false;
            ValidationText = "Registration Completed!";
            Debug.LogWarning(ValidationText);
            validationText.GetComponent<Text>().text = ValidationText;

        }
    }


    // Update is called once per frame
    void Update()
    {
        // Move to the next button with TAB.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (email.GetComponent<InputField>().isFocused)
            {
                username.GetComponent<InputField>().Select();
            }
            if (username.GetComponent<InputField>().isFocused)
            {
                password.GetComponent<InputField>().Select();
            }
            if (password.GetComponent<InputField>().isFocused)
            {
                confirmPassword.GetComponent<InputField>().Select();
            }
        }

        //get the field of the components
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfirmPassword = confirmPassword.GetComponent<InputField>().text;
        Username = email.GetComponent<InputField>().text;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Email != "" && Username != "" && Password != "" && ConfirmPassword != "")
            {
                RegisterButton();
            }
        }

    }

}
