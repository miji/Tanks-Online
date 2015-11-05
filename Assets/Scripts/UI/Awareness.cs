﻿using UnityEngine;
using System.Collections;
using System.Xml;

public class Awareness : MonoBehaviour {

	public string file;

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
		Load (file);
	}

	public void ApplyAwarenessElements(){

		// Disable minimap
		if(!WhereLocation && !WherePosition) GameObject.Find ("Minimap").gameObject.SetActive (false);

		//if(!WhatStatus) GameObject.Find ("ScoreText").gameObject.SetActive (false);

		if(!HowDevice) GameObject.Find ("Keys").gameObject.SetActive (false);

		if(!WhatNextEvent) GameObject.Find ("NextItemText").gameObject.SetActive (false);

		if(!WhatNextAbilities) GameObject.Find ("Items").gameObject.SetActive (false);
	}

	void Load (string file)
	{
		Debug.Log (file);
		using (XmlReader reader = XmlReader.Create(file)) {
			while (reader.Read()) {
				if (reader.IsStartElement ()) {
					while (reader.Read()) {
						if (reader.IsStartElement ()) {
							switch (reader.Name) {
							case "Present":
								WhoIdentity = String2Bool (reader ["WhoIdentity"]);
								WhoAuthorship = String2Bool (reader ["WhoAuthorship"]);
								WhatTask = String2Bool (reader ["WhatTask"]);
								WhatStatus = String2Bool (reader ["WhatStatus"]);
								WhatAbilities = String2Bool (reader ["WhatAbilities"]);
								WhereLocation = String2Bool (reader ["WhereLocation"]);
								WhereGaze = String2Bool (reader ["WhereGaze"]);
								WhereReach = String2Bool (reader ["WhereReach"]);
								WherePosition = String2Bool (reader ["WherePosition"]);
								HowDevice = String2Bool (reader ["HowDevice"]);
								break;
							case "Past":
								WhatTaskHistory = String2Bool (reader ["WhatTaskHistory"]);
								WhenEventHistory = String2Bool (reader ["WhenEventHistory"]);
								break;
							case "Future":
								WhatNextEvent = String2Bool (reader ["WhatNextEvent"]);
								WhatNextAbilities = String2Bool (reader ["WhatNextAbilities"]);
								break;
							case "SocialGroupDynamics":
								WhoMembers = String2Bool (reader ["WhoMembers"]);
								WhoOtherMembers = String2Bool (reader ["WhoOtherMembers"]);
								WhatBelonging = String2Bool (reader ["WhatBelonging"]);
								WhatGroupGoal = String2Bool (reader ["WhatGroupGoal"]);
								break;

							
							default:
								Debug.LogError("Awareness configuration error");
								break;
							}
						}
						
					}
				}
			}
		}

	}

	private bool String2Bool(string s)
	{
		if (s == "True" || s == "true") return true;
		else if (s == "False" || s == "false") return false;
		else return false;
		
	}
}
