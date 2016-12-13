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


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
