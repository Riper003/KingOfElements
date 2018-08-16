using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Loader :MonoBehaviour {

    public GameObject gameManager;

    void Awake() {

        if (GameManager.instance == null) {

            Instantiate(gameManager);
        }
    }

    void Start() {

        loadSceneSettings();

    }

    private void loadSceneSettings() {

        GameManager.Instance.floorNumber = Application.loadedLevel;



        if (Application.loadedLevel != 4) {

            GameManager.Instance.MusicPlayer(Application.loadedLevel);


        } else {
            if (!GameManager.Instance.activatedJul)
                GameManager.Instance.musicPlayer.Stop();

            GameManager.Instance.savedTrack = -1;

        }

        if (Application.loadedLevel == 5) {
            if (GameManager.Instance.activatedJul) {
                GameManager.Instance.activatedJul = false;
                GameManager.Instance.MusicPlayer(7);
                GameManager.Instance.activatedJul = true;
            }

            Invoke("EndCredit",34.5f);

        } else {

            if (Application.loadedLevel == 0) {
                GameManager.Instance.ResetAll();


            } else {
                GameManager.Instance.currencyText = GameObject.Find("CurrencyAmount").GetComponent<Text>();

                GameManager.Instance.fireWay = GameObject.Find("FireWay");
                GameManager.Instance.frostWay = GameObject.Find("FrostWay");
                GameManager.Instance.lightningWay = GameObject.Find("LightningWay");



                GameManager.Instance.meleeActiveIcon = GameObject.Find("MeleeActive");
                GameManager.Instance.rangedActiveIcon = GameObject.Find("RangeActive");
                GameManager.Instance.dashActiveIcon = GameObject.Find("DashActive");
                GameManager.Instance.meleeCDIcon = GameObject.Find("MeleeOnCd");
                GameManager.Instance.rangedCDIcon = GameObject.Find("RangeOnCd");
                GameManager.Instance.dashCDIcon = GameObject.Find("DashOnCd");
                GameManager.Instance.UpdateCurrencyText();
            }
        }
        GameManager.Instance.snowParticles = GameObject.Find("SnowParticles");
        GameManager.Instance.snowParticles.SetActive(GameManager.Instance.activatedJul);
        GameManager.Instance.OpenRightWay();
        GameManager.Instance.UpdateSkillIconsOnLoad();


    }


    void EndCredit() {
        Application.LoadLevel(0);
    }

}
