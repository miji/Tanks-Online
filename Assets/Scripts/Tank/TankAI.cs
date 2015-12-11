using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TankAI : NetworkBehaviour {


	NavMeshAgent nav;
	TankSetup setup;
	TankMovement movement;
	TankShooting shooting;

	public float fireRate;
	
	private float nextFire;


	[Server]
	void Awake(){
		nav = GetComponent<NavMeshAgent> ();
		setup = GetComponent<TankSetup> ();
		movement = GetComponent<TankMovement> ();
		shooting = GetComponent<TankShooting> ();
		string diff = FindObjectOfType<Awareness> ().difficulty;
		if(diff=="Easy" || diff=="easy" || diff=="E" || diff=="e") fireRate=2;
		else if (diff=="Normal" || diff=="normal" || diff=="n" || diff=="n") fireRate=1;
		else if (diff=="Hard" || diff=="hard" || diff=="H" || diff=="h") fireRate=0.5f;

	}
	// Use this for initialization
	[Server]
	void Start () {



	}




	// Update is called once per frame
	[Server]
	void Update () {
		nav.speed = movement.m_Speed;
		nav.angularSpeed = movement.m_TurnSpeed;
		//nav.SetDestination (destination.transform.position);

		int goal=0;

		if (setup.m_Team == 1)
			goal = 2;
		else if (setup.m_Team == 2)
			goal = 1;

		Vector3 destination = Vector3.zero;

		foreach (TankManager tank in GameManager.m_Tanks) {
			if(tank.m_Team==goal && tank.m_Health.getCurrentHeath()>0) {
				destination=tank.m_Instance.transform.position;
				break;
			}
		}
		nav.SetDestination (destination);

		if (Vector3.Distance (transform.position, destination) < 10 && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			shooting.Fire();

		} 

	}


	
}
