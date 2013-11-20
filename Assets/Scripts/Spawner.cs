using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;	         // The amount of time between each spawn.
	public float spawnDelay = 3f;	        // The amount of time before spawning starts.
	public GameObject enemie;		       // Array of enemy prefabs.


	void Start ()
	{
		// Start calling the Spawn function repeatedly after a delay .
		InvokeRepeating("Spawn", spawnDelay, spawnTime);

	}


	void Spawn ()
	{

		Instantiate(enemie, transform.position, transform.rotation);


	}


}