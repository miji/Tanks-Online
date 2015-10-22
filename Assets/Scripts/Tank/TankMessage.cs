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
		string messageToShow="";

		string current = GameObject.Find ("MessagesText").GetComponent<Text> ().text;

		if (FindObjectOfType<Awareness> ().WhatTask) {
			if (FindObjectOfType<Awareness> ().WhenEventHistory)
				messageToShow = System.DateTime.Now.ToShortTimeString()+ " ";

			messageToShow += message;

			if (FindObjectOfType<Awareness> ().WhatTaskHistory){
				messageToShow = messageToShow + "\n" + current;
			}
		}
		GameObject.Find ("MessagesText").GetComponent<Text> ().text = messageToShow;

	}

}
