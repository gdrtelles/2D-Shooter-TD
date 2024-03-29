﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.


	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
			// Delay for when the taunt should happen.


	//private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool onPlatform = false;
	private Animator anim;					// Reference to the player's animator component.
	private Vector2 mousePos;				// Mouse position for rotating gun.
	private Camera cam;
	public GameObject gun;
	public GameObject shoulder;
	private Score lives;
	KongregateAPI kongAPI;
	
	
	void Start(){

		
		GameObject kongregateAPIObject = GameObject.Find("KongregateAPI");
		if(kongregateAPIObject != null)
			kongAPI = kongregateAPIObject.GetComponent<KongregateAPI>();
	}



	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
		cam = GameObject.Find ("Main Camera").camera;
		lives = GameObject.Find("Score").GetComponent<Score>();

	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")); 
		onPlatform = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform")); 

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}
		else if(Input.GetButtonDown("Jump") && onPlatform)
		{
			jump = true;
		}

		//Gun rotation.
		mousePos = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		if(transform.position.x < mousePos.x && !facingRight)
		{
			Flip();
		}
		else if ( transform.position.x > mousePos.x && facingRight)
		{
			Flip();
		}
		float rotation = Mathf.Atan2((mousePos.y - gun.transform.position.y), (mousePos.x - gun.transform.position.x)) * Mathf.Rad2Deg;
//		Debug.Log (rotation);
		if(facingRight){ 
			if((rotation <= 90 && rotation > 0) || (rotation >= -90 && rotation < 0)){
				gun.transform.eulerAngles = new Vector3(0, 0, rotation);
				shoulder.transform.eulerAngles = new Vector3(0, 0, rotation*0.25f);
			}
		}
		else if (!facingRight){
			if ((rotation >= 90 && rotation < 180) || (rotation < -90) && rotation > - 179){
				gun.transform.eulerAngles = new Vector3(0, 0, -(rotation - 180));
				shoulder.transform.eulerAngles = new Vector3(0, 0, -(rotation - 180)*0.5f);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		int HighScore = lives.score;
		if(kongAPI.isKongregate)
			kongAPI.SubmitStats("HighScore", HighScore);
	
		if(col.gameObject.tag == "Enemy")
			lives.lives--;
			//Application.LoadLevel("gameOver");
		if(lives.lives <= 0)
		{
			// Report Game Statistics to Kongregate
			//){
				

				
			//}
			Application.LoadLevel("gameOver");
			
		}


	}

	void FixedUpdate ()
	{
	
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

	
		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Play a random jump audio clip.
			//int i = Random.Range(0, jumpClips.Length);
			//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}



}
