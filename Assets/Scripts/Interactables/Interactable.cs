using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour {

    public bool withinInteractRange;
    public bool recentlyInteractedWith;

    public Collider2D player;
    public GameObject text;

    // Use this for initialization
    void Start() {
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (withinInteractRange && Input.GetKeyDown(KeyCode.E) && !recentlyInteractedWith) {
            InteractEffect();
            recentlyInteractedWith = true;
            text.SetActive(false);
            StartCoroutine(InteractTimer());
        }
    }

    void OnTriggerEnter2D(Collider2D other) {        
        if (other.tag == "Player") {
            withinInteractRange = true;
            player = other;
            if (!recentlyInteractedWith) {
                text.SetActive(true);
            }            
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            withinInteractRange = false;
            player = null;
            text.SetActive(false);
            ExitEffect();
        }
    }

    public virtual void InteractEffect() {
        //överskugga i individuella interactables
    }

    public virtual void ExitEffect() {
        //överskugga i individuella interactables
    }

    IEnumerator InteractTimer() {
        yield return new WaitForSeconds(2);
        recentlyInteractedWith = false;
        if (withinInteractRange) {
            text.SetActive(true);
        }        
    }
}
