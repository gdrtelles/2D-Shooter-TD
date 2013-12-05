using UnityEngine;
using System.Collections;

enum WeaponPickupType{ AR, PISTOL, SHOTGUN};

public class WeaponPickup : MonoBehaviour {
	
	private WeaponPickupType WTPickup;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D col){
		//Debug.Log ("first");
		if(col.gameObject.CompareTag("Player")){
			if(WTPickup == WeaponPickupType.AR){
				//Debug.Log ("here");
				col.collider.gameObject.GetComponentInChildren<Gun>().setWeapon(0);
				Destroy(this.gameObject);
			}
			else if(WTPickup == WeaponPickupType.SHOTGUN){
				col.collider.gameObject.GetComponentInChildren<Gun>().setWeapon(1);
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
}