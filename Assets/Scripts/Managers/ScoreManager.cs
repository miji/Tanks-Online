using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {

	public Text scoreText;
	public GameManager gameManager;





	[Command]
	public void CmdUpdateScore(){

		string playersText1="";
		string playersText2="";

	

		
		foreach (TankManager tm in GameManager.m_Tanks) {
			if(tm.m_Team==1){
				playersText1+="<color=#"+ ColorUtility.ToHtmlStringRGB (tm.m_Setup.m_Color) +">" +tm.m_PlayerName+ "</color>\n";

			}
			else if (tm.m_Team==2){
				playersText2+="<color=#"+ ColorUtility.ToHtmlStringRGB (tm.m_Setup.m_Color) +">" +tm.m_PlayerName+ "</color>\n";

			}
		}

		RpcUpdateScore (gameManager.Wins1, gameManager.Wins2, playersText1, playersText2);
	}

	[ClientRpc]
	public void RpcUpdateScore(int score1, int score2, string players1, string players2){

		if (!FindObjectOfType<Awareness> ().WhoMembers && !FindObjectOfType<Awareness> ().WhoOtherMembers && !FindObjectOfType<Awareness> ().WhatGroupGoal)
			return;

		string text = "<color=#"+ ColorUtility.ToHtmlStringRGB (Color.blue) + ">TEAM 1";
		if(FindObjectOfType<Awareness>().WhatGroupGoal) text += ": " + score1;
		text+="</color>\n";

		if ((gameManager.GetLocalPlayer ().m_Team == 1 && FindObjectOfType<Awareness> ().WhoMembers) ||
			(gameManager.GetLocalPlayer ().m_Team == 2 && FindObjectOfType<Awareness> ().WhoOtherMembers)) {
			text+=players1;
		}

		text+="\n";

		text += "<color=#"+ ColorUtility.ToHtmlStringRGB (Color.red) + ">TEAM 2";
		if(FindObjectOfType<Awareness>().WhatGroupGoal) text += ": " + score2;
		text+="</color>\n";

		if ((gameManager.GetLocalPlayer ().m_Team == 2 && FindObjectOfType<Awareness> ().WhoMembers) ||
		    (gameManager.GetLocalPlayer ().m_Team == 1 && FindObjectOfType<Awareness> ().WhoOtherMembers)) {
			text+=players2;
		}



		scoreText.text = text;

	}


}
