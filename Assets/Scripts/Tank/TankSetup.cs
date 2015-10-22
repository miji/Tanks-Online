﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Purpose of that class is syncing data between server - client
public class TankSetup : NetworkBehaviour
{
	[Header("UI")]
	public Text
		m_NameText;
	public GameObject m_Crown;
	[Header("Network")]
	[Space]
	[SyncVar]
	public Color
		m_Color;
	[SyncVar]
	public string
		m_PlayerName;

	//this is the player number in all of the players
	[SyncVar]
	public int
		m_PlayerNumber;

	//This is the local ID when more than 1 player per client
	[SyncVar]
	public int
		m_LocalID;
	[SyncVar]
	public bool
		m_IsReady = false;

	//This allow to know if the crown must be displayed or not
	protected bool m_isLeader = false;

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		if (!isServer) //if not hosting, we had the tank to the gamemanger for easy access!
			GameManager.AddTank (gameObject, m_PlayerNumber, m_Color, m_PlayerName, m_LocalID);

		GameObject m_TankRenderers = transform.Find ("TankRenderers").gameObject;

		// Get all of the renderers of the tank.
		Renderer[] renderers = m_TankRenderers.GetComponentsInChildren<Renderer> ();

		// Go through all the renderers...
		for (int i = 0; i < renderers.Length; i++) {
			// ... set their material color to the color specific to this tank.

			// Present-Who-Identity
			if (GameObject.FindObjectOfType<Awareness> ().WhoIdentity)
				renderers [i].material.color = m_Color;
			else
				renderers [i].material.color = Color.green;
		}

		if (m_TankRenderers)
			m_TankRenderers.SetActive (false);

		// Present-Who-Identity
		if (GameObject.FindObjectOfType<Awareness> ().WhoIdentity) 
			m_NameText.text = "<color=#" + ColorUtility.ToHtmlStringRGB (m_Color) + ">" + m_PlayerName + "</color>";
		else
			m_NameText.text = "";

		m_Crown.SetActive (false);


	}

	[ClientCallback]
	public void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		if (GameManager.s_Instance.m_GameIsFinished && !m_IsReady) {
			if (Input.GetButtonUp ("Fire" + (m_LocalID + 1))) {
				CmdSetReady ();
			}
		}
	}

	public void SetLeader (bool leader)
	{
		RpcSetLeader (leader);
	}

	[ClientRpc]
	public void RpcSetLeader (bool leader)
	{
		m_isLeader = leader;
	}

	[Command]
	public void CmdSetReady ()
	{
		m_IsReady = true;
	}

	public void ActivateCrown (bool active)
	{//if we try to show (not hide) the crown, we only show it we are the current leader
		m_Crown.SetActive (active ? m_isLeader : false);
		m_NameText.gameObject.SetActive (active);
	}

	public override void OnNetworkDestroy ()
	{
		GameManager.s_Instance.RemoveTank (gameObject);
	}
}
