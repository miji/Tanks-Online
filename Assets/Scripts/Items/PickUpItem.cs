using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PickUpItem : NetworkBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Tank") {
			// Is a tank
			TankHealth th = other.gameObject.GetComponent<TankHealth> ();
			if (th.isLocal ()) {
				// Is local player
				TankItem ti = other.gameObject.GetComponent<TankItem> ();
				if (gameObject.tag == "Heart") {
					// Is a heart
					ti.ActivateHeart ();
				} else if (gameObject.tag == "Red") {
					ti.ActivateRed ();
				} else if (gameObject.tag == "Blue") {
					ti.ActivateBlue ();
				} else if (gameObject.tag == "Yellow") {
					ti.ActivateYellow ();
				} else if (gameObject.tag == "Green") {
					ti.ActivateGreen ();
				}
				ti.CmdDestroyItem(gameObject);

			}
		}

	}






		
	
}
