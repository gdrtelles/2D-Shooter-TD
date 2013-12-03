using UnityEngine;
using System.Collections;
using System.Threading;

enum WeaponType { AR, PISTOL, SHOTGUN};

public class Gun : MonoBehaviour
{
	public Rigidbody2D bullet;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
	public Transform bulletSpawner;			// Where the bullets spawn.
	public Transform[] flechetteSpawner;
	public float fireRate;

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
	private WeaponType currWT;


	void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
		currWT = WeaponType.SHOTGUN;
		//flechetteSpawner = new Transform[3];
		/*for (int i = 0; i < flechetteSpawner.Length; i++) {
			flechetteSpawner[i] = GameObject.Find ("
		}*/
	}


	void Update ()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			fireWeapon();
			/*
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			//audio.Play();

			Vector2 fireDirection;
			fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);

			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed * fireDirection.x, speed * fireDirection.y);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-1*speed * (fireDirection.x), speed * (fireDirection.y));
			}
			*/
		}
	}

	void fireWeapon(){
		Vector2 fireDirection;
		fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
		fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);

		switch(currWT){
		case WeaponType.AR:
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			
			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed * fireDirection.x, speed * fireDirection.y);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-1*speed * (fireDirection.x), speed * (fireDirection.y));
			}
			break;
		case WeaponType.SHOTGUN:
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			
			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D[] flechetteArray = new Rigidbody2D[7];
				Random rnd = new Random();

				for(int i = 0; i < flechetteArray.Length; i++){
					int flechSpawner = randIntGen ();
					float flechSpeed = randFloatGen ();
					flechSpeed *= speed;
					//Debug.Log (flechSpawner);
					flechetteArray[i] = Instantiate(bullet, flechetteSpawner[flechSpawner].position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					flechetteArray[i].velocity = new Vector2(flechSpeed * fireDirection.x, flechSpeed * fireDirection.y);
				}
				//Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			}
			else
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D[] flechetteArray = new Rigidbody2D[7];
				Random rnd = new Random();
				
				for(int i = 0; i < flechetteArray.Length; i++){
					int flechSpawner = randIntGen ();
					float flechSpeed = randFloatGen ();
					flechSpeed *= speed;
					//Debug.Log (flechSpawner);
					flechetteArray[i] = Instantiate(bullet, flechetteSpawner[flechSpawner].position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
					flechetteArray[i].velocity = new Vector2(-1*flechSpeed * fireDirection.x, flechSpeed * fireDirection.y);
				}
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				//Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				//bulletInstance.velocity = new Vector2(-1*speed * (fireDirection.x), speed * (fireDirection.y));
			}
			break;
		}
	}

	int randIntGen(){

		return Random.Range (0, 2);
	}

	float randFloatGen(){
		return Random.Range (0.6f, 1.0f);
	}
}
