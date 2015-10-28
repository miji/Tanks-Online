using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalManager : MonoBehaviour
{

	public GameManager gameManager;

	public void UpdateGoal ()
	{
		string text = "";
		if (gameManager.GetLocalPlayer ().m_Team == 1) {
			if (FindObjectOfType<Awareness> ().WhatBelonging)
				text += "You are in <color=#" + ColorUtility.ToHtmlStringRGB (Color.blue) + ">Team 1</color>\n\n";
			if (FindObjectOfType<Awareness> ().WhatGroupGoal)
				text += "Your team's goal is to destroy <color=#" + ColorUtility.ToHtmlStringRGB (Color.red) + ">Team 2</color>"; 
		} else if (gameManager.GetLocalPlayer ().m_Team == 2) {
			if (FindObjectOfType<Awareness> ().WhatBelonging)
				text += "You are in <color=#" + ColorUtility.ToHtmlStringRGB (Color.red) + ">Team 2</color>\n\n";
			if (FindObjectOfType<Awareness> ().WhatGroupGoal)
				text += "Your team's goal is to destroy <color=#" + ColorUtility.ToHtmlStringRGB (Color.blue) + ">Team 1</color>";
		}
		GetComponent<Text> ().text = text;
	}
}
