using UnityEngine;
using System.Collections;

public class AssaultRifle : Weapon {
	public float _rateOfFire;
	public float _reloadSpeed;
	public int _totalRounds;
	public int _clipSize;
	public int _clips;
	//public Sprite _gunSprite;
	// Use this for initialization
	void Start () {
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
		clipSize = _clipSize;
		clips = _clips;
		canFire = true;
	}

	public override void fireWeapon(){
		if(rounds > 0){
			Vector2 fireDirection;
			fireDirection.x = Mathf.Cos((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			fireDirection.y = Mathf.Sin((this.transform.eulerAngles.z) * Mathf.Deg2Rad);
			canFire = false;
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
		}
	}

	public override void reloadWeapon(){
	}
}
