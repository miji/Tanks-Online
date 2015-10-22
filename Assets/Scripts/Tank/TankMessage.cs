using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class TankMessage : NetworkBehaviour
{


	public static void ClearMessages ()
	{
		GameObject.Find ("MessagesText").GetComponent<Text> ().text = "";
	}

	[Command]
	public void CmdSendMessage (string message)
	{
		RpcShowMessage (message);
	}

	[ClientRpc]
	public void RpcShowMessage (string message)
	{
		string current = GameObject.Find ("MessagesText").GetComponent<Text> ().text;

		string messageToShow="";

		if (FindObjectOfType<Awareness> ().WhatTask) {
			messageToShow = message;
			if (FindObjectOfType<Awareness> ().WhatTaskHistory){
				messageToShow = message + "\n" + current;
			}
		}
		GameObject.Find ("MessagesText").GetComponent<Text> ().text = messageToShow;

	}

}
