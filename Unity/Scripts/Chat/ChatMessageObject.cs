/**
*Script used to define the object of a message in the chat.
*@author TheHub
*DIT029 H16 Project: Software Architecture for Distributed Systems
*University of Gothenburg, Sweden 2016
*/

using UnityEngine;
using System.Collections;

public class ChatMessageObject : MonoBehaviour {

    public string text { get; set; }
    public string user { get; set; }
    public string messageId { get; set; }

    public ChatMessageObject() { }

    public ChatMessageObject(string Text, string User, string MessageId)
    {
        text = Text;
        user = User;
        messageId = MessageId;
    }
		
}
