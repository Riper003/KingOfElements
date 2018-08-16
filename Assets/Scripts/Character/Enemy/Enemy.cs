
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy :Character {

    private Rigidbody2D rigidbody;
    public Rigidbody2D[] enemyAttack;
    [HideInInspector]
    public Transform target;

    public GameObject[] itemDrops;

    [HideInInspector]
    public CastSkill attack;

    //States
    public bool isAggro = false;

    [HideInInspector]
    public bool isAttacking = false;
    [HideInInspector]
    public bool attackReady = true;


    public float aggroRange;
    public float chaseRange;
    private float originalChaseRange;

    public float meleeAttackRange = 1;
    public float rangedAttackRange = 5;
    public float explodingAttackRange = 1;
    public float meleeCastTime = 0.4f;
    public float rangedCastTime = 1;
    public float explodingCastTime = 0.4f;
    public float meleeCooldown = 1;
    public float rangedCooldown = 2;
    public float explodingDieTime = 0.2f;

    private float castTime;
    [HideInInspector]
    public float attackRange;
    public float cooldown;


    public int chanceToDropHeal;

    private bool respawning;

    public float reactionTime = 0.4f;

    [HideInInspector]
    public bool attackFeedback;

    public LayerMask hitLayers;
    public LayerMask enemyMask;

    [HideInInspector]
    public bool hitByLightning;
    private float hitAgainDelay;

    //Animator shit
    [HideInInspector]
    public Animator anim;
    private Vector2 faceDirection;
    private bool isMoving;
    private Vector2 direction;

    //Collision


    [HideInInspector]
    public int goCrazyAmount;
    

    public bool chillOut;

    public float chillOutTime;

    [HideInInspector]
    public Vector3 attackPos;

    
    public Type enemyType;

    public bool isBoss;

   
    public GameObject feedBackObject;


    public enum Type {
        Melee, Ranged, Exploding
    }

    // Use this for initialization
    public override void Start() {
        base.Start();

        currentHealthBar.color = Color.red;

        gameObject.GetComponentInChildren<CircleCollider2D>().radius = aggroRange;
        attack = GetComponent<CastSkill>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalChaseRange = chaseRange;
        SetTypeNumbers();


    }



    // Update is called once per frame
    public override void Update() {
        base.Update();
            
        if (target != null) {
            OnPlayerRespawn();
            if (!isAggro) {
                if (RaycastToPlayer()) {
                    isAggro = true;

                    if (GetComponentInParent<AggroHandler>() != null)
                        AggroOther();
                }
            }
        }


        if (attackFeedback) {
            StartCoroutine(FeedbackAttack());
        }

    }

    private void FixedUpdate() {
        if (isAggro) {
            if (chillOut) {
                return;
            }

            if (!Within(attackRange) && !isAttacking) {
                Chase();
            } else {

                if (attackReady) {
                    StartCoroutine(Attacking(castTime));
                    attackReady = false;
                }
            }


        }
    }

    public void SetTypeNumbers() {
        switch (enemyType) {
            case Type.Melee:
                attackRange = meleeAttackRange;
                castTime = meleeCastTime;
                enemyAttack = attack.melees;
                cooldown = meleeCooldown;
                break;
            case Type.Ranged:
                attackRange = rangedAttackRange;
                castTime = rangedCastTime;
                enemyAttack = attack.projectiles;
                cooldown = rangedCooldown;
                break;
            case Type.Exploding:
                attackRange = explodingAttackRange;
                castTime = explodingCastTime;
                enemyAttack = attack.explosions;
                break;
        }
    }

    public void StartAggro() {
        if (!isDead) {

            if (!isAggro) {
                target = GameObject.FindGameObjectWithTag("Player").transform;


            }
        }
    }

    void AggroOther() {
        GetComponentInParent<AggroHandler>().ActivateAggro(2);
    }

    public void StopAggro() {
        if (!isDead) {
            isMoving = false;
            //target = null;
            chaseRange -= chaseRange;
            isAggro = false;
            attackReady = false;
            anim.SetBool("EnemyIsMoving",false);
        }
    }

    void Chase() {
        if (Within(chaseRange)) {

            isMoving = true;
            SetAnim();

            transform.position = Vector2.MoveTowards(transform.position,target.position,moveSpeed * Time.deltaTime);

        } else {
            StopAggro();
        }
    }

    public void SetAnim() {
        Vector2 heading = target.position - transform.position;
        float distance = heading.magnitude;
        direction = heading / distance; // This is now the normalized direction.


        if (direction.x > 0.5f || direction.x < -0.5f) {
            //isMoving = true;
            faceDirection = new Vector2(direction.x,0f);
        }

        if (direction.y > 0.5f || direction.y < -0.5f) {
            //isMoving = true;
            faceDirection = new Vector2(0f,direction.y);
        }
        if (anim != null) {
            anim.SetFloat("EnemyMoveY",direction.y);
            anim.SetFloat("EnemyMoveX",direction.x);
            anim.SetFloat("EnemyFaceX",faceDirection.x);
            anim.SetFloat("EnemyFaceY",faceDirection.y);
            
            anim.SetBool("EnemyIsMoving",isMoving);

        }


    }

    public bool RaycastToPlayer() {
        Vector3 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin,target.position - origin,aggroRange,hitLayers);


        if (hit) {
            if (hit.collider.tag == "Player" && Within(chaseRange)) {
                return true;
            }
        }
        return false;
    }

    public bool Within(float distance) {
        return Vector2.Distance(target.transform.position,transform.position) < distance;
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);
        if (!isAggro) {
            if (GetComponentInParent<AggroHandler>() != null) {
                AggroOther();
            } else {
                StartAggro();
            }
            isAggro = true;
        }

    }


    IEnumerator Cooldown(float cd) {
        yield return new WaitForSeconds(cd);
        attackReady = true;


    }

    IEnumerator FeedbackAttack() {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(reactionTime);
        if (!isStunned && !chillOut)
            renderer.material.color = Color.white;

    }

    IEnumerator Attacking(float sec) {
        bool multiShotActive = false;
        isAttacking = true;
        isMoving = false;

        yield return new WaitForSeconds(sec - reactionTime);

        attackFeedback = true;
        SetAnim();
        attackPos = target.position;

        if (multiShotObject != null && enemyType == Type.Ranged) {
            MultiShot();
            multiShotActive = true;

        }
        if (enemyType == Type.Ranged && feedBackObject != null && GetComponent<CastSkill>().specc == 3) {
            
            if (multiShotActive) {
                for (int i = 0; i < multiShot.Length; i++) {
                    FeedBackInstantiate(multiShot[i]);
                }
            }else {
                FeedBackInstantiate(attackPos);
            }
        }


        yield return new WaitForSeconds(reactionTime);

        if (!isFrozen && !isStunned && !chillOut) {
            if (multiShotActive) {
                for (int i = 0; i < multiShot.Length; i++) {
                    attack.ActivateSkill(enemyAttack,multiShot[i]);
                    
                }
            } else {
                attack.ActivateSkill(enemyAttack,attackPos);
            }
            if (enemyType == Type.Exploding) {

                yield return new WaitForSeconds(explodingDieTime);

                Destroy(gameObject);
            }

            StartCoroutine(Cooldown(cooldown));

        } else {
            attackReady = true;
        }
        
        if (isBoss && !chillOut) {
            if (enemyType == Type.Melee) {
                enemyType = Type.Ranged;
            } else {
                enemyType = Type.Melee;
            }
            SetTypeNumbers();
        }
        
        attackFeedback = false;
        isAttacking = false;
    }



    public override void Die() {
        //överskugga i enemy och player för animation och vad som händer
        Drop();
        Destroy(gameObject);
    }

    void FeedBackInstantiate(Vector3 attackPos) {
        GameObject clone;
        clone = Instantiate(feedBackObject,transform);
        clone.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((attackPos.y - transform.position.y),(attackPos.x - transform.position.x)) * Mathf.Rad2Deg);    

        Destroy(clone,reactionTime);
    }
    void MultiShot() {
        GameObject clone;
        clone = Instantiate(multiShotObject,transform);
        clone.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((attackPos.y - transform.position.y),(attackPos.x - transform.position.x)) * Mathf.Rad2Deg);
        for (int i = 0; i < multiShot.Length; i++) {
            multiShot[i] = clone.transform.GetChild(i).position;
            
        }
        Destroy(clone,reactionTime);
    }

    void Drop() {

        GameObject clone;
        Vector3 charPos = transform.position;
        if (Random.Range(1,100) < chanceToDropHeal) {
            clone = Instantiate(itemDrops[0],new Vector3(charPos.x,charPos.y - 1,0),transform.rotation,null);
        }

        clone = Instantiate(itemDrops[1],charPos,transform.rotation,null);


    }

    void OnPlayerRespawn() {
        if (target.gameObject.GetComponent<Player>().isDead) {
            StopAggro();
            if (!respawning) {
                StartCoroutine(ReturnTimer());
                respawning = true;
            }

        }
    }

    IEnumerator ReturnTimer() {

        yield return new WaitForSeconds(2);
        transform.position = startPos;
        attackReady = true;
        respawning = false;
        chaseRange = originalChaseRange;



    }

    public override void UpdateCurrentHealthBar() {
        //currentHealthBar.rectTransform.localScale = new Vector3 (currentHealth / maxHealth, 1, 1);
        base.UpdateCurrentHealthBar();
    }



}
