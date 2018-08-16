using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour {

    private float xPos;
    private float yPos;
	
	// Update is called once per frame
	void Update () {

        xPos = transform.position.x;
        yPos = transform.position.y;
        transform.position = new Vector3(xPos,yPos,yPos);
    }
}
