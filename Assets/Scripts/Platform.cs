using UnityEngine;
using System.Collections;


public class Platform : MonoBehaviour {
	
	public string playerName = "Player";
	public float yOffset = 0.8f;
	private GameObject player;
	private bool inside = false;
	
	//Find player by name
	void Start () {
		player = GameObject.Find(playerName);
	}

	
	//Check to see if player is under the platform. Collide only if the player is above the platform.
	void FixedUpdate () {
	
			if (player.transform.position.y < this.transform.position.y + yOffset) gameObject.collider2D.enabled = false;
			else gameObject.collider2D.enabled = true;

	}
}
