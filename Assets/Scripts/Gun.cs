using UnityEngine;
using System.Collections;
using System.Threading;

public enum WeaponType { AR, PISTOL, SHOTGUN, DUAL_PISTOL};

public class Gun : MonoBehaviour
{
	public Sprite[] gunSprites;
	public float AR_RoF;
	public int AR_ROUNDS;
	public int AR_CLIPS;
	public float AR_RELOADSPEED;
	public float SHOTGUN_RoF;
	public int SHOTGUN_ROUNDS;
	public int SHOTGUN_CLIPS;
	public float SHOTGUN_RELOADSPEED;
	public float PISTOL_RoF;
	public int PISTOL_ROUNDS;
	public int PISTOL_CLIPS;
	public float PISTOL_RELOADSPEED;
	public float DUAL_PISTOL_RELOADSPEED;
	
	public GUIText ammoCounter;
	public GUIText ammoCounter1;
	public Rigidbody2D bullet;                             // Prefab of the rocket.
	public float speed = 20f;                              // The speed the rocket will fire at.
	public Transform bulletSpawner;                        // Where the bullets spawn.
	public Transform[] flechetteSpawner;
	public Transform bulletSpawnerPistol;
	public Transform bulletSpawnerPistol1;
	public GameObject secondPistol;
	
	public GameObject reloadBar;
	public GameObject reloadBar1;
	
	public AudioClip[] audioArray;
	private AudioSource audioSource;
	
	private PlayerControl playerCtrl;                // Reference to the PlayerControl script.
	private Animator anim;                                        // Reference to the Animator component.
	private WeaponType currWT;
	private Sprite currGunSprite;
	private SpriteRenderer gunRenderer;
	private int rounds;
	private int rounds1;
	private int clips;
	private int clips1;
	private int totalRounds;
	private int totalRounds1;
	private float fireRate;
	private float reloadSpeed;
	private bool canFire;
	private bool canFire1;
	private bool pistolFired1;
	private GameObject hand_R;
	private GameObject hand_RCopy;
	private int currWepTypeRounds;
	
	private Hashtable ht = new Hashtable();
	private Hashtable ht1 = new Hashtable();
	//private int shot
	
	
	void Awake()
	{
		//Default weapon stats
		AR_RoF = 0.15f;
		AR_ROUNDS = 50;
		AR_CLIPS = 4;
		AR_RELOADSPEED = 2f;
		SHOTGUN_RoF = 0.5f;
		SHOTGUN_ROUNDS = 4;
		SHOTGUN_CLIPS = 4;
		SHOTGUN_RELOADSPEED = 0.7f;
		PISTOL_RoF = 0.125f;
		PISTOL_ROUNDS = 7;
		PISTOL_CLIPS = 10;
		PISTOL_RELOADSPEED = 0.3f;
		DUAL_PISTOL_RELOADSPEED = 1.2f;
		
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		audioSource = this.GetComponent<AudioSource>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
		gunRenderer = this.GetComponent<SpriteRenderer>();
		hand_R = GameObject.Find ("hand_R");
		hand_RCopy = (GameObject)Instantiate(hand_R, hand_R.transform.position, hand_R.transform.rotation);
		hand_RCopy.transform.parent = this.transform.parent;
		
		setWeapon(2);
		canFire = true;
		canFire1 = true;
	}
	
	
	void Update ()
	{
		if(currWT == WeaponType.AR && Input.GetButton("Fire1") && canFire){
			fireWeapon();
		}
		else if(currWT == WeaponType.DUAL_PISTOL){
			if(Input.GetButtonDown ("Fire1") && canFire){
				pistolFired1 = false;
				fireWeapon ();
			}
			else if(Input.GetButtonDown ("Fire2") && canFire1){
				pistolFired1 = true;
				fireSecondPistol();
			}
		}
		else if(Input.GetButtonDown("Fire1") && canFire)
		{
			fireWeapon();
		}

		if(currWT != WeaponType.DUAL_PISTOL && Input.GetKeyDown (KeyCode.R) && canFire){
			canFire = false;
			ht = iTween.Hash ("from", 0f,"to", 1f, "time", reloadSpeed, "onupdate", "reloading");
			iTween.ValueTo(gameObject, ht);
			reloadBar.GetComponent<SpriteRenderer>().color = new Color (237f, 28f, 36f);
			Invoke ("reloadWeapon", reloadSpeed);
		}
		if(currWT == WeaponType.DUAL_PISTOL && Input.GetKeyDown(KeyCode.R) && canFire && canFire1){
			if(rounds < PISTOL_ROUNDS){
				canFire = false;
				ht = iTween.Hash ("from", 0f,"to", 1f, "time", reloadSpeed, "onupdate", "reloading");
				iTween.ValueTo(gameObject, ht);
				reloadBar.GetComponent<SpriteRenderer>().color = new Color (237f, 28f, 36f);
				Invoke ("reloadWeapon", reloadSpeed);
			}
			if(rounds1 < PISTOL_ROUNDS){
				canFire1 = false;
				ht1 = iTween.Hash ("from", 0f,"to", 1f, "time", reloadSpeed, "onupdate", "reloading1");
				reloadBar1.GetComponent<SpriteRenderer>().color = new Color (237f, 28f, 36f);
				iTween.ValueTo(gameObject, ht1);
				Invoke ("reloadSecondaryWeapon", reloadSpeed);
			}
		}
	}
	
	void fireWeapon(){
		if(rounds > 0){
			Vector2 fireDirection;
			fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			canFire = false;
			switch(currWT){
			case WeaponType.AR:
				// ... set the animator Shoot trigger parameter and play the audioclip.
				anim.SetTrigger("Shoot");
				audioSource.Play();
				
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
					bulletInstance.velocity = new Vector2(-1*speed * fireDirection.x, speed * fireDirection.y);
				}
				rounds--;
				break;
			case WeaponType.SHOTGUN:
				// ... set the animator Shoot trigger parameter and play the audioclip.
				anim.SetTrigger("Shoot");
				audioSource.Play ();
				// If the player is facing right...
				if(playerCtrl.facingRight)
				{
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D[] flechetteArray = new Rigidbody2D[5];
					
					for(int i = 0; i < flechetteArray.Length; i++){
						int flechSpawner = randIntGen ();
						float flechSpeed = randFloatGen (false);
						float spread = randFloatGen(true);
						flechSpeed *= speed;
						//Debug.Log (flechSpawner);
						flechetteArray[i] = Instantiate(bullet, flechetteSpawner[flechSpawner].position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
						//flechetteArray[i].transform.eulerAngles = new Vector3(0, 0, 5.0f);
						flechetteArray[i].velocity = new Vector2((flechSpeed * fireDirection.x) + spread, (flechSpeed * fireDirection.y) - spread);
					}
					//Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				}
				else
				{
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D[] flechetteArray = new Rigidbody2D[4];
					
					for(int i = 0; i < flechetteArray.Length; i++){
						int flechSpawner = randIntGen ();
						float flechSpeed = randFloatGen (false);
						flechSpeed *= speed;
						float spread = randFloatGen (true);
						//Debug.Log (flechSpawner);
						flechetteArray[i] = Instantiate(bullet, flechetteSpawner[flechSpawner].position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
						flechetteArray[i].velocity = new Vector2((-1*flechSpeed * fireDirection.x) + spread, (flechSpeed * fireDirection.y) - spread);
					}
					// Otherwise instantiate the rocket facing left and set it's velocity to the left.
					//Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawner.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
					//bulletInstance.velocity = new Vector2(-1*speed * (fireDirection.x), speed * (fireDirection.y));
				}
				rounds--;
				break;
			case WeaponType.PISTOL:
				// ... set the animator Shoot trigger parameter and play the audioclip.
				anim.SetTrigger("Shoot");
				audioSource.Play();
				
				// If the player is facing right...
				if(playerCtrl.facingRight)
				{
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawnerPistol.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(speed * fireDirection.x, speed * fireDirection.y);
				}
				else
				{
					// Otherwise instantiate the rocket facing left and set it's velocity to the left.
					Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawnerPistol.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(-1*speed * fireDirection.x, speed * fireDirection.y);
				}
				rounds--;
				break;
			case WeaponType.DUAL_PISTOL:
				// ... set the animator Shoot trigger parameter and play the audioclip.
				anim.SetTrigger("Shoot");
				audioSource.Play();
				
				// If the player is facing right...
				if(playerCtrl.facingRight)
				{
					// ... instantiate the rocket facing right and set it's velocity to the right. 
					Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawnerPistol.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(speed * fireDirection.x, speed * fireDirection.y);
				}
				else
				{
					// Otherwise instantiate the rocket facing left and set it's velocity to the left.
					Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawnerPistol.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
					bulletInstance.velocity = new Vector2(-1*speed * fireDirection.x, speed * fireDirection.y);
				}
				rounds--;
				break;
			}
		}
		ammoCount();
	}
	
	void fireSecondPistol(){
		if(rounds1 > 0){
			Vector2 fireDirection;
			fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			canFire1 = false;
			// ... set the animator Shoot trigger parameter and play the audioclip.
			anim.SetTrigger("Shoot");
			audioSource.Play();
			
			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawnerPistol1.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed * fireDirection.x, speed * fireDirection.y);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(bullet, bulletSpawnerPistol1.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-1*speed * fireDirection.x, speed * fireDirection.y);
			}
			rounds1--;
		}
		ammoCount ();
	}
	
	void reloadSecondaryWeapon(){
		audioSource.PlayOneShot(audioArray[2]);
		if(totalRounds1 - PISTOL_ROUNDS >= 0){
			totalRounds1 -= (PISTOL_ROUNDS - rounds1);
			rounds1 = PISTOL_ROUNDS;
		}
		else{
			rounds1 = totalRounds1;
			totalRounds1 = 0;
		}
		reloadBar1.GetComponent<SpriteRenderer>().color = new Color(19f, 243f, 99f);
		setAmmoCounter();
		canFire1 = true;
	}
	
	int randIntGen(){
		
		return Random.Range (0, 3);
	}
	
	float randFloatGen(bool spread){
		if(!spread){
			return Random.Range (0.8f, 1.0f);
		}
		else{
			int a = Random.Range (0, 2);
			if(a == 0){	return Random.Range (0f, 4f);}
			else{ return Random.Range (0f, -4f);}
			//return -20f;
		}
	}
	
	void cycleWeapon(){
		if(currWT == WeaponType.AR){
			setWeapon (1);
		}
		else if(currWT == WeaponType.SHOTGUN){
			setWeapon(2);
		}
		else {
			setWeapon(0);
		}
	}
	
	public void setWeapon(int weapon){
		audioSource.PlayOneShot(audioArray[2]);
		pistolFired1 = false;
		reloadBar.transform.localScale = new Vector3(1, 1, 1);
		reloadBar.GetComponent<SpriteRenderer>().color = new Color(19f, 243f, 99f);
		if(weapon == 0){
			audioSource.clip = audioArray[0];
			currWT = WeaponType.AR;
			fireRate = AR_RoF;
			currWepTypeRounds = AR_ROUNDS;
			rounds = AR_ROUNDS;
			clips = AR_CLIPS;
			totalRounds = rounds * clips;
			reloadSpeed = AR_RELOADSPEED;
			gunRenderer.sprite = gunSprites[0];
			hand_R.GetComponent<SpriteRenderer>().enabled = true;
			hand_RCopy.SetActive(false);
			secondPistol.GetComponent<SpriteRenderer>().enabled = false;
			reloadBar1.GetComponent<SpriteRenderer>().enabled = false;
			ammoCounter1.text = "";
		}
		else if(weapon == 1){
			audioSource.clip = audioArray[1];
			currWT = WeaponType.SHOTGUN;
			fireRate = SHOTGUN_RoF;
			currWepTypeRounds = SHOTGUN_ROUNDS;
			rounds = SHOTGUN_ROUNDS;
			clips = SHOTGUN_CLIPS;
			totalRounds = rounds * clips;
			reloadSpeed = SHOTGUN_RELOADSPEED;
			gunRenderer.sprite = gunSprites[1];
			hand_R.GetComponent<SpriteRenderer>().enabled = true;
			hand_RCopy.SetActive(false);
			secondPistol.GetComponent<SpriteRenderer>().enabled = false;
			reloadBar1.GetComponent<SpriteRenderer>().enabled = false;
			ammoCounter1.text = "";
		}
		else if(weapon == 2){
			audioSource.clip = audioArray[0];
			currWT = WeaponType.PISTOL;
			fireRate = PISTOL_RoF;
			currWepTypeRounds = PISTOL_ROUNDS;
			rounds = PISTOL_ROUNDS;
			clips = PISTOL_CLIPS;
			totalRounds = rounds * clips;
			reloadSpeed = PISTOL_RELOADSPEED;
			gunRenderer.sprite = gunSprites[3];
			hand_R.GetComponent<SpriteRenderer>().enabled = false;
			hand_RCopy.SetActive(true);
			secondPistol.GetComponent<SpriteRenderer>().enabled = false;
			reloadBar1.GetComponent<SpriteRenderer>().enabled = false;
			ammoCounter1.text = "";
		}
		else if(weapon == 3){
			audioSource.clip = audioArray[0];
			currWT = WeaponType.DUAL_PISTOL;
			fireRate = PISTOL_RoF;
			currWepTypeRounds = PISTOL_ROUNDS;
			rounds = PISTOL_ROUNDS;
			rounds1 = PISTOL_ROUNDS;
			clips = PISTOL_CLIPS;
			clips1 = PISTOL_CLIPS;
			totalRounds = rounds * clips;
			totalRounds1 = rounds * clips;
			reloadSpeed = DUAL_PISTOL_RELOADSPEED;
			gunRenderer.sprite = gunSprites[3];
			hand_R.GetComponent<SpriteRenderer>().enabled = true;
			hand_RCopy.SetActive(false);
			secondPistol.GetComponent<SpriteRenderer>().enabled = true;
			reloadBar1.GetComponent<SpriteRenderer>().enabled = true;
			canFire1 = true;
		}
		setAmmoCounter();
		canFire = true;
	}
	
	void enableFire(){
		canFire = true;
	}
	
	void enableSecondaryFire(){
		canFire1 = true;
	}
	
	void ammoCount(){
		//Debug.Log ("ammCOunt");
		if(rounds > 0 && currWT != WeaponType.DUAL_PISTOL){
			Invoke ("enableFire", fireRate);
		}
		else if(rounds > 0 && !pistolFired1){ 
			Invoke ("enableFire", fireRate);
		}
		else if(rounds <= 0 && totalRounds >= 0 && !pistolFired1){ 
			ht = iTween.Hash ("from", 0f,"to", 1f, "time", reloadSpeed, "onupdate", "reloading");
			iTween.ValueTo(gameObject, ht);
			//clips = totalRounds/
			reloadBar.GetComponent<SpriteRenderer>().color = new Color (237f, 28f, 36f);
			Invoke ("reloadWeapon", reloadSpeed);
		}
		else if(rounds <= 0 && totalRounds <= 0){ canFire = false;}
		if(rounds1 > 0 && pistolFired1){
			Invoke("enableSecondaryFire", fireRate);
		}
		else if(rounds1 <= 0 && totalRounds1 >= 0 && pistolFired1){
			//Debug.Log ("reload second weapon1");
			ht1 = iTween.Hash ("from", 0f,"to", 1f, "time", reloadSpeed, "onupdate", "reloading1");
			iTween.ValueTo(gameObject, ht1);
			reloadBar1.GetComponent<SpriteRenderer>().color = new Color (237f, 28f, 36f);
			Invoke ("reloadSecondaryWeapon", reloadSpeed);
		}
		else if(rounds1 <= 0 && totalRounds1 <= 0){ canFire1 = false;}
		setAmmoCounter();
	}
	
	void reloadWeapon(){
		//Debug.Log (reloadBar.transform.localScale);
		//reloadBar.transform.localScale = new Vector3(1, 1, 1);
		//Debug.Log (reloadBar.transform.localScale);
		audioSource.PlayOneShot(audioArray[2]);
		
		switch(currWT){
		case WeaponType.AR:
			if(totalRounds - AR_ROUNDS >= 0){
				totalRounds -= (AR_ROUNDS - rounds);
				rounds = AR_ROUNDS;
			}
			else {
				rounds = totalRounds;
				totalRounds = 0;
			}
			break;
		case WeaponType.SHOTGUN:
			if(totalRounds - 1 >= 0 && rounds < SHOTGUN_ROUNDS){
				totalRounds --;
				rounds++;
			}
			break;
		case WeaponType.PISTOL:
			if(totalRounds - PISTOL_ROUNDS >= 0){
				totalRounds -= (PISTOL_ROUNDS - rounds);
				rounds = PISTOL_ROUNDS;
			}
			else {
				rounds = totalRounds;
				totalRounds = 0;
			}
			break;
		case WeaponType.DUAL_PISTOL:
			/*if(pistolFired1){
				if(totalRounds1 - PISTOL_ROUNDS >= 0){
					totalRounds1 -= (PISTOL_ROUNDS - rounds1);
					rounds1 = PISTOL_ROUNDS;
				}
				else{
					rounds1 = totalRounds1;
					totalRounds1 = 0;
				}
				canFire1 = true;
			}*/
			if(totalRounds - PISTOL_ROUNDS >= 0){
				totalRounds -= (PISTOL_ROUNDS - rounds);
				rounds = PISTOL_ROUNDS;
			}
			else {
				rounds = totalRounds;
				totalRounds = 0;
			}
			break;
		}
		reloadBar.GetComponent<SpriteRenderer>().color = new Color(19f, 243f, 99f);
		setAmmoCounter();
		canFire = true;
	}
	
	void setAmmoCounter(){
		if(currWT == WeaponType.DUAL_PISTOL){
			ammoCounter.text = rounds + "/" + totalRounds;
			ammoCounter1.text = rounds1 + "/" + totalRounds1;
			if(pistolFired1){
				reloadBar1.transform.localScale = new Vector3(reloadBar1.transform.localScale.x - (1f/currWepTypeRounds), 1, 1);
			}
			else{
				reloadBar.transform.localScale = new Vector3(reloadBar.transform.localScale.x - (1f/currWepTypeRounds), 1, 1);
			}
		}
		else {
			//Debug.Log (1f/rounds);
			reloadBar.transform.localScale = new Vector3(reloadBar.transform.localScale.x - (1f/currWepTypeRounds), 1, 1);
			ammoCounter.text = rounds + "/" + totalRounds;
		}
	}
	
	public void reloading(float reloading){
		//Debug.Log (reloading);
		reloadBar.transform.localScale = new Vector3(reloading, 1, 1);
	}
	
	public void reloading1(float reloading){
		//Debug.Log (reloading);
		reloadBar1.transform.localScale = new Vector3(reloading, 1, 1);
	}
	
	public WeaponType getCurrWT(){
		return currWT;
	}
}