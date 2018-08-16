using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    public int level;


    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Application.LoadLevel(level);
        }
    }

}
