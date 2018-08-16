using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public AudioClip pickUpSound;
    public float volume;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            PickUpEffect(other);
            Destroy(gameObject);
        }
    }

    public virtual void PickUpEffect(Collider2D other) {
        //överskugga i individuella pickups
    }

    void OnDestroy() {
        if(pickUpSound != null)
            GameManager.Instance.OtherSounds(pickUpSound,volume);
    }

}
