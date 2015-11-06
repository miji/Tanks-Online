using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TankItem : NetworkBehaviour {

	public int ItemDuration=30;

	TankHealth healt;
	TankMovement movement;
	TankShooting shooting;
	Transform activeItems;
	TankSetup setup;
	TankMessage messages;
	[SyncVar]
	public bool red=false,yellow=false, blue=false, green=false;

	Coroutine redCoroutine, blueCoroutine, yellowCoroutine, greenCoroutine;

	float initialSpeed;

	private void Awake(){
		healt = gameObject.GetComponent<TankHealth> ();
		movement = gameObject.GetComponent<TankMovement> ();
		shooting = gameObject.GetComponent<TankShooting> ();
		setup = gameObject.GetComponent<TankSetup> ();
		messages = gameObject.GetComponent<TankMessage> ();
		initialSpeed = movement.m_Speed;
		activeItems = transform.FindChild ("ActiveItems");
	}

	public void ActivateHeart(){
		healt.ResetHealth ();

		if(FindObjectOfType<Awareness> ().WhoAuthorship)
			messages.CmdSendMessage (setup.m_PlayerName + " got a heart");
		else
			messages.CmdSendMessage ("Somebody got a heart");
	}

	public void ActivateRed(){
		
		redCoroutine=StartCoroutine(StartRed());
		if(FindObjectOfType<Awareness> ().WhoAuthorship)
			messages.CmdSendMessage (setup.m_PlayerName + " got a red item");
		else
			messages.CmdSendMessage ("Somebody got a red item");
	}

	public void ActivateBlue(){

		blueCoroutine=StartCoroutine(StartBlue());
		if(FindObjectOfType<Awareness> ().WhoAuthorship)
			messages.CmdSendMessage (setup.m_PlayerName + " got a blue item");
		else
			messages.CmdSendMessage ("Somebody got a blue item");
	}

	public void ActivateYellow(){
		
		yellowCoroutine=StartCoroutine(StartYellow());
		if(FindObjectOfType<Awareness> ().WhoAuthorship)
			messages.CmdSendMessage (setup.m_PlayerName + " got a yellow item");
		else
			messages.CmdSendMessage ("Somebody got a yellow item");
	}

	public void ActivateGreen(){
		
		greenCoroutine=StartCoroutine(StartGreen());
		if(FindObjectOfType<Awareness> ().WhoAuthorship)
			messages.CmdSendMessage (setup.m_PlayerName + " got a green item");
		else
			messages.CmdSendMessage ("Somebody got a green item");
	}
	
	IEnumerator StartRed ()
	{

		if (red) {
			StopCoroutine(redCoroutine);
			StopRed();
		}
		CmdSetRed (true);
		movement.m_Speed *= 1.5f;
		yield return new WaitForSeconds (ItemDuration);
		messages.CmdSendMessage (setup.m_PlayerName + " finished red item");
		StopRed ();
	}

	void StopRed ()
	{
		movement.m_Speed = initialSpeed;
		CmdSetRed (false);

	}

	IEnumerator StartBlue ()
	{

		if (blue) {
			StopCoroutine(blueCoroutine);
			StopBlue();
		}
		CmdSetBlue (true);
		shooting.CmdSetStrong (true);
		yield return new WaitForSeconds (ItemDuration);
		messages.CmdSendMessage (setup.m_PlayerName + " finished blue item");
		StopBlue ();
	}

	void StopBlue ()
	{
		shooting.CmdSetStrong (false);
		CmdSetBlue (false);

	}

	IEnumerator StartYellow ()
	{

		if (yellow) {
			StopCoroutine(yellowCoroutine);
			StopYellow();
		}
		CmdSetYellow (true);
		shooting.CmdSetLong (true);
		yield return new WaitForSeconds (ItemDuration);
		messages.CmdSendMessage (setup.m_PlayerName + " finished yellow item");
		StopYellow ();
	}

	void StopYellow ()
	{
		shooting.CmdSetLong (false);
		CmdSetYellow (false);

	}

	IEnumerator StartGreen ()
	{

		if (green) {
			StopCoroutine(greenCoroutine);
			StopGreen();
		}
		CmdSetGreen (true);
		healt.CmdSetImmortal (true);
		yield return new WaitForSeconds (ItemDuration);
		messages.CmdSendMessage (setup.m_PlayerName + " finished green item");
		StopGreen ();
	}

	void StopGreen ()
	{
		healt.CmdSetImmortal (false);
		CmdSetGreen (false);

	}

	public void DisableItems(){
		if (redCoroutine != null)  StopCoroutine (redCoroutine);
		if (blueCoroutine != null)StopCoroutine (blueCoroutine);
		if (yellowCoroutine != null)StopCoroutine (yellowCoroutine);
		if (greenCoroutine != null)StopCoroutine (greenCoroutine);
		StopRed ();
		StopBlue ();
		StopYellow ();
		StopGreen ();

	}

	[Command]
	void CmdSetRed(bool status){
		red = status;
	}

	[Command]
	void CmdSetBlue(bool status){
		blue = status;
	}

	[Command]
	void CmdSetYellow(bool status){
		yellow = status;
	}

	[Command]
	void CmdSetGreen(bool status){
		green = status;
	}

	private void Update(){
		if (FindObjectOfType<Awareness> ().WhatAbilities) {
			activeItems.transform.FindChild ("RedItem").gameObject.SetActive (red);
			activeItems.transform.FindChild ("BlueItem").gameObject.SetActive (blue);
			activeItems.transform.FindChild ("YellowItem").gameObject.SetActive (yellow);
			activeItems.transform.FindChild ("GreenItem").gameObject.SetActive (green);
		}
	}

	[Command]
	public void CmdDestroyItem(GameObject item)
	{
		NetworkServer.Destroy(item);
	}



}
