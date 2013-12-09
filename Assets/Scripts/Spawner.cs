using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour, IEventListener {
	public GameObject EasyEnemy;
	public GameObject MediumEnemy;
	public GameObject HardEnemy;
	public GameObject BossEnemy;
	
	
	public int totalEnemy = 10;
	public int numEnemy = 0;
	public int spawnedEnemy = 0;
	private bool waveSpawn = false;
	public bool inACoroutine = false;
	public float waveTimer = 30.0f;
	private float nextRound = 0.0f;
	public int totalWaves = 5;
	public int numRounds = 0;
	public float spawnMin = 1.0f;
	public float spawnMax = 3.0f;
	public float nextSpawnTime = 0.0f;
	private Score Round;				// Reference to the Score script.
	private Transform player;
	private Vector2 offset;
	public float distance = 10;

	//event listener that starts the coroutine
	public bool OnMonsterDestroyed(IEvent evt){
		spawnedEnemy--;

		return true;
	}

	public bool HandleEvent(IEvent evt) { return false; }

	void Awake()
	{
		Round = GameObject.Find("Score").GetComponent<Score>();
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Start()
	{
		EventManager.instance.AddListener(this, "MonsterDestroyed", OnMonsterDestroyed);
		//StartCoroutine("SpawnEnemies");
	}
	
	void Update ()
	{
		if(spawnedEnemy == 0 && !inACoroutine){
			StartCoroutine("SpawnEnemies");
		}

	}
	
	IEnumerator SpawnEnemies(){
		inACoroutine = true;
		yield return new WaitForSeconds(5f);
		Round.round++;
		totalEnemy += 5;
		spawnMax -= 0.25f;
		for(int i = 0; i < totalEnemy; i++) {
			spawnEnemy();
			nextSpawnTime = Random.Range (spawnMin,spawnMax);
			yield return new WaitForSeconds(nextSpawnTime);
		}
		inACoroutine = false;
	}

	// spawns an enemy 
	private void spawnEnemy()
	{
		float randomizer = Mathf.Sign(Random.Range(-1.0f,1.0f));
		distance = distance * randomizer;
		offset = new Vector2 (player.position.x - distance , transform.position.y);
		if(numEnemy % 7 == 0 && numEnemy != 0)
			Instantiate(MediumEnemy, offset, transform.rotation);
		else
			Instantiate(EasyEnemy, offset, transform.rotation);

		// Increase the total number of enemies spawned and the number of spawned enemies
		numEnemy++;
		spawnedEnemy++;
	}
	

}