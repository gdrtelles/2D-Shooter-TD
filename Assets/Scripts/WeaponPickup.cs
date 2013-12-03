using UnityEngine;
using System.Collections;

public class WeaponPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.CompareTag("Player")){
			if(this.name == "ARPickup"){
				col.collider.gameObject.GetComponentInChildren<Gun>().setWeapon(0);
				Destroy(this.gameObject);
			}
			else if(this.name == "ShotgunPickup"){
				col.collider.gameObject.GetComponentInChildren<Gun>().setWeapon(1);
				Destroy (this.gameObject);
			}
		}
	}
}
