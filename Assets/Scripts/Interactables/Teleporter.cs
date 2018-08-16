using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Teleporter : Interactable {

    public Transform teleportLocation;
    public GameObject areYouSureCanvas;
    public Button proceed;
    public Button goBack;
   

    public override void InteractEffect() {
        if (areYouSureCanvas != null) {
            areYouSureCanvas.SetActive(true);
            proceed.onClick.AddListener(Proceed);
            goBack.onClick.AddListener(GoBack);
        } else {
            player.transform.position = teleportLocation.position;
        }
             
    }

    public void Proceed() {
        player.transform.position = teleportLocation.position;
        areYouSureCanvas.SetActive(false);
    }
    public void GoBack() {
        areYouSureCanvas.SetActive(false);
    }

    public override void ExitEffect() {
        if (areYouSureCanvas != null) {
            areYouSureCanvas.SetActive(false);
        }
    }

}
