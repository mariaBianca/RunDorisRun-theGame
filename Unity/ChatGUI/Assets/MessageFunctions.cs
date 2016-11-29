using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageFunctions : MonoBehaviour {
	// add the messages that have been writing in the chatbox an put them up

	[SerializeField] Text text;// field for the inspector 
	 

	public void ShowMessage (string message){
		//it will take the test and put it to the message 
		text.text = message;
	}

	public void HideMessage (){
		// it will hide the message and then desdroyed
		Destroy (gameObject);
	}

}