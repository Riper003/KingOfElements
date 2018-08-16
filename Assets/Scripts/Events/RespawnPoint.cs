using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnPoint : MonoBehaviour {

    private bool checkpointReached;

	// Use this for initialization
	void Start () {
        checkpointReached = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !checkpointReached) {
            other.GetComponent<Player>().startPos = transform.position;
            other.GetComponent<Player>().feedbackText.text = "Checkpoint reached!";
            other.GetComponent<Player>().feedbackText.color = Color.yellow;
            other.GetComponent<Player>().feedbackCanvas.SetActive(true);
            StartCoroutine(other.GetComponent<Player>().TextDuration());
            checkpointReached = true;
        }
    }

}
