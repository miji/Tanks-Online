using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{


	public MObject playerArrow;
	public MObject enemyArrow;
	public MObject playerCircle;
	public MObject enemyCircle;

	public void UpdateMinimap ()
	{
		if (FindObjectOfType<Awareness> ().WhereLocation) {
			foreach (TankManager tm in GameManager.m_Tanks) {
				if (tm.m_Health.getCurrentHeath () > 0) {
					// Alive
					if (tm.isLocal ()) {
						// Is local
						if (tm.m_Instance.GetComponent<MActor> () == null) {
							//Add player arrow
							MActor ma = tm.m_Instance.AddComponent<MActor> ();
							if (FindObjectOfType<Awareness> ().WhereGaze)
								ma.objPrefab = playerArrow;
							else
								ma.objPrefab = playerCircle;

							//color
							if (FindObjectOfType<Awareness> ().WhoIdentity)
								ma.objPrefab.GetComponent<Image> ().color = tm.m_Setup.m_Color;

						}
					} else {
						// Is not local
						if (tm.m_Instance.GetComponent<MActor> () == null) {
							// Add enemy arrow
							MActor ma = tm.m_Instance.AddComponent<MActor> ();
							if (FindObjectOfType<Awareness> ().WhereGaze)
								ma.objPrefab = enemyArrow;
							else
								ma.objPrefab = enemyCircle;

							//color
							if (FindObjectOfType<Awareness> ().WhoIdentity)
								ma.objPrefab.GetComponent<Image> ().color = tm.m_Setup.m_Color;
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
