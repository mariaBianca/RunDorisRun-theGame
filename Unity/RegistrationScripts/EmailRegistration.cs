using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour
{
    public GameObject email;
    public GameObject password;
    public GameObject confirmPassword;
    private string Email;
    private string Password;
    private string ConfirmPassword;
    private string form;
    private bool EmailValid = false;
    private string[] Characters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                   "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                                   "1","2","3","4","5","6","7","8","9","0","_","-"};


    // Use this for initialization
    void Start()
    {

    }


    // Adds the register button
    public void RegisterButton()
    {
        //  Modify this later, for example: if (System.IO.File.Exists(@""))
        print("Registration Successful");

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
            if (password.GetComponent<InputField>().isFocused)
            {
                confirmPassword.GetComponent<InputField>().Select();
            }


        }
        // Get the components.
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (Email != "" && Password != "" && Password != "" && Password != "")
            {
                RegisterButton();
            }
        }

        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfirmPassword = confirmPassword.GetComponent<InputField>().text;

    }

    void EmailValidation() {
        bool StartsWith = false;
        bool EndsWith = false;
        for (int i = 0; i < Characters.Length; i++) {
            if (Email.StartsWith(Characters[i])) {
                StartsWith = true;
            }
        }
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Email.EndsWith(Characters[i]))
            {
                EndsWith = true;
            }
        }
        if(StartsWith == true && EndsWith == true) {
            EmailValid = true;
            }


    }


}
