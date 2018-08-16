using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowScreen :MonoBehaviour {

    public Image image;
    private Color c;
    private void Awake() {
        image = GetComponent<Image>();



    }

    public void OnEnable() {
        try {
            if (GameManager.Instance.activatedJul) {
                StartCoroutine(FakeLerp());
            }
        } catch (System.NullReferenceException) {
        }
    }

    IEnumerator FakeLerp() {

        c = image.color;
        for (float i = 16; i > 1; i--) {

            c.a = (i / 2) / 10;
            image.color = c;

            yield return new WaitForSeconds(0.05f);
        }

    }
}
