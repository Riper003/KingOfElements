using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningField :MonoBehaviour {

    public float enemyRestruckDelay = 0.3f;
    public GameObject triggerCircle;
    public GameObject lightningEffect;
    [HideInInspector]
    public int currentBounce;
    [HideInInspector]
    public int maxBounceCount;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float bounceMaxLength;

    Character opponent;
    private bool hasHit;



    private void Start() {

        hasHit = false;
        Destroy(gameObject,0.5f);
    }

    private void Update() {
        if (transform.localScale.x < bounceMaxLength) {
            transform.localScale += new Vector3(10 * Time.deltaTime,10 * Time.deltaTime,0);
        } else {
            DestroyThisObject();
        }



    }

    void OnTriggerEnter2D(Collider2D other) {


        if (gameObject.tag == "PlayerSkill")
            opponent = other.gameObject.GetComponent<Enemy>();
        if (gameObject.tag == "EnemySkill")
            opponent = other.gameObject.GetComponent<Player>();


        if (opponent == null) {  // It is not enemy

            return;
        }
        
            Hit hit = other.gameObject.GetComponent<Hit>();
        
            if (hit == null && currentBounce < maxBounceCount) { // the enemy is yet to be hit
            if (!hasHit) {
                hasHit = true;
                print("hit a target that has not already been hit");
                currentBounce++;
                print(currentBounce);
                if (currentBounce >= 2) {
                    lightningEffect = Instantiate(lightningEffect,other.gameObject.transform.position,Quaternion.identity) as GameObject;
                    lightningEffect.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((other.transform.position.y - transform.position.y),(other.transform.position.x - transform.position.x)) * Mathf.Rad2Deg);
                    //lightningEffect.transform.localScale = new Vector3(1* ((other.transform.position - transform.position).magnitude-5),1,1);
                    Destroy(lightningEffect,0.2f);
                }


                //Call lightning effect here
                opponent.TakeDamage(damage);
                opponent.StartCoroutine(opponent.Stun());
                //Create another copy of this lightning field, by doing this, it will start chaining when the condition is right
                triggerCircle = Instantiate(triggerCircle,other.gameObject.transform.position,Quaternion.identity) as GameObject;


                hit = other.gameObject.AddComponent<Hit>();
                hit.killDelay = enemyRestruckDelay;

                DestroyThisObject();
            }
        }
    }


    public void SetCurrentBounceToZero() {
        currentBounce = 0;
    }

    //Call this using an animation event, just in case the sphere strike nothing at all
    void DestroyThisObject() {

        Destroy(gameObject);
    }
}

