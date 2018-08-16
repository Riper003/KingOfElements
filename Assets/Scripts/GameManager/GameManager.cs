using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System;

public class GameManager :MonoBehaviour {
    public static GameManager instance = null;


    public int specc { get; set; }
    public int currency { get; set; }
    public int floorNumber { get; set; }

    [HideInInspector]
    public Text currencyText;

    //Upgrades
    public float damageUpgrade;
    public int damageUpgradeCount;
    public int aoeUpgrade;
    public int aoeUpgradeCount;
    public float healthUpgrade;
    public int healthUpgradeCount;
    public float dashUpgrade;
    public int dashUpgradeCount;
    public float ultiUpgrade;
    public int ultiUpgradeCount;
    

    //Control Options
    public bool dashToMouse;

    //UI Skill Icons
    [HideInInspector]
    public GameObject meleeActiveIcon;
    [HideInInspector]
    public GameObject rangedActiveIcon;
    [HideInInspector]
    public GameObject dashActiveIcon;
    [HideInInspector]
    public GameObject meleeCDIcon;
    [HideInInspector]
    public GameObject rangedCDIcon;
    [HideInInspector]
    public GameObject dashCDIcon;

    //UI Skill Sprites
    [HideInInspector]
    public Sprite meleeSprite;
    [HideInInspector]
    public Sprite rangeSprite;
    [HideInInspector]
    public Sprite dashSprite;

    //Specc way
    [HideInInspector]
    public GameObject fireWay;
    [HideInInspector]
    public GameObject frostWay;
    [HideInInspector]
    public GameObject lightningWay;

    //AudioSources
    public AudioClip playerDamageSound;
    public AudioClip enemyDamageSound;
    public AudioClip shatterSound;
    public AudioSource enemyHurtSource;
    public AudioSource playerHurtSource;
    public AudioSource[] otherSources;
    public AudioSource musicPlayer;
    private bool enemySoundsPlaying;
    private bool playerSoundsPlaying;


    //AudioClips
    public AudioClip[] musicTracks;
    [HideInInspector]
    public int savedTrack;

    //StatusEffect
    public GameObject hitEffect;
    public GameObject slowEffect;
    public GameObject burnEffect;
    public GameObject freezeEffect;
    public GameObject stunEffect;
    public GameObject julEffect;

    //Jul
    public bool activatedJul;
    [HideInInspector]
    public GameObject snowParticles;

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    public void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    void Start() {
        ResetAll();
    }

    public void AddCurrency(int amount) {
        currency += amount;
        UpdateCurrencyText();
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {

            ToggleJul();

        }
    }

    public void ResetAll() {
        damageUpgrade = 1;
        aoeUpgrade = 1;
        healthUpgrade = 1;
        damageUpgradeCount = 1;
        aoeUpgradeCount = 1;
        healthUpgradeCount = 1;
        dashUpgrade = 1;
        dashUpgradeCount = 1;
        ultiUpgrade = 1;
        ultiUpgradeCount = 1;

        specc = 0;
        currency = 0;
        meleeSprite = null;
        rangeSprite = null;
        dashSprite = null;

        dashToMouse = false;
    }

    public void UpdateCurrencyText() {
        currencyText.text = currency + "";
    }


    public void UpdateSkillIcons() {
        meleeSprite = meleeCDIcon.GetComponent<Image>().sprite;
        rangeSprite = rangedCDIcon.GetComponent<Image>().sprite;
        dashSprite = dashCDIcon.GetComponent<Image>().sprite;
    }
    public void UpdateSkillIconsOnLoad() {
        if (meleeSprite && rangeSprite && dashSprite != null && meleeCDIcon != null) {


            meleeCDIcon.GetComponent<Image>().sprite = meleeSprite;
            rangedCDIcon.GetComponent<Image>().sprite = rangeSprite;
            dashCDIcon.GetComponent<Image>().sprite = dashSprite;
            meleeActiveIcon.GetComponent<Image>().sprite = meleeSprite;
            rangedActiveIcon.GetComponent<Image>().sprite = rangeSprite;
            dashActiveIcon.GetComponent<Image>().sprite = dashSprite;
        }
    }

    public void StartCredits() {

        Application.LoadLevel(5);
    }

    public void PlayerHurtSound(AudioClip sound,float volume) {
        if (!playerSoundsPlaying) {
            playerSoundsPlaying = true;
            playerHurtSource.pitch = Random.Range(0.85f, 0.95f);
            playerHurtSource.PlayOneShot(sound, 0.4f);
            StartCoroutine(WaitForSound(1));
        }        
    }

    public void EnemyHurtSound(AudioClip sound, float volume) {
        if (!enemySoundsPlaying) {
            enemySoundsPlaying = true;
            enemyHurtSource.pitch = Random.Range(0.80f, 1.05f);
            enemyHurtSource.PlayOneShot(sound, 0.4f);
            StartCoroutine(WaitForSound(2));
        }        
    }

    public void OtherSounds(AudioClip sound, float volume) {
        for (int i = 0; i < otherSources.Length; i++) {
            if (!otherSources[i].isPlaying) {
                otherSources[i].pitch = Random.Range(0.80f, 1.05f);
                otherSources[i].PlayOneShot(sound, volume);
                return;
            }
        }     
    }

    IEnumerator WaitForSound(int source) {
        yield return new WaitForSeconds(0.1f);
        switch (source) {
            case 1:
                playerSoundsPlaying = false;
                break;
            case 2:
                enemySoundsPlaying = false;
                break;
        }
    }

    /*public void HitSounds(AudioClip sound,float volume) {
    for(int i = 0; i < enemySources.Length; i++) {
        if (!enemySources[i].isPlaying && !soundPlaying) {
            soundPlaying = true;
            enemySources[i].pitch = Random.Range(0.80f, 1.05f);
            enemySources[i].PlayOneShot(sound, volume);
            StartCoroutine(WaitForSound());
            return;
        }           
    }      
}*/

    public void MusicPlayer(int trackNumber) {
        if (trackNumber != 7)
            savedTrack = trackNumber;

        if (!activatedJul) {
            musicPlayer.clip = musicTracks[trackNumber];
            musicPlayer.loop = true;
            musicPlayer.Play();
            
        }


    }

    public void ToggleJul() {        
        if (!activatedJul) {
            
            MusicPlayer(7);
            activatedJul = true;
            
        } else {
            
            activatedJul = false;
            if (savedTrack != -1)
                MusicPlayer(savedTrack);
            else
                musicPlayer.Stop();
            
        }
        snowParticles.SetActive(activatedJul);
    }


    public void OpenRightWay() {
        if (fireWay != null && frostWay != null && lightningWay != null) {
            fireWay.SetActive(true);
            frostWay.SetActive(true);
            lightningWay.SetActive(true);

            switch (specc) {
                case 0:

                    break;
                case 1:
                    fireWay.SetActive(false);
                    break;
                case 2:
                    frostWay.SetActive(false);
                    break;
                case 3:
                    lightningWay.SetActive(false);
                    break;

            }
        }
    }

    public void HitEffect(Transform trans, Vector3 scale) {
        if(hitEffect != null) {
            GameObject clone;
            clone = Instantiate(hitEffect, trans.position, trans.rotation) as GameObject;
            clone.transform.localScale = scale;
            Destroy(clone.gameObject, 0.1f);
        }       
    }

}
