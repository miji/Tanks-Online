using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ItemManager : NetworkBehaviour {

	public Transform[] spawnPoints;
	public GameObject[] itemPrefabs;
	int timeBetwenItems;
	public bool spawning=false;
	GameObject clone;
	Coroutine coroutine;

	IEnumerator SpawnItems(){
		while (spawning) {
			yield return new WaitForSeconds (timeBetwenItems);

			int point = Random.Range (0, spawnPoints.Length);
			int item = Random.Range (0, itemPrefabs.Length);
			clone = (GameObject)Instantiate (itemPrefabs [item], spawnPoints [point].position, spawnPoints [point].rotation);
			if(!FindObjectOfType<Awareness>().WherePosition){
				Destroy(clone.GetComponent<MActor>());
			}
			Destroy (clone, timeBetwenItems);
			NetworkServer.Spawn (clone);
		}
	}

	[Command]
	public void CmdDestroyItem(GameObject item)
	{
		NetworkServer.Destroy(item);
	}

	[Server]
	public void Enable(int timeBetweenItems){

		// debug 

		//clone = (GameObject)Instantiate (itemPrefabs [2], spawnPoints [0].position, spawnPoints [0].rotation);
		//NetworkServer.Spawn (clone);

		// end debug
		this.timeBetwenItems = timeBetweenItems;
		spawning = true;
		coroutine=StartCoroutine (SpawnItems ());
	}




	[Server]
	public void Disable(){
		spawning = false;
		StopCoroutine (coroutine);

		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Heart")) {
			CmdDestroyItem(item);
		}
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Red")) {
			CmdDestroyItem(item);
		}
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Blue")) {
			CmdDestroyItem(item);
		}
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Yellow")) {
			CmdDestroyItem(item);
		}
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Green")) {
			CmdDestroyItem(item);
		}

	}





}
