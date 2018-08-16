using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character :MonoBehaviour {

    [HideInInspector]
    public PlayerController controller;

    public LayerMask groundLayer;
    [HideInInspector]
    public Vector2 velocity;
    [HideInInspector]
    public float accelerationTimeGround = .05f;
    [HideInInspector]
    public float velocityXSmoothing;
    [HideInInspector]
    public float velocityYSmoothing;
    [HideInInspector]
    public Material freezeMat;


    private Material originalMat;

    public float maxHealth = 100;
    public float currentHealth;
    public float moveSpeed = 5;
    [HideInInspector]
    public float originalSpeed;
    private float oldSpeed;
    [HideInInspector]
    public bool isDead = false;

    public float stunDuration;
    [HideInInspector]
    public bool isStunned;
    [HideInInspector]
    public bool isFrozen;
    [HideInInspector]
    public bool isSlowed = false;
    [HideInInspector]
    public int slowCounter = 0;
    [HideInInspector]
    public int freezeCounter = 0;

    public float freezeDuration;

    public int damageMultiplier;


    [HideInInspector]
    public bool standingInFire;
    [HideInInspector]
    public int burnCounter;
    [HideInInspector]
    public bool isBurning;

    [HideInInspector]
    public Color oldColor;
    [HideInInspector]
    public Renderer renderer;

    public Image currentHealthBar;

    public float upgradeMultiplier;

    private SpriteRenderer myRenderer;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;

    public bool hasJul;

    public Vector3 startPos;

    [HideInInspector]
    public Vector3[] multiShot;
    public int shotsAmount;
    public GameObject multiShotObject;
    GameObject statusEffect;
    [HideInInspector]
    public GameObject[] statusEffects;


    private List<Coroutine> burnCoroutines = new List<Coroutine>();
    private List<Coroutine> freezeCoroutines = new List<Coroutine>();

    // Use this for initialization
    public virtual void Start() {
        statusEffects = new GameObject[5];
        startPos = transform.position;
        standingInFire = false;
        multiShot = new Vector3[shotsAmount];

        currentHealth = maxHealth;
        renderer = gameObject.GetComponent<Renderer>();
        originalSpeed = moveSpeed;
       
       
        controller = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    public virtual void Update() {
        if (!isSlowed && statusEffects[0] != null) {
            Destroy(statusEffects[0]);
        }
            
        if (!isBurning && statusEffects[1] != null) {
            Destroy(statusEffects[1]);
        }           

        if (!isFrozen && statusEffects[2] != null) {
            Destroy(statusEffects[2]);
        }
            
        if (!isStunned && statusEffects[3] != null) {
            Destroy(statusEffects[3]);
        }

        if(GameManager.Instance.activatedJul == true && !hasJul) {
            StatusEffect(4);
            hasJul = true;
        }    

        if(GameManager.Instance.activatedJul == false && statusEffects[4] != null) {
            Destroy(statusEffects[4]);
            hasJul = false;
        }
        


    }



    public virtual void TakeDamage(float dmg) {

        if (gameObject.tag == "Enemy")
            dmg *= GameManager.Instance.damageUpgrade;

        if (!isDead) {
            if (isFrozen) {
                /*if (GameManager.Instance.shatterSound != null)
                    GameManager.Instance.OtherSounds(GameManager.Instance.shatterSound,0.2f);*/
                dmg *= damageMultiplier;
            }

            currentHealth -= dmg;

            if (currentHealth <= 0) {
                isDead = true;
                Unfreeze();
                Die();

            }
            UpdateCurrentHealthBar();
        }
    }

    public void RestorHealth(int healing) {
        currentHealth += healing;
        if (currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        }
        UpdateCurrentHealthBar();
    }

    public void SlowMoveSpeed(float slowAmount,float slowDuration) {
        if (!GetComponent<CastSkill>().lightningDashing) {
            moveSpeed *= slowAmount;
            Coroutine c = StartCoroutine(SlowTimer(slowDuration));
            freezeCoroutines.Add(c);
            isSlowed = true;
            StatusEffect(0);
            slowCounter += 1;
            freezeCounter += 1;
            if (freezeCounter > 2) {
                Freeze();
            }
        }
    }


    IEnumerator SlowTimer(float slowDuration) {

        yield return new WaitForSeconds(slowDuration);
        slowCounter -= 1;
        if (slowCounter <= 0) {
            ResetMovement();
        }
    }

    public void Freeze() {
        isFrozen = true;
        isSlowed = false;
        StatusEffect(2);
        
        StopAnim();
        Destroy(statusEffects[0]);
        gameObject.GetComponent<Character>().enabled = false;
        Coroutine c = StartCoroutine(FreezeTimer());
        freezeCoroutines.Add(c);
    }

    public void StopAnim() {
        if (tag == "Enemy")
            GetComponent<Enemy>().anim.SetBool("EnemyIsMoving",false);
        else {
            GetComponent<PlayerController>().anim.SetBool("PlayerIsMoving",false);
        }
    }

    public IEnumerator Stun() {
        isStunned = true;
        StatusEffect(3);
        

        StopAnim();
        gameObject.GetComponent<Character>().enabled = false;
		if (tag == "Enemy") {
			yield return new WaitForSeconds (stunDuration + GameManager.Instance.ultiUpgrade - 1);
                   

        } else {
			yield return new WaitForSeconds(stunDuration);
		}
        gameObject.GetComponent<Character>().enabled = true;
        
        isStunned = false;

    }

    IEnumerator FreezeTimer() {
		if (gameObject.tag == "Enemy") {
			yield return new WaitForSeconds (freezeDuration + GameManager.Instance.ultiUpgrade-1);
		} else {
			yield return new WaitForSeconds(freezeDuration);
		}
        Unfreeze();
    }

    public virtual void Unfreeze() {
        gameObject.GetComponent<Character>().enabled = true;
        //StopAllCoroutines();

        foreach (Coroutine c in freezeCoroutines) {
            StopCoroutine(c);
        }


        isFrozen = false;
        ResetMovement();

    }

    public virtual void UpdateCurrentHealthBar() {
        currentHealthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth,1,1);
    }

    public void ResetMovement() {

        isSlowed = false;
        slowCounter = 0;
        freezeCounter = 0;
        //moveSpeed = oldSpeed;
        moveSpeed = originalSpeed;
        
    }

    public void Burn(int dmg, float tick, int amounts) {
        Coroutine c = StartCoroutine(DamageOverTime(dmg, tick, amounts));
        burnCoroutines.Add(c);
    }

    public void TrailBurn(int dmg, float tick, int amounts) {
        Coroutine c = StartCoroutine(FireTrail(dmg, tick, amounts));
        burnCoroutines.Add(c);
    }

    public IEnumerator DamageOverTime(int dmg,float tick,int amounts) {
        burnCounter++;
        isBurning = true;
        StatusEffect(1);
        

        for (int i = 0; i < amounts; i++) {
            TakeDamage(dmg);
            yield return new WaitForSeconds(tick);
        }
        burnCounter--;
        if (burnCounter <= 0) {
            ResetBurn();
            
        }
    }

    public IEnumerator FireTrail(int dmg,float tick,int amounts) {
        burnCounter++;
        isBurning = true;
        StatusEffect(1);
        
        for (int i = 0; i < amounts; i++) {
            TakeDamage(dmg);
            yield return new WaitForSeconds(tick);
        }
        standingInFire = false;
        burnCounter--;
        if (burnCounter <= 0) {
            ResetBurn();
            
        }
    }

    public void ResetBurn() {
        foreach (Coroutine c in burnCoroutines) {
            StopCoroutine(c);
        }
        burnCounter = 0;
        isBurning = false;
    }


    public virtual void Die() {
        //överskugga i enemy och player för animation och vad som händer
        print("Här ska dom dö :P");
    }




    public void StatusEffect(int type) {        
        switch (type) {
            case 0:
                statusEffect = GameManager.instance.slowEffect;
                break;
            case 1:
                statusEffect = GameManager.instance.burnEffect;
                break;
            case 2:
                statusEffect = GameManager.instance.freezeEffect;
                break;
            case 3:
                statusEffect = GameManager.instance.stunEffect;
                break;
            case 4:
                statusEffect = GameManager.instance.julEffect;
                break;
        }
        GameObject clone;
        if (statusEffects[type] == null) {
            clone = Instantiate(statusEffect, transform.position, transform.rotation) as GameObject;
            clone.transform.localScale = transform.localScale;
            clone.transform.parent = gameObject.transform;
            statusEffects[type] = clone;
        }               
    }

}
