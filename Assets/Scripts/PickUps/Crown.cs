using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crown :PickUp {

    // Use this for initialization
    void Start() {
        StartCoroutine(TurnOn());
        
    }

    IEnumerator TurnOn() {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        GetComponent<Collider2D>().enabled = true;


    }

    public override void PickUpEffect(Collider2D other) {
        //other.gameObject.GetComponent<Player>().GetCurrency(amount);
        GameManager.Instance.musicPlayer.Stop();
        GameManager.Instance.musicPlayer.volume = 0.2f;
        GameManager.Instance.musicPlayer.PlayOneShot(GameManager.Instance.musicTracks[6]);
        GameManager.Instance.savedTrack = -1;

        GameManager.Instance.Invoke("StartCredits",5);
    }
    
}
