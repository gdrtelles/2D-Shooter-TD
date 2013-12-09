using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{



	void Start () 
	{
		// Destroy the bullet after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 1);
	}



	
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy...
		if(col.tag == "Enemy")
		{
			Debug.Log("hey");
			// ... find the Enemy script and call the Hurt function.
			col.gameObject.GetComponent<Enemy>().Hurt();



			// Destroy the bullet.
			Destroy (gameObject,0.06f);
		}


}
}