﻿using UnityEngine;
using System.Collections;

public class MonsterDestroyed : BaseEvent { }

public class Enemy : MonoBehaviour
{
	public float moveSpeed = 4f;		// The speed the enemy moves at.
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public Sprite deadEnemy;			// A sprite of the enemy when it's dead.
	public Sprite damagedEnemy;			// An optional sprite of the enemy when it's damaged
	public float xDistance = 2f;
	public float yDistance = 1f;
	public bool facingRight = true;	
	public GameObject ARPickup;
	public GameObject shotgunPickup;
	public GameObject pistolPickup;
	private SpriteRenderer ren;			// Reference to the sprite renderer.
	//private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private bool dead = false;			// Whether or not the enemy is dead.
	private Score score;				// Reference to the Score script.
	private Transform player;			// Reference to the player's transform.
	private float temp;
	private Transform check;
	private RaycastHit2D hit;
	private bool clear = true;
	public GameObject jumpingBug; 
	public GameObject monster;


	
	
	void Awake()
	{
		// Setting up the references.
		ren = transform.Find("body").GetComponent<SpriteRenderer>();
		//frontCheck = transform.Find("frontCheck").transform;
		score = GameObject.Find("Score").GetComponent<Score>();

		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag("Player").transform;






	}

	
	bool CheckXDistance()
	{
		// Returns true if the distance between the enemy and the player in the x axis is greater than the x distance.

		return Mathf.Abs(transform.position.x - player.position.x) > xDistance;
	}

	bool CheckXDistanceJumper()
	{
		// Returns true if the distance between the enemy and the player in the x axis is greater than the x distance.
		
		return Mathf.Abs(transform.position.x - player.position.x) > 5f;
	}
	
	
	float CheckYDistance()
	{
		// Returns true if the distance between the enemy and the player in the y axis is greater than the y distance.
		return Mathf.Abs(transform.position.y - player.position.y);
	}

	float CheckDirection()
	{
		return Mathf.Sign(transform.position.x - player.position.x);

	}
/*
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Obstacle" )// && rigidbody2D.velocity.y == 0f && clear 
		{
			//rigidbody2D.AddForce(new Vector2(0f, 500f));
			//clear = true;

			moveSpeed = 0;
		}
	
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Obstacle" )// && rigidbody2D.velocity.y == 0f && clear 
		{
			moveSpeed = 4f;
			TrackPlayer();
			//rigidbody2D.AddForce(new Vector2(0f, 500f));
			//clear = true;
		}
		
	}
*/
	void FixedUpdate ()
	{

		if(!CheckXDistanceJumper() && jumpingBug.rigidbody2D.velocity.y == 0f)
			jumpingBug.rigidbody2D.AddForce(new Vector2(100f, CheckYDistance()*200f));
		
		/*	if(CheckYDistance() < 2f)
			{	
				jumpingBug.rigidbody2D.AddForce(new Vector2(100f, 200f));
			}
			else if(CheckYDistance() < 4f)
			{
				jumpingBug.rigidbody2D.AddForce(new Vector2(100f, 400f));
			}
			*/
		//if(rigidbody2D.velocity.y > 12f)
			//rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, transform.localScale.y * 0);

		TrackPlayer();

	
		// If the enemy has one hit point left and has a damagedEnemy sprite...
		if(HP == 1 && damagedEnemy != null)
			// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
			ren.sprite = damagedEnemy;
			
		// If the enemy has zero or fewer hit points and isn't dead yet...
		if(HP <= 0 && !dead)
			// ... call the death function.
			Death ();
	}

	void TrackPlayer ()
	{	
		// checks to see if the player is far away from the enemy if so makes the enemy close the gap
		if(CheckXDistance() )
		{

			if(CheckDirection() > 0 && !facingRight  )
			{

				Flip();
				moveSpeed = -moveSpeed;
			}
			else if(CheckDirection() < 0 && facingRight )
			{
				Flip();
				moveSpeed = -moveSpeed;
			}
		
			// Set the enemy's velocity to moveSpeed in the x direction.
			monster.rigidbody2D.velocity = new Vector2(transform.localScale.x *( moveSpeed * CheckDirection()), rigidbody2D.velocity.y);
			if(jumpingBug.rigidbody2D.velocity.y == 0)
				jumpingBug.rigidbody2D.velocity = new Vector2(transform.localScale.x * ( moveSpeed * CheckDirection()), rigidbody2D.velocity.y);
		}

	}
	
	public void Hurt()
	{
		// Reduce the number of hit points by one.
		HP--;
		rigidbody2D.AddForce( new Vector2(1000f*CheckDirection(),0f));
	}
	
	void Death()
	{
		// Find all of the sprite renderers on this object and it's children.
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		// Disable all of them sprite renderers.
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}

		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
		ren.enabled = true;
		ren.sprite = deadEnemy;

		// Increase the score by 100 points
		score.score += 100;

		// Set dead to true.
		dead = true;

	

		// Find all of the colliders on the gameobject and set them all to be triggers.
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}

	

		// Create a vector that is just above the enemy.
		Vector3 scorePos;
		scorePos = transform.position;
		scorePos.y += 1.5f;

		//Possibly drop new weapon
		if(Random.Range (0, 8) == 1){
			int rando = Random.Range (0, 4);
			if(rando == 0){
				GameObject AR = (GameObject)Instantiate (ARPickup, this.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
				AR.GetComponent<WeaponPickup>().setWTPickup(0);
			}
			else if(rando == 1){
				GameObject shotgun = (GameObject)Instantiate (shotgunPickup, this.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
				shotgun.GetComponent<WeaponPickup>().setWTPickup(2);
			}
			else {
				GameObject pistol = (GameObject)Instantiate (pistolPickup, this.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
				pistol.GetComponent<WeaponPickup>().setWTPickup(1);
			}
		}

		Destroy (this.gameObject);

		EventManager.instance.QueueEvent(new MonsterDestroyed());
	}


	public void Flip()
	{
		facingRight = !facingRight;
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
	
}
