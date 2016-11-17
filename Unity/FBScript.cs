/**
*Script used to connect to Facebook.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.UI;

public class FBScript : MonoBehaviour
{


    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUsername;
    public GameObject DialogProfilePic;

    //Initialize the function
    void Awake()
    {
        FB.Init(setInit, onHideUnity);
    }

    //set the Facebook initialization and log accordingly
    void setInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is logged in.");
        }
        else
        {
            Debug.Log("Facebook is not logged in.");
        }
        DealWithFBMenus(FB.IsLoggedIn);

    }

    //if the game is shown continue, otherwise pause the game
    void onHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //Login function
    public void FBlogin()
    {
        List<string> permissions = new List<string>();
        //Facebook in their documentations recommends to not ask too many permissions when you want to login
        //when you want to extra things down the line, thus only few permissions have been added
        permissions.Add("public_profile");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    //deal with errors
    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Facebook is logged in.");
            }
            else
            {
                Debug.Log("Facebook is not logged in.");
            }
            DealWithFBMenus(FB.IsLoggedIn);

        }
    }

    //change Facebook menus accordingly
    void DealWithFBMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            DialogLoggedIn.SetActive(true);
            DialogLoggedOut.SetActive(false);
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            DialogLoggedIn.SetActive(false);
            DialogLoggedOut.SetActive(true);
        }
    }

    //method that displays the grabbed username
    void DisplayUsername(IResult result)
    {
        Text UserName = DialogUsername.GetComponent<Text>();
        if (result.Error == null)
        {
            UserName.text = "Hi there, " + result.ResultDictionary["first_name"];
        }
        else
        {
            Debug.Log(result.Error);
        }

    }
    //method that displays the profile picture
    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Image ProfilePic = DialogProfilePic.GetComponent<Image>();
            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
    }
}