using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {

	public Text scoreText;




	[Command]
	public void CmdUpdateScore(){
		string scoreText="";
		
		foreach (TankManager tm in GameManager.m_Tanks) {
			scoreText+=tm.m_PlayerName + ": " + tm.m_Wins + "\n";
		}

		RpcUpdateScore (scoreText);
	}

	[ClientRpc]
	public void RpcUpdateScore(string newScore){
		scoreText.text = newScore;

	}


}
