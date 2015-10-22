using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	System.Timers.Timer timer;
	int secondsToEnd;
	Coroutine coroutine;
	Text text;

	private void Start(){
		text = GetComponent<Text> ();
	}

	public void StartCountDown(int seconds){
		secondsToEnd = seconds;
		coroutine = StartCoroutine (_StartCountDown ());
	}

	IEnumerator _StartCountDown(){
		while (true) {
			for (int i=secondsToEnd; i>0; i--) {
				text.text = "Next item in " + i + " seconds";
				yield return new WaitForSeconds (1);
			}
		}
	}

	public void StopCountDown(){
		StopCoroutine (coroutine);
		text.text = "";
	}

}
