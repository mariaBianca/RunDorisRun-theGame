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

public class FBScript : MonoBehaviour {


	public GameObject DialogLoggedIn;
	public GameObject DialogLoggedOut;
	public GameObject DialogFirstName;
	public GameObject DialogLastName;
	public GameObject DialogEmail;
	public GameObject DialogProfilePic;

	//Initialize the function
	void Awake ()
	{
		FB.Init (setInit, onHideUnity);
	}

	//set the Facebook initialization and log accordingly
	void setInit()
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("Facebook is logged in.");
		} else {
			Debug.Log ("Facebook is not logged in.");
		}
		DealWithFBMenus (FB.IsLoggedIn);

	}

	//if the game is shown continue, otherwise pause the game
	void onHideUnity(bool isGameShown)
	{
		if (!isGameShown) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}
	}

	//Login function
	public void FBlogin()
	{
		List<string> permissions = new List<string> ();
		//Facebook in their documentations recommends to not ask too many permissions when you want to login
		//when you want to extra things down the line, thus only few permissions have been added
		permissions.Add("email");
		permissions.Add("public_profile");
		//permissions.Add ("user_friends");

		FB.LogInWithReadPermissions (permissions, AuthCallBack);
	}

	//deal with errors
	void AuthCallBack(IResult result)
	{
		if (result.Error != null){
			Debug.Log(result.Error);
		} else {
			if (FB.IsLoggedIn){
				Debug.Log("Facebook is logged in.");
			} else {
				Debug.Log("Facebook is not logged in.");
			}
		DealWithFBMenus (FB.IsLoggedIn);

		}
	}

	//change Facebook menus accordingly
	void DealWithFBMenus(bool isLoggedIn)
	{
		if (isLoggedIn) {
			DialogLoggedIn.SetActive (true);
			DialogLoggedOut.SetActive (false);
			FB.API ("/me?fields=email", HttpMethod.GET, DisplayEmail);
			FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayFirstName);
			FB.API ("/me?fields=last_name", HttpMethod.GET, DisplayLastName);
			//FB.API ("/me?fields=user_about_me", HttpMethod.GET, DisplayAboutMe);
		} else {
			DialogLoggedIn.SetActive (false);
			DialogLoggedOut.SetActive (true);
		}
	}

	//method that displays the grabbes and displays the email
	void DisplayEmail(IResult result)
	{
		Text email = DialogEmail.GetComponent<Text> ();
		if (result.Error == null) {
			email.text = "Email:" + result.ResultDictionary["email"];

		} else {
			Debug.Log (result.Error);
		}
	}

	//method that displays the grabbes and displays the first name
	void DisplayFirstName(IResult result)
	{
		Text firstName = DialogFirstName.GetComponent<Text> ();
		if (result.Error == null) {
			firstName.text = "First Name:" + result.ResultDictionary["first_name"];

		} else {
			Debug.Log (result.Error);
		}
	}

	//method that displays the grabbes and displays the first name
	void DisplayLastName(IResult result)
	{
		Text lastName = DialogLastName.GetComponent<Text> ();
		if (result.Error == null) {
			lastName.text = "Last Name:" + result.ResultDictionary["last_name"];

		} else {
			Debug.Log (result.Error);
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
