using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{


	public MObject playerArrow;
	public MObject team1Arrow;
	public MObject team2Arrow;
	public MObject playerCircle;
	public MObject team1Circle;
	public MObject team2Circle;

	void FixedUpdate(){
		UpdateMinimap ();
	}

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
							//if (FindObjectOfType<Awareness> ().WhoIdentity)
								//ma.objPrefab.GetComponent<Image> ().color = tm.m_Setup.m_Color;
						}
					} else {
						// Is not local
						if (tm.m_Instance.GetComponent<MActor> () == null) {
							// Add enemy arrow
							MActor ma = tm.m_Instance.AddComponent<MActor> ();
							if(tm.m_Team==1){
								if (FindObjectOfType<Awareness> ().WhereGaze){
									ma.objPrefab = team1Arrow;
								}
								else{
									ma.objPrefab = team1Circle;
								}
							} else if(tm.m_Team==2){
								if (FindObjectOfType<Awareness> ().WhereGaze){
									ma.objPrefab = team2Arrow;
								}
								else{
									ma.objPrefab = team2Circle;
								}
							}





							//color
							//if (FindObjectOfType<Awareness> ().WhoIdentity)
								//ma.objPrefab.GetComponent<Image> ().color = tm.m_Setup.m_Color;
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
