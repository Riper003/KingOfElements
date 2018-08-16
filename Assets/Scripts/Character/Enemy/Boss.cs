using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss :Enemy {

    public bool goneCrazy1;
    public bool goneCrazy2;
    public bool goneCrazy3;

    public Transform[] attackSpots;
    public Transform[] frostAttackSpots;
    public Transform[] frostHitSpots;
    public Text bossHealthProcent;

    public GameObject shadow;

    public GameObject bossHPBar;

    public override void Start() {
        goneCrazy1 = false;
        goneCrazy2 = false;
        goneCrazy3 = false;

        base.Start();
        enemyAttack[attack.specc].gameObject.transform.localScale = new Vector3(1.5f,1.5f);

    }


    // Update is called once per frame
    public override void Update() {
        if (isAggro && !bossHPBar.activeInHierarchy) {
            bossHPBar.SetActive(true);
            GameManager.Instance.MusicPlayer(4);
        }

        if (currentHealth <= maxHealth * 0.75f && !goneCrazy1 && !chillOut) {
            Unfreeze();
            currentHealth = maxHealth * 0.75f;

            UpdateCurrentHealthBar();
            BeforePattern();

            StartCoroutine(BossAttackPattern());
            goneCrazy1 = true;



        }
        if (currentHealth <= maxHealth * 0.5f && !goneCrazy2 && !chillOut) {
            Unfreeze();
            currentHealth = maxHealth * 0.5f;
            UpdateCurrentHealthBar();
            BeforePattern();
            StartCoroutine(BossAttackPatternFire());
            goneCrazy2 = true;



        }
        if (currentHealth <= maxHealth * 0.25f && !goneCrazy3 && !chillOut) {
            Unfreeze();
            currentHealth = maxHealth * 0.25f;
            UpdateCurrentHealthBar();
            BeforePattern();
            StartCoroutine(BossAttackPatternFrost());

            goneCrazy3 = true;
            moveSpeed = 13;

        }

        base.Update();
    }

    public override void Unfreeze() {
        base.Unfreeze();
        chillOut = false;
    }

    public override void TakeDamage(float dmg) {

        base.TakeDamage(dmg);
        bossHealthProcent.text = Mathf.RoundToInt((currentHealth / maxHealth) * 100) + "%";
    }

    IEnumerator BossAttackPattern() {

        yield return new WaitForSeconds(0.8f);

        enemyAttack[attack.specc].gameObject.transform.localScale = new Vector3(0.5f,0.5f);
        for (int g = 0; g < 10; g++) {
            yield return new WaitForSeconds(0.7f);
            for (int i = 0; i < attackSpots.Length; i++) {
                if (i % 2 == 0) {
                    attackPos = attackSpots[i].position;
                    attack.ActivateSkill(enemyAttack,attackPos);
                }
            }
            yield return new WaitForSeconds(0.7f);
            for (int i = 0; i < attackSpots.Length; i++) {
                if (i % 2 != 0) {
                    attackPos = attackSpots[i].position;
                    attack.ActivateSkill(enemyAttack,attackPos);
                }
            }
        }
        AfterPattern();


        yield return new WaitForSeconds(chillOutTime);
        isStunned = false;
        chillOut = false;

    }

    IEnumerator BossAttackPatternFire() {
        yield return new WaitForSeconds(0.8f);
        for (int g = 0; g < 3; g++) {

            for (int i = 0; i < attackSpots.Length; i++) {
                int b = i;

                attackPos = attackSpots[i].position;
                attack.ActivateSkill(enemyAttack,attackPos);
                if (b + (attackSpots.Length / 2) >= attackSpots.Length) {
                    b -= attackSpots.Length;
                }
                attackPos = attackSpots[b + (attackSpots.Length / 2)].position;

                attack.ActivateSkill(enemyAttack,attackPos);
                yield return new WaitForSeconds(0.07f);
            }


        }
        AfterPattern();
        yield return new WaitForSeconds(chillOutTime);
        isStunned = false;
        chillOut = false;



    }
    IEnumerator BossAttackPatternFrost() {
        yield return new WaitForSeconds(0.8f);
        bool minus = false;
        int safeSpot1 = 10;
        int safeSpot2 = 11;
        int safeSpot3 = 12;

        for (int g = 0; g < 40; g++) {
            
            if (!minus) {
                for (int i = 0; i < frostAttackSpots.Length; i++) {
                    if (i != safeSpot1 && i != safeSpot2 && i != safeSpot3) {
                        attackPos = frostHitSpots[i].position;
                        attack.ActivateSkill(enemyAttack,attackPos,frostAttackSpots[i]);
                    }
                }
                safeSpot1++;
                safeSpot2++;
                safeSpot3++;
                if (safeSpot3 == frostAttackSpots.Length-1) {
                    minus = true;
                }
                
            } else {
                for (int i = frostAttackSpots.Length-1; i >= 0 ; i--) {
                    if (i != safeSpot1 && i != safeSpot2 && i != safeSpot3) {
                        attackPos = frostHitSpots[i].position;
                        attack.ActivateSkill(enemyAttack,attackPos,frostAttackSpots[i]);
                    }
                }
                safeSpot1--;
                safeSpot2--;
                safeSpot3--;
                if (safeSpot1 == 0) {
                    minus = false;
                }               
            }
            yield return new WaitForSeconds(0.5f);


        }
        AfterPattern();
        yield return new WaitForSeconds(chillOutTime);
        isStunned = false;
        chillOut = false;
    }



    void BeforePattern() {
        anim.SetBool("EnemyIsMoving",true);
        anim.SetFloat("EnemyFaceX",0);
        anim.SetFloat("EnemyFaceY",-1);
        anim.SetBool("EnemyIsMoving",false);
        shadow.SetActive(true);
        bossHealthProcent.text = Mathf.RoundToInt((currentHealth / maxHealth) * 100) + "%";
        chillOut = true;
        renderer.material.color = Color.red;
        transform.position = startPos;
        GetComponent<Collider2D>().enabled = false;
        enemyType = Type.Ranged;
        SetTypeNumbers();
        ResetBurn();
    }

    void AfterPattern() {
        isStunned = true;
        StatusEffect(3);
        shadow.SetActive(false);
        attack.specc++;
        renderer.material.color = Color.white;
        GetComponent<Collider2D>().enabled = true;
        transform.position = startPos - new Vector3(0,2);
        anim.SetBool("EnemyIsMoving",false);        
    }

    public override void Die() {

        bossHealthProcent.text = "Dead";
        GameManager.Instance.musicPlayer.Stop();
        GameManager.Instance.musicPlayer.volume = 0.4f;
        GameManager.Instance.musicPlayer.PlayOneShot(GameManager.Instance.musicTracks[8]);
        GameManager.Instance.savedTrack = -1;

        base.Die();


    }

}


