using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TankHealth : NetworkBehaviour
{
	public float m_StartingHealth = 100f;             // The amount of health each tank starts with.
	public Slider m_Slider;                           // The slider to represent how much health the tank currently has.
	public Image m_FillImage;                         // The image component of the slider.
	public Color m_FullHealthColor = Color.green;     // The color the health bar will be when on full health.
	public Color m_ZeroHealthColor = Color.red;       // The color the health bar will be when on no health.
	public AudioClip m_TankExplosion;                 // The clip to play when the tank explodes.
	public ParticleSystem m_ExplosionParticles;       // The particle system the will play when the tank is destroyed.
	public GameObject m_TankRenderers;                // References to all the gameobjects that need to be disabled when the tank is dead.
	public GameObject m_HealthCanvas;
	public GameObject m_AimCanvas;
	public GameObject m_LeftDustTrail;
	public GameObject m_RightDustTrail;
	public TankSetup m_Setup;
	public TankManager m_Manager;                   //Associated manager, to disable control when dying.
	public TankMessage messages;
	[SyncVar(hook = "OnCurrentHealthChanged")]
	private float
		m_CurrentHealth;                  // How much health the tank currently has.*
	[SyncVar]
	private bool
		m_ZeroHealthHappened;              // Has the tank been reduced beyond zero health yet?
	private BoxCollider m_Collider;                 // Used so that the tank doesn't collide with anything when it's dead.

	[SyncVar]
	public bool
		immortal = false;

	[Command]
	public void CmdSetImmortal (bool status)
	{
		immortal = status;
	}
	 
	private void Awake ()
	{
		m_Collider = GetComponent<BoxCollider> ();
		messages = GetComponent<TankMessage> ();
	}


	// This is called whenever the tank takes damage.
	public void Damage (float amount, string attacker, int team)
	{
		// if green
		if (immortal)
			return;

		// Reduce current health by the amount of damage done.

		// Avoid suicide - My code
		//if (!attacker.Equals (m_Setup.m_PlayerName))
		if (team!=m_Setup.m_Team)
			m_CurrentHealth -= amount;

		// If the current health is at or below zero and it has not yet been registered, call OnZeroHealth.
		if (m_CurrentHealth <= 0f && !m_ZeroHealthHappened) {
			OnZeroHealth (attacker);
		}
	}

	private void SetHealthUI ()
	{
		// Set the slider's value appropriately.
		//if (FindObjectOfType<Awareness> ().WhatStatus)
		m_Slider.value = m_CurrentHealth;
		//else
		//	m_Slider.gameObject.SetActive (false);

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
	}

	void OnCurrentHealthChanged (float value)
	{
		m_CurrentHealth = value;
		// Change the UI elements appropriately.
		SetHealthUI ();

	}

	private void OnZeroHealth (string attacker)
	{
		// Set the flag so that this function is only called once.
		m_ZeroHealthHappened = true;

		string message;
		if (FindObjectOfType<Awareness> ().WhoAuthorship) {
			if (attacker.Equals (m_Setup.m_PlayerName))
				message = attacker + " destroyed itself";
			else
				message = attacker + " destroyed " + m_Setup.m_PlayerName;
		} else {
			message = m_Setup.m_PlayerName + " was destroyed";
		}

		messages.CmdSendMessage (message);



		RpcOnZeroHealth ();
	}

	private void InternalOnZeroHealth ()
	{
		// Disable the collider and all the appropriate child gameobjects so the tank doesn't interact or show up when it's dead.
		SetTankActive (false);
	}

	[ClientRpc]
	private void RpcOnZeroHealth ()
	{
		// Play the particle system of the tank exploding.
		m_ExplosionParticles.Play ();

		// Create a gameobject that will play the tank explosion sound effect and then destroy itself.
		AudioSource.PlayClipAtPoint (m_TankExplosion, transform.position);

		/*
		try{
			GameObject.Find ("Minimap").GetComponent<MiniMapManager> ().UpdateMinimap ();
		}
		catch{}*/

		InternalOnZeroHealth ();
	}

	private void SetTankActive (bool active)
	{
		try{
			GameObject.Find ("Minimap").GetComponent<MiniMapManager> ().UpdateMinimap ();
		}
		catch{}

		m_Collider.enabled = active;

		m_TankRenderers.SetActive (active);
		if (FindObjectOfType<Awareness> ().WhatStatus)
			m_HealthCanvas.SetActive (active);
		else
			m_HealthCanvas.SetActive (false);

		// Where - Reach
		if (FindObjectOfType<Awareness> ().WhereReach)
			m_AimCanvas.SetActive (active);
		else
			m_AimCanvas.SetActive (false);
		m_LeftDustTrail.SetActive (active);
		m_RightDustTrail.SetActive (active);

		if (active)
			m_Manager.EnableControl ();
		else
			m_Manager.DisableControl ();


		if(!isLocal())
			m_Setup.ActivateCrown (active);

		// Camera 
		
		if(isLocal()) transform.FindChild ("Camera").gameObject.SetActive (active);
		if(isLocal()) transform.FindChild ("NameCanvas").gameObject.SetActive (!active);


	}

	// This function is called at the start of each round to make sure each tank is set up correctly.
	public void SetDefaults ()
	{
		m_CurrentHealth = m_StartingHealth;
		m_ZeroHealthHappened = false;
		SetTankActive (true);
	}

	public float getCurrentHeath ()
	{
		return m_CurrentHealth;
	}

	public void ResetHealth ()
	{
		m_CurrentHealth = m_StartingHealth;
	}

	public bool isLocal ()
	{
		if (isLocalPlayer) {
			return true;
		} else {
			return false;
		}
	}
}
