using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class Register : MonoBehaviour {
    public GameObject email;
    public GameObject password;
    public GameObject confirmPassword;
    private string Email;
    private string Password;
    private string ConfirmPassword;
    private string form;
    private bool EmailValid = false;


    // Use this for initialization
    void Start()
    {

    }

    //public void Register() {
    //}

	
	// Update is called once per frame
	void Update () {


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
        Email = email.GetComponent<InputField>().text;
        Password = password.GetComponent<InputField>().text;
        ConfirmPassword = confirmPassword.GetComponent<InputField>().text;

     }
}
