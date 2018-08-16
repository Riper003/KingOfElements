using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : PickUp {

    public int healValue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void PickUpEffect(Collider2D other) {
        other.gameObject.GetComponent<Character>().RestorHealth(healValue);
    }
}
