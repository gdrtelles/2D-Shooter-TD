using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour, IEventListener {
	public GameObject EasyEnemy;
	public GameObject MediumEnemy;
	public GameObject HardEnemy;
	public GameObject BossEnemy;
	
	
	public int totalEnemy = 11;
	public int numEnemy = 0;
	public int spawnedEnemy = 0;
	private bool waveSpawn = false;
	public bool Spawn = true;
	public float waveTimer = 30.0f;
	private float nextRound = 0.0f;
	public int totalWaves = 5;
	public int numRounds = 0;
	public float spawnMin = 1.0f;
	public float spawnMax = 3.0f;
	public float nextSpawnTime = 0.0f;

	//event listener that starts the coroutine
	public bool OnMonsterDestroyed(IEvent evt){
		spawnedEnemy--;
		if(spawnedEnemy == 0){
			numRounds++;
			StartCoroutine("SpawnEnemies");
		}
		return true;
	}

	public bool HandleEvent(IEvent evt) { return false; }

	void Start()
	{
		EventManager.instance.AddListener(this, "MonsterDestroyed", OnMonsterDestroyed);
		StartCoroutine("SpawnEnemies");
	}
	
	void Update ()
	{

	}
	
	IEnumerator SpawnEnemies(){
		yield return new WaitForSeconds(10f);
		for(int i = 0; i < totalEnemy; i++) {
			spawnEnemy();
			nextSpawnTime = Random.Range (spawnMin,spawnMax);
			yield return new WaitForSeconds(nextSpawnTime);
		}
	}

	// spawns an enemy 
	private void spawnEnemy()
	{
		Instantiate(EasyEnemy, transform.position, transform.rotation);

		// Increase the total number of enemies spawned and the number of spawned enemies
		numEnemy++;
		spawnedEnemy++;
	}
	
	// returns the current round
	public float currentRound
	{
		get
		{
			return numRounds;
		}
	}
}