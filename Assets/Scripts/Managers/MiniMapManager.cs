using UnityEngine;
using System.Collections;

public class MiniMapManager : MonoBehaviour {


	public MObject playerArrow;
	public MObject enemyArrow;


	public void UpdateMinimap(){
		if (FindObjectOfType<Awareness> ().WhereLocation) {
			foreach (TankManager tm in GameManager.m_Tanks) {
				if (tm.m_Health.getCurrentHeath () > 0) {
					// Alive
					if (tm.isLocal ()) {
						// Is local
						if (tm.m_Instance.GetComponent<MActor> () == null) {
							//Add player arrow
							MActor ma = tm.m_Instance.AddComponent<MActor> ();
							ma.objPrefab = playerArrow;
						}
					} else {
						// Is not local
						if (tm.m_Instance.GetComponent<MActor> () == null) {
							// Add enemy arrow
							MActor ma = tm.m_Instance.AddComponent<MActor> ();
							ma.objPrefab = enemyArrow;
						}
					}
				} else {
					// Dead
					MActor ma = tm.m_Instance.GetComponent<MActor> ();
					if (ma != null) {
						Destroy (ma);
					}
				}
			}
		}
	}


}
