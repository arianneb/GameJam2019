using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
	public GameObject object_to_spawn;
	public float initial_delay = 2.0f;
	public float time_between_spawns = 1.0f;


	void Start () {
		InvokeRepeating ("SpawnObject", initial_delay, time_between_spawns);
	}


	public void SpawnObject()
	{
		Instantiate (object_to_spawn, this.transform.position, Quaternion.identity);
	}
}
