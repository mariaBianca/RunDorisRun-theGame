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
//using System;

public class FBScript : MonoBehaviour
{


    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUsername;
    public GameObject DialogProfilePic;
    public GameObject DialogEmail;
    public GameObject DialogId;

    //Initialize the function
    void Awake()
    {

        //FB.Init(setInit, onHideUnity); 
        FBManager.Instance.InitFB();
        DealWithFBMenus(FB.IsLoggedIn);


    }

    //Login function
    public void FBlogin()
    {
        List<string> permissions = new List<string>();
        //Facebook in their documentations recommends to not ask too many permissions when you want to login
        //when you want to extra things down the line, thus only few permissions have been added
        permissions.Add("public_profile");
        permissions.Add("email");

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
                FBManager.Instance.IsLoggedIn = true;
                FBManager.Instance.GetProfile();
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
            if (FBManager.Instance.ProfileName != null)
            {
                Text UserName = DialogUsername.GetComponent<Text>();
                UserName.text = "hi, " + FBManager.Instance.ProfileName;
            }
            else
            {
                StartCoroutine("WaitForProfileName");
            }

            if (isLoggedIn)
            {
                DialogLoggedIn.SetActive(true);
                DialogLoggedOut.SetActive(false);
                if (FBManager.Instance.ProfilePic != null)
                {
                    Image ProfilePic = DialogProfilePic.GetComponent<Image>();
                    ProfilePic.sprite = FBManager.Instance.ProfilePic;
                }
                else
                {
                    StartCoroutine("WaitForProfilePic");
                }

            }
            else
            {

                DialogLoggedIn.SetActive(false);
                DialogLoggedOut.SetActive(true);
            }
        }
    }
    IEnumerator WaitForProfileName()
    {
        while (FBManager.Instance.ProfileName == null)
        {
            yield return null;
        }
        DealWithFBMenus(FB.IsLoggedIn);
    }

    IEnumerator WaitForProfilePic()
    {
        while (FBManager.Instance.ProfilePic == null)
        {
            yield return null;
        }
        DealWithFBMenus(FB.IsLoggedIn);
    }
    public void Share()
    {
        FBManager.Instance.Share();
    }

}