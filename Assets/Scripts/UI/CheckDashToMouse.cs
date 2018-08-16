using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckDashToMouse :MonoBehaviour {
    public Text toggleButtonLable;

    private void OnEnable() {
        if (GameManager.Instance.dashToMouse)
            toggleButtonLable.text = "Dash to mouse: " + "On";
        else
            toggleButtonLable.text = "Dash to mouse: " + "Off";


    }
}
