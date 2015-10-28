using UnityEngine;
using System.Collections;

public class Awareness : MonoBehaviour {

	[Header("Present")]
	public bool WhoIdentity;
	public bool WhoAuthorship; //requires WhatTask
	public bool WhatTask;
	public bool WhatStatus; // health + crown + score
	public bool WhatAbilities;
	public bool WhereLocation;
	public bool WhereGaze; // requires WhereLocation
	public bool WhereReach;
	public bool WherePosition;
	public bool HowDevice;

	[Header("Past")]
	public bool WhatTaskHistory; //requires WhatTask
	public bool WhenEventHistory; //requires WhatTask

	[Header("Future")]
	public bool WhatNextEvent;
	public bool WhatNextAbilities;

	[Header("Social & Group Dynamics")]
	public bool WhoMembers;
	public bool WhoOtherMembers;
	public bool WhatBelonging;
	public bool WhatGroupGoal;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void ApplyAwarenessElements(){

		// Disable minimap
		if(!WhereLocation && !WherePosition) GameObject.Find ("Minimap").gameObject.SetActive (false);

		//if(!WhatStatus) GameObject.Find ("ScoreText").gameObject.SetActive (false);

		if(!HowDevice) GameObject.Find ("Keys").gameObject.SetActive (false);

		if(!WhatNextEvent) GameObject.Find ("NextItemText").gameObject.SetActive (false);

		if(!WhatNextAbilities) GameObject.Find ("Items").gameObject.SetActive (false);
	}
}
