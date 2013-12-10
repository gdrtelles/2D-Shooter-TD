using UnityEngine;
using System.Collections;

enum WeaponPickupType{ AR, PISTOL, SHOTGUN};

public class WeaponPickup : MonoBehaviour {
	
	private WeaponPickupType WTPickup;
	
	// Use this for initialization
	void Start () {
		Invoke ("destroyPickup", 12.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if(col.collider.CompareTag("ground")){
			this.collider2D.isTrigger = true;
			this.rigidbody2D.gravityScale = 0.0f;
			//Destroy (rigidbody2D);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log (col.gameObject.tag);
		if(col.gameObject.CompareTag("Player")){
			Debug.Log ("first");
			if(WTPickup == WeaponPickupType.AR && col.GetComponentInChildren<Gun>().getCanPickup()){
				//Debug.Log ("here");
				col.gameObject.GetComponentInChildren<Gun>().setWeapon(0);
				Destroy(this.gameObject);
			}
			else if(WTPickup == WeaponPickupType.SHOTGUN && col.GetComponentInChildren<Gun>().getCanPickup()){
				col.gameObject.GetComponentInChildren<Gun>().setWeapon(1);
				Destroy (this.gameObject);
			}
			else if(WTPickup == WeaponPickupType.PISTOL && col.GetComponentInChildren<Gun>().getCanPickup()){
				if(col.gameObject.GetComponentInChildren<Gun>().getCurrWT() == WeaponType.PISTOL
				   || col.gameObject.GetComponentInChildren<Gun>().getCurrWT() == WeaponType.DUAL_PISTOL){
					col.gameObject.GetComponentInChildren<Gun>().setWeapon(3);
				}
				else {
					col.gameObject.GetComponentInChildren<Gun>().setWeapon(2);
				}
				Destroy (this.gameObject);
			}
		}
	}
	
	public void setWTPickup(int WTPickup){
		switch(WTPickup){
		case 0:
			this.WTPickup = WeaponPickupType.AR;
			break;
		case 1:
			this.WTPickup = WeaponPickupType.PISTOL;
			break;
		case 2:
			this.WTPickup = WeaponPickupType.SHOTGUN;
			break;
		}
	}

	void destroyPickup(){
		Destroy (this.gameObject);
	}
}