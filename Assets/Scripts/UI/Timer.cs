using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Timer : NetworkBehaviour {

	System.Timers.Timer timer;
	int secondsToEnd;
	Coroutine coroutine;
	public GameObject nextItemText;



	[ClientRpc]
	public void RpcStartCountDown(int seconds){
		secondsToEnd = seconds;
		coroutine = StartCoroutine (_StartCountDown ());
	}


	public void StartCountDown(int seconds){
		secondsToEnd = seconds;
		coroutine = StartCoroutine (_StartCountDown ());
	}

	IEnumerator _StartCountDown(){
		while (true) {
			for (int i=secondsToEnd; i>0; i--) {
				nextItemText.GetComponent<Text>().text = "Next item in " + i + " seconds";
				yield return new WaitForSeconds (1);
			}
		}
	}

	public void StopCountDown(){
		StopCoroutine (coroutine);
		nextItemText.GetComponent<Text>().text = "";
	}

	[ClientRpc]
	public void RpcStopCountDown(){
		StopCoroutine (coroutine);
		nextItemText.GetComponent<Text>().text = "";
	}

}
