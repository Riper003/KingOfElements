using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : PickUp {

    private int amount;
    public int minAmount;
    public int maxAmount;

	// Use this for initialization
	void Start () {
        amount = Random.Range(minAmount, maxAmount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void PickUpEffect(Collider2D other) {
        //other.gameObject.GetComponent<Player>().GetCurrency(amount);
        GameManager.Instance.AddCurrency(amount);
    }
}
