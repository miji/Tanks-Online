using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ChatManager : NetworkBehaviour {


	public GameManager gameManager;
	public Text chatText;
	public InputField chatInput;
	public int messagesToShow=5;

	List<string> chatMessages;

	void Start(){
		chatMessages = new List<string> ();
		if(FindObjectOfType<Awareness> ().HowInnerCommunication) ShowChatMessage("Press c to chat to talk to your team members",0);
		if(FindObjectOfType<Awareness> ().HowOuterCommunication) ShowChatMessage("Start the message with \"-\" to talk to everybody",0);
	}


	public void SendChatMessage(){

		TankManager localPlayer = gameManager.GetLocalPlayer ();

		string sender = GameManager.ColorPlayerStirng(localPlayer.m_PlayerName);
		string message = sender + ": " + chatInput.text;

		localPlayer.m_Instance.GetComponent<TankMessage> ().SendChatMessage (message, gameManager.GetLocalPlayer().m_Team);

		chatInput.text = "";

		chatInput.DeactivateInputField ();

	}



	public void ShowChatMessage(string message, int senderTeam){

		int localTeam = gameManager.GetLocalPlayer ().m_Team;

		if (senderTeam == 0 || message.Contains(">: -") || senderTeam == localTeam) {
			chatMessages.Add (message.Replace(">: -", ">: "));
			chatText.text = List2String ();
		}

	}



	public string List2String(){
	
		List<string> tempList = chatMessages;
		//tempList.Reverse ();
		string [] messages = tempList.ToArray ();

		int c=0;
		string res="";

		foreach (string message in messages) {
			if(c<messagesToShow){
				res+=message;
				c++;
				if(c!=messagesToShow || c==messages.Length){
					res+="\n";
				} else{
					break;
				}
			}
		}

		return res;



	}
}
