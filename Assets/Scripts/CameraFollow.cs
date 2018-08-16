using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    GameObject player;
    bool followPlayer = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            setFollowPlayer(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (followPlayer == true) {
            camFollowPlayer();
        }
	}

    public void setFollowPlayer(bool val) {
        followPlayer = val;
    }

    void camFollowPlayer() {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        this.transform.position = newPos;
    }
}