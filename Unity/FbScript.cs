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

public class FbScript : MonoBehaviour {

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
		permissions.Add("public_profile");

		FB.LogInWithReadPermissions (permissions, AuthCallBack);
	}

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

		}
	}
}
