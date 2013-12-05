using UnityEngine;
using System.Collections;
using System.Threading;

enum WeaponType { AR, PISTOL, SHOTGUN};

public class Gun : MonoBehaviour
{
	public float AR_RoF = 0.15f;
	public int AR_ROUNDS = 40;
	public int AR_CLIPS = 4;
	public float AR_RELOADSPEED = 3.0f;
	public float SHOTGUN_RoF = 0.6f;
	public int SHOTGUN_ROUNDS = 4;
	public int SHOTGUN_CLIPS = 4;
	public float SHOTGUN_RELOADSPEED = 2.5f;
	//public float PISTOL_RoF = 0.2f;
	//public int PISTOL_ROUNDS = 7;
	
	public GUIText ammoCounter;
	public Rigidbody2D bullet;                                // Prefab of the rocket.
	public float speed = 20f;                                // The speed the rocket will fire at.
	public Transform bulletSpawner;                        // Where the bullets spawn.
	public Transform[] flechetteSpawner;
	
	private PlayerControl playerCtrl;                // Reference to the PlayerControl script.
	private Animator anim;                                        // Reference to the Animator component.
	private WeaponType currWT;
	private int rounds;
	private int clips;
	private float fireRate;
	private float reloadSpeed;
	private bool canFire;
	//private int shot
	
	
	void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
		setWeapon(0);
		canFire = true;
	}
	
	
	void Update ()
	{
		if(currWT == WeaponType.AR && Input.GetButton("Fire1") && canFire == true){
			fireWeapon();
		}
		else if(Input.GetButtonDown("Fire1") && canFire == true)
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
		if(Input.GetKeyDown(KeyCode.G)){
			cycleWeapon();
		}
	}
	
	void fireWeapon(){
		Vector2 fireDirection;
		fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
		fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
		canFire = false;
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
		ammoCount();
	}
	
	int randIntGen(){
		
		return Random.Range (0, 2);
	}
	
	float randFloatGen(){
		return Random.Range (0.5f, 1.0f);
	}
	
	void cycleWeapon(){
		if(currWT == WeaponType.AR){
			setWeapon (1);
		}
		else{
			setWeapon(0);
		}
	}
	
	public void setWeapon(int weapon){
		if(weapon == 0){
			currWT = WeaponType.AR;
			fireRate = AR_RoF;
			rounds = AR_ROUNDS;
			clips = AR_CLIPS;
			reloadSpeed = AR_RELOADSPEED;
		}
		else if(weapon == 1){
			currWT = WeaponType.SHOTGUN;
			fireRate = SHOTGUN_RoF;
			rounds = SHOTGUN_ROUNDS;
			clips = SHOTGUN_CLIPS;
			reloadSpeed = SHOTGUN_RELOADSPEED;
		}
		setAmmoCounter();
		canFire = true;
	}
	
	void enableFire(){
		canFire = true;
	}
	
	void ammoCount(){
		if(rounds > 0){ 
			rounds--;
			setAmmoCounter();
			Invoke("enableFire", fireRate);
		}
		else if(rounds == 0 && clips > 0){ 
			clips--; 
			Invoke ("reloadWeapon", reloadSpeed);
		}
		else { setAmmoCounter(); canFire = false;}
	}
	
	void reloadWeapon(){
		switch(currWT){
		case WeaponType.AR:
			rounds = AR_ROUNDS;
			break;
		case WeaponType.SHOTGUN:
			rounds = SHOTGUN_ROUNDS;
			break;
		}
		setAmmoCounter();
		canFire = true;
	}
	
	void setAmmoCounter(){
		ammoCounter.text = rounds + "/" + clips;
	}
}