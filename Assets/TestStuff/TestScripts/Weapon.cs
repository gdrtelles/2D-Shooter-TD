using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	protected float rateOfFire;
	protected float reloadSpeed;
	protected int totalRounds;
	protected int clipSize;
	protected int clips;
	protected bool canFire;
	//protected Sprite gunSprite;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void initWeapon(){
	}

	public virtual void fireWeapon(){
	}

	public virtual void reloadWeapon(){
	}
}
