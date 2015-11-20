using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class TankMessage : NetworkBehaviour
{


	public static void ClearMessages ()
	{
		GameObject.Find ("MessagesText").GetComponent<Text> ().text = "";
		GameObject.Find ("NewMessageText").GetComponent<Text> ().text = "";
	}

	[Command]
	public void CmdSendMessage (string message)
	{
		RpcShowMessage (message);
	}


	Coroutine sendingNewMessage;
	[ClientRpc]
	public void RpcShowMessage (string message)
	{
		string messageToShow="";

		string current = GameObject.Find ("MessagesText").GetComponent<Text> ().text;

		if (FindObjectOfType<Awareness> ().WhatTask) {
			if(sendingNewMessage!=null) StopCoroutine(sendingNewMessage);
			sendingNewMessage=StartCoroutine(SendNewMessage(message));
			if (FindObjectOfType<Awareness> ().WhenEventHistory)
				messageToShow = System.DateTime.Now.ToShortTimeString()+ " ";

			messageToShow += message;

			if (FindObjectOfType<Awareness> ().WhatTaskHistory){
				messageToShow = messageToShow + "\n" + current;
			}
		}
		GameObject.Find ("MessagesText").GetComponent<Text> ().text = messageToShow;

	}

	IEnumerator SendNewMessage(string newMessage){

		Text newMessageText = GameObject.Find ("NewMessageText").GetComponent<Text> ();

		newMessageText.canvasRenderer.SetAlpha (1.0f);
		newMessageText.text = newMessage;
		yield return new WaitForSeconds(1);
		newMessageText.CrossFadeAlpha (0.0f, 2, false);
	}

	public void SendChatMessage(string message, int team){
		CmdSendChatMessage (message, team);
	}

	[Command]
	public void CmdSendChatMessage(string message, int team){
		RpcShowChatMessage (message, team);
	}

	[ClientRpc]
	public void RpcShowChatMessage(string message, int team){
		GameObject.FindObjectOfType<ChatManager> ().ShowChatMessage (message, team);
	}
}
