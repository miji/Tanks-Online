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
	public bool WherePosition;
	public bool WhereGaze; // requires WhereLocation

	[Header("Past")]
	public bool WhatTaskHistory; //requires WhatTask

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
