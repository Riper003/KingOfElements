using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpen : MonoBehaviour {

    AggroHandler aggroHandler;
    public GameObject closedWay;

    private void Start() {
        aggroHandler = gameObject.GetComponent<AggroHandler>();
        closedWay.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if (aggroHandler.triggerd) {
            for (int i = 0; i < aggroHandler.enemyArray.Length; i++) {
                if (aggroHandler.enemyArray[i] != null) {
                    return;

                }
            }
            closedWay.SetActive(false);
        }
	}
}
