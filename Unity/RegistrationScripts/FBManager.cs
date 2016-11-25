using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.UI;


public class FBManager : MonoBehaviour
{

    private static FBManager _instance;

    public static FBManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject facebookmanager = new GameObject("FBManager");
                facebookmanager.AddComponent<FBManager>();
            }

            return _instance;
        }
    }

    public bool IsLoggedIn { get; set; }
    public string ProfileName { get; set; }
    public string ProfileId { get; set; }
    public string ProfileEmail { get; set; }
    public Sprite ProfilePic { get; set; }
    public string AppLinkURL { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
        IsLoggedIn = true;
    }
    public void InitFB()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(setInit, onHideUnity);
        }
        else
        {
            IsLoggedIn = FB.IsLoggedIn;
        }
    }
    //set the Facebook initialization and log accordingly
    void setInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is logged in.");
            GetProfile();
        }
        else
        {
            Debug.Log("Facebook is not logged in.");
        }
        IsLoggedIn = FB.IsLoggedIn;

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
    public void GetProfile()
    {
        FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        //    FB.API("/me?fields=email", HttpMethod.GET, DisplayEmail);
        //    FB.API("/me?fields=id", HttpMethod.GET, DisplayId);
        FB.GetAppLink(DealWithAppLink);
    }
    //method that displays the grabbed username
    void DisplayUsername(IResult result)
    {
        // this line can be removed     Text UserName = DialogUsername.GetComponent<Text>();
        if (result.Error == null)
        {
            ProfileName = "" + result.ResultDictionary["first_name"];
        }
        else
        {
            Debug.Log(result.Error);
        }

    }
    //method that displays the grabbed email
    // void DisplayEmail(IResult result)
    // {
    //     // this line can be removed     Text Email = DialogEmail.GetComponent<Text>();
    //    if (result.Error == null)
    //    {
    //        ProfileEmail = "" + result.ResultDictionary["email"];
    //     }
    //     else
    //     {
    //          Debug.Log(result.Error);
    //      }

    // }

    //method that displays the grabbed Id
    //   void DisplayId(IResult result)
    //   {
    //       // this line can be removed      Text Id = DialogId.GetComponent<Text>();
    //       if (result.Error == null)
    //       {
    //            ProfileId = "" + result.ResultDictionary["id"];
    //        }
    //       else
    //        {
    //            Debug.Log(result.Error);
    //       }
    //
    //   }

    //displays the profile picture
    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            ProfilePic = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
        }
    }
    void DealWithAppLink(IAppLinkResult result)
    {
        if (!String.IsNullOrEmpty(result.Url))
        {
            AppLinkURL = "" + result.Url + "";
            Debug.Log(AppLinkURL);
        }
        else
        {
            AppLinkURL = "http://google.com";
        }
    }


    //share something
    public void Share()
    {
        FB.FeedShare(
            string.Empty,
            new Uri(AppLinkURL),
            "Title hehe",
            "Caption here",
            "Text here",
            new Uri("https://upload.wikimedia.org/wikipedia/commons/1/1a/Image_upload_test.jpg"),
            string.Empty,
            ShareCallBack
            );
    }

    //print the result
    void ShareCallBack(IResult result)
    {
        if (result.Cancelled)
        {
            Debug.Log("Share cancelled");
        }
        else if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Error on share!");
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            Debug.Log("Success!");
        }

    }

}
