using UnityEngine;
using System.Collections;

public class Awareness : MonoBehaviour {

	[Header("Present")]
	public bool WhoIdentity;
	public bool WhoAuthorship; //requires WhatTask
	public bool WhatTask;
	public bool WhatStatus;
	public bool WhatAbilities;
	public bool WhereLocation;
	public bool WhereGaze; // requires WhereLocation
	public bool WhereReach;
	public bool WherePosition;
	public bool HowDevice;

	[Header("Past")]
	public bool WhatTaskHistory; //requires WhatTask
	public bool WhenEventHistory; //requires WhatTask

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void ApplyAwarenessElements(){

		// Disable minimap
		if(!WhereLocation && !WherePosition) GameObject.Find ("Minimap").gameObject.SetActive (false);

		if(!HowDevice) GameObject.Find ("Keys").gameObject.SetActive (false);
	}
}
