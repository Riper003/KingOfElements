using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroHandler :MonoBehaviour {

    public GameObject[] enemyArray;
    public bool triggerd;

    // Use this for initialization
    void Start() {
        triggerd = false;
    }

    // Update is called once per frame
    void Update() {

    }


    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            ActivateAggro(1);

        }
    }

   public void ActivateAggro(int a) {
        if (enemyArray.Length > 0) {
            for (int i = 0; i < enemyArray.Length; i++) {
                if (enemyArray[i] != null) {
                    triggerd = true;
                    enemyArray[i].GetComponentInChildren<Enemy>().StartAggro();
                    if(a == 2) {
                        enemyArray[i].GetComponentInChildren<Enemy>().isAggro = true;
                    }
                    
                }
            }

        }
    }
}
