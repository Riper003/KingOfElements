using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JulSpriteSwitch :MonoBehaviour {

    public Sprite julSprite;
    public Sprite normalSprite;
    bool julActivated;
    public bool isUpgrade;

    // Update is called once per frame
    void FixedUpdate() {

        if (GameManager.Instance.activatedJul && !julActivated) {        
              
            SetSprite(julSprite);
            julActivated = true;
            if(isUpgrade)
                transform.localScale = new Vector3(1.5f,1.5f);
        }
        if (!GameManager.Instance.activatedJul && julActivated) {
            SetSprite(normalSprite);
            julActivated = false;
            if(isUpgrade)
                transform.localScale = new Vector3(1f,1f);
        }

    }

    void SetSprite(Sprite sprite) {
        if (GetComponent<Image>() != null)
            GetComponent<Image>().sprite = sprite;
        else
            GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
