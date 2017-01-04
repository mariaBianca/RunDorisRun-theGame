using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChatBoxFunctions : MonoBehaviour {
	//using serialiyefields to avoid other scrips accesing the variables  
	[SerializeField] ContentSizeFitter contentSizeFitter;
	//layout controller, controlls the size of the layout elements 
	[SerializeField] Text showHideButtonText;
	//to show the user whether the chat is showing or hiding it will say Hidde chat or show chat 
	[SerializeField] Transform messageParentPanel;
	//Using transform instead of GAmeObject to set the position and scale of the message that will show
	[SerializeField] GameObject newMessagePrefab;
	//variable that it will contain the new message
	bool isChatShowing = false; //manipulation the 
	string message = "";//message define as an empty string (SetMessage)

	//calling toggleChat in the start functipon to make sure that everyrhing start whit the 
	//showing or hiding the chat 

	void Start () {
		ToggleChat ();
	}

	public void ToggleChat (){
		isChatShowing = !isChatShowing;
		//if the chat is showing the contentsiyefitter will be the preferednsize
		if(isChatShowing){
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			showHideButtonText.text = "Hide Chat";
			//if the chat is showing it will appear hide chat
		} else {
			//if the chat is not showing it will be shown with his mininum size
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
			showHideButtonText.text = "Show Chat";
			//if the chat is not showing it will appeat showchat
		}
	}


	//this methos will take the string as a message, the variable message in this scrip will be equal to the
	//incoming message
	    public void SetMessage (string message){
		this.message = message;
	}

	//the ShowMessage function will show the 
	public void ShowMessage (){
		if(message != ""){
			GameObject clone = (GameObject) Instantiate (newMessagePrefab);//it will instantiate the variable message (above)
			clone.transform.SetParent (messageParentPanel);// setting the message to the message parent panel in the GUI
			clone.transform.SetSiblingIndex (messageParentPanel.childCount - 2);//setting the message above the parent (Inputfield)
			clone.GetComponent<MessageFunctions>().ShowMessage (message);// calling the messageFunction scrip with the showMessage
			//function 
					}
	}
}