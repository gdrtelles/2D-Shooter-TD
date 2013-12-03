using UnityEngine;
using System.Collections;
/*
 * Some simple code for One Way (cloud) Platforms.
 * USAGE: works with horizontal platforms that only the player will stand on.
 * Platform pivot should be where the player can stand. Player pivot should be at the player's feet.
 * <3 - @x01010111
 */

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
