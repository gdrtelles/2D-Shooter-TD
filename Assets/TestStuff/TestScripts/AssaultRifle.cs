using UnityEngine;
using System.Collections;

public class AssaultRifle : Weapon {
	public float _rateOfFire;
	public float _reloadSpeed;
	public int _totalRounds;
	public int _currentRounds;
	public int _clipSize;
	public int _clips;
	//public Sprite _gunSprite;

	//To be made private
	public Animator anim;
	public PlayerControl2 playerCtrl;
	public Rigidbody2D bullet;                             // Prefab of the rocket.
	public float speed = 20f;
	public Transform bulletSpawner;

	public GUIText ammoCounter;

	// Use this for initialization
	void Start () {
		GameObject ammCounterObject = GameObject.Find ("AmmoCounter");
		ammoCounter = ammCounterObject.GetComponent<GUIText> ();
		initWeapon();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton ("Fire1") && canFire == true){
			fireWeapon ();
		}
	}

	public override void initWeapon(){
		rateOfFire = _rateOfFire;
		reloadSpeed = _reloadSpeed;
		totalRounds = _totalRounds;
		currentRounds = _currentRounds;
		clipSize = _clipSize;
		clips = _clips;
		canFire = true;
	}

	public override void fireWeapon(){
		//if(_currentRounds > 0){
		Debug.Log ("fireweapon");
			Vector2 fireDirection;
			fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			canFire = false;
			// ... set the animator Shoot trigger parameter and play the audioclip.
			//anim.SetTrigger("Shoot");
			//audioSource.Play();
			//
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
			Invoke ("enableFire", rateOfFire);
			currentRounds--;
		//}
	}

	void enableFire(){
		canFire = true;
	}

	public override void reloadWeapon(){
	}
}
