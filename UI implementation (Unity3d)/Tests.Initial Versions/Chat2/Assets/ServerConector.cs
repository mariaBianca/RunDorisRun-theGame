using UnityEngine;
using System.Collections;
using System;


public class ServerConector : MonoBehaviour 
{
	private string serverName="", numPlayers="0", port ="129.16.155.34";
	private Rect windowRect=new Rect (0,0,400,400);

	private void OnGUI()
	{
		windowRect = GUI.Window (0, windowRect, windowProp, "Servers");

		if (Network.peerType==NetworkPeerType.Disconnected)
		{	
			GUILayout.Label ("Server Name");
			serverName = GUILayout.TextField (serverName);

			GUILayout.Label ("Port");
			port = GUILayout.TextField (port);

			GUILayout.Label ("Number of Players");
			numPlayers = GUILayout.TextField (numPlayers);


			if (GUILayout.Button ("Create Server")) 
			{
				try
				{
					Network.InitializeSecurity();
					Network.InitializeServer(int.Parse(numPlayers), int.Parse(port), !Network.HavePublicAddress());
					MasterServer.RegisterHost("Testing", serverName);
				}
				catch (Exception) 
				{
					print("Please Type in the IP adress for the port and the number of players");
				}
			}			
		}
		else
		{
			if (GUILayout.Button("Disconnect"))
			{
				Network.Disconnect();
			}	
		}
	}
	private void windowProp(int id)
	{
		if (GUILayout.Button ("Refresh"))
		{
			MasterServer.RequestHostList ("Testing");
		}
		GUILayout.BeginHorizontal ();
		GUILayout.Box ("Server Name");
		GUILayout.EndHorizontal ();
		if (MasterServer.PollHostList().Length != 0) 
		{
			HostData[] data = MasterServer.PollHostList();
			foreach (HostData c in data)
			{
				GUILayout.BeginHorizontal ();
				GUILayout.Box (c.gameName);
				if (GUILayout.Button ("Connect"))
				{
					Network.Connect (c);
				}
				GUILayout.EndHorizontal ();
			}
		}

		GUI.DragWindow (new Rect (0, 0, Screen.width, Screen.height));

	}
}
