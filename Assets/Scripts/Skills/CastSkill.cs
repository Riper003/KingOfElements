using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastSkill : MonoBehaviour {

    public Rigidbody2D[] projectiles;
    public Rigidbody2D[] melees;
    public Rigidbody2D[] dashes;
    public Rigidbody2D[] explosions;

    public int specc;
    private float activeTime;
    private float projectileSpeed;
    private float cooldown;
    private Vector3 mousePos;
    private Vector3 playerPos;
    public float globalCooldown = 0.1f;

    public Image meleeIcon;
    public Image rangedIcon;
    public Image dashIcon;

    public Vector3 targetPos;

    public LayerMask blinkMask;
    Vector3 dashDirection;
    public float blinkDistance;

    private float meleeTimer;
    [HideInInspector]
    public float rangedTimer;
    private float dashTimer;

    private float meleeCD;
    [HideInInspector]
    public float rangedCD;

    public float normalDashCD;
    public float frostDashCD;
    public float fireDashCD;
    public float lightningDashCD;
    public float lightningDashDuration;
    public float lightningSpeedMultiplier;
    private float dashCD;
    public AudioClip dashSound;
    public AudioClip frostSound;
    public AudioClip lightningSound;    
    public bool lightningDashing;
    private Vector3 oldBlinkDirection;


    public float dashActiveTime;
    public float dashSpeed;

    public float distanceFromPlayer;

    public bool isAttacking;

    private float originalMoveSpeed;
    private bool normalActivateSkill = false;

    [HideInInspector]
    public Vector3[] multiShot;
    public int shotsAmount;
    public GameObject multiShotObject;


    public Vector2 aiming;
    private Vector2 aimDirection;
    private Animator anim;

    void Start() {
        if (tag == "Player") {
            UpdatePlayerSpecc();
        }

        anim = GetComponent<Animator>();
        SetGlobalCooldown();
    }


    void Update() {

        if (gameObject.tag == "Player") {
            MeleeCD();
            RangedCD();
            DashCD();
        }
        if (!GetComponent<Character>().isStunned && !GetComponent<Character>().isFrozen) {


            if (Input.GetKey(KeyCode.Mouse0) && meleeTimer == meleeCD && (gameObject.tag == "Player") && isAttacking == false) {

                ActivateSkill(melees);
                meleeTimer = 0;
                isAttacking = true;

            }

            if (Input.GetKey(KeyCode.Mouse1) && rangedTimer == rangedCD && (gameObject.tag == "Player") && isAttacking == false) {
                if (GameManager.Instance.ultiUpgrade != 1 && specc == 1) {

                    if (GameManager.Instance.ultiUpgrade == 2) {
                        shotsAmount = 3;
                    }
                    if (GameManager.Instance.ultiUpgrade == 3) {
                        shotsAmount = 5;
                    }

                    multiShot = new Vector3[shotsAmount];
                    PlayerMultiShot();
                    for (int i = 0; i < multiShot.Length; i++) {
                        ActivateSkill(projectiles, multiShot[i]);

                    }
                }
                else {
                    ActivateSkill(projectiles);
                }
                rangedTimer = 0;
                isAttacking = true;

            }

            if (Input.GetKey(KeyCode.Space) && dashTimer == dashCD && (gameObject.tag == "Player" && isAttacking == false)) {

                ActivateDash();
                dashTimer = 0;
                isAttacking = true;
            }
        }
    }

    void PlayerMultiShot() {
        GetPos();
        GameObject clone;
        clone = Instantiate(multiShotObject, transform);
        clone.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((targetPos.y - transform.position.y), (targetPos.x - transform.position.x)) * Mathf.Rad2Deg);
        for (int i = 0; i < multiShot.Length; i++) {
            multiShot[i] = clone.transform.GetChild(i).position;

        }
        Destroy(clone);

    }

    void WhileAttacking(float s) {
        originalMoveSpeed = GetComponent<Character>().moveSpeed;

        if (!lightningDashing) {
            GetComponent<Character>().moveSpeed *= s;
        }
        StartCoroutine(GlobalCooldown());

    }

    public void UpdatePlayerSpecc() {
        specc = GameManager.Instance.specc;
    }

    IEnumerator GlobalCooldown() {

        yield return new WaitForSeconds(globalCooldown);
        isAttacking = false;
        if (!lightningDashing) {
            if (GetComponent<Character>().isSlowed) {
                GetComponent<Character>().moveSpeed = originalMoveSpeed;
            }
            else {

                GetComponent<Character>().moveSpeed = GetComponent<Character>().originalSpeed;
            }
        }


    }

    //två likadana metoder ffs
    //delet this
    void MeleeCD() {
        if (meleeTimer < meleeCD) {
            meleeTimer += Time.deltaTime;
            if (gameObject.tag == "Player")
                meleeIcon.fillAmount = meleeTimer / meleeCD;
        }

        if (meleeTimer > meleeCD) {
            meleeTimer = meleeCD;
        }
    }

    void RangedCD() {
        if (rangedTimer < rangedCD) {
            rangedTimer += Time.deltaTime;
            if (gameObject.tag == "Player")
                rangedIcon.fillAmount = rangedTimer / rangedCD;
        }

        if (rangedTimer > rangedCD) {
            rangedTimer = rangedCD;
        }
    }

    void DashCD() {

        if (dashTimer < dashCD) {
            dashTimer += Time.deltaTime;
            if (gameObject.tag == "Player")
                dashIcon.fillAmount = dashTimer / dashCD;
        }

        if (dashTimer > dashCD) {
            dashTimer = dashCD;
        }
    }

    public void GetPos() {
        if (this.tag == "Enemy") {
            targetPos = GetComponent<Enemy>().target.position;
        }
        else if (this.tag == "Player") {
            targetPos = GetComponent<Player>().mousePos;
        }
    }
    public void ActivateSkill(Rigidbody2D[] type) {
        normalActivateSkill = true;
        ActivateSkill(type, new Vector3(1, 1, 1));
    }

    public void ActivateSkill(Rigidbody2D[] type, Vector3 attackPos) {

        ActivateSkill(type, attackPos, transform);
    }

    public void ActivateSkill(Rigidbody2D[] type, Vector3 attackPos, Transform spawnTransform) {

        Skill skill = type[specc].GetComponent<Skill>();


        Rigidbody2D clone;

        GetPos();
        if (!normalActivateSkill) {
            targetPos = attackPos;
        }
        normalActivateSkill = false;
        activeTime = skill.activeTime;
        projectileSpeed = skill.projectileSpeed;
        cooldown = skill.cooldown;
        distanceFromPlayer = skill.distanceFromPlayer;
        WhileAttacking(skill.slowWhileAttacking);


        // TODO skillIcon = type[specc].GetComponent<Skill>().skillIcon;

        if (skill.skillType == Skill.TypeOfSkill.Dash && GameManager.Instance.dashToMouse == false) {
            clone = Instantiate(type[specc], (spawnTransform.position + dashDirection * distanceFromPlayer), spawnTransform.rotation) as Rigidbody2D;
        }
        else {
            clone = Instantiate(type[specc], (spawnTransform.position + (targetPos - spawnTransform.position).normalized * distanceFromPlayer), spawnTransform.rotation) as Rigidbody2D;

        }

        if (skill.skillType == Skill.TypeOfSkill.Melee) {
            clone.transform.parent = gameObject.transform;
            meleeCD = cooldown;
        }
        else if (skill.skillType == Skill.TypeOfSkill.Ranged) {
            rangedCD = cooldown;
        }



        if (this.tag == "Enemy") {
            clone.tag = "EnemySkill";
        }
        else if (this.tag == "Player") {
            clone.tag = "PlayerSkill";
        }


        //clone.transform.position = new Vector3(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized * 2;

        if (skill.skillType == Skill.TypeOfSkill.Explosion && specc == 2) {

        }
        else if (skill.skillType == Skill.TypeOfSkill.Explosion && GameManager.Instance.dashToMouse == false) {
            clone.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((dashDirection.y), (dashDirection.x)) * Mathf.Rad2Deg);
        }
        else {
            clone.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((targetPos.y - spawnTransform.position.y), (targetPos.x - spawnTransform.position.x)) * Mathf.Rad2Deg);
        }

        //clone.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((targetPos.y - transform.position.y), (targetPos.x - transform.position.x)) * Mathf.Rad2Deg);

        if (skill.skillType == Skill.TypeOfSkill.Dash && GameManager.Instance.dashToMouse == false) {
            clone.velocity = spawnTransform.TransformDirection(dashDirection * projectileSpeed);
        }
        else {
            clone.velocity = spawnTransform.TransformDirection((targetPos - spawnTransform.position).normalized * projectileSpeed);
        }



        if (GameManager.Instance.dashToMouse == false && tag == "Player" && (skill.skillType == Skill.TypeOfSkill.Dash || skill.skillType == Skill.TypeOfSkill.Explosion)) {
            aiming = dashDirection;
        }
        else {
            aiming = (targetPos - spawnTransform.position).normalized;
        }


        if (aiming.x > 0.5f || aiming.x < -0.5f) {
            aimDirection = new Vector2(aiming.x, 0f);
        }

        if (aiming.y > 0.5f || aiming.y < -0.5f) {
            aimDirection = new Vector2(0f, aiming.y);
        }

        if (anim != null && tag == "Player") {
            anim.SetFloat("FaceX", aimDirection.x);
            anim.SetFloat("FaceY", aimDirection.y);
            //anim.SetBool("IsAttacking", isAttacking);
        }

        if (skill.skillType == Skill.TypeOfSkill.Dash) {
            gameObject.transform.SetParent(clone.transform);
            StartCoroutine(UnParent(clone));
        } else {
            Destroy(clone.gameObject, activeTime);
        }


        

    }

    IEnumerator UnParent(Rigidbody2D clone) {
        yield return new WaitForSeconds(activeTime - 0.05f);
        GetComponent<Collider2D>().enabled = true;
        gameObject.transform.SetParent(null);
        if (clone != null) {
            Destroy(clone.gameObject);
        }        
    }

    public void ActivateDash() {
        GetPos();
        //dashCD = 2;
        GetComponent<Character>().ResetMovement();
        switch (specc) {
            case 0:
                Dash();
                if (dashSound != null)
                    GameManager.Instance.OtherSounds(dashSound, 0.2f);
                WhileAttacking(0);
                dashCD = normalDashCD;
                break;
            case 1:
                Dash();
                WhileAttacking(0);
                StartCoroutine(FireDash());
                dashCD = fireDashCD - (GameManager.Instance.dashUpgrade - 1);
                break;
            case 2:
                FrostDash();
                if (frostSound != null)
                    GameManager.Instance.OtherSounds(frostSound, 0.2f);
                WhileAttacking(0);
                dashCD = frostDashCD - (GameManager.Instance.dashUpgrade - 1);
                break;
            case 3:
                StartCoroutine(LightningDash());
                if (lightningSound != null)
                    GameManager.Instance.OtherSounds(lightningSound, 0.2f);
                WhileAttacking(1);
                dashCD = lightningDashCD;
                break;
        }



    }

    void Dash() {
        Player player = GetComponent<Player>();

        if (GameManager.Instance.dashToMouse)
            dashDirection = (targetPos - transform.position).normalized;
        else {
            if (player.input.x == 0 && player.input.y == 0) {
                player.input = player.oldInput;
            }
            dashDirection = player.input;
        }


        Vector3 dest = transform.position + dashDirection * 1.5f;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, dest, blinkMask);
        if (!hit) {
            ActivateSkill(dashes);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    IEnumerator LightningDash() {
        GetComponent<Character>().Unfreeze();
        lightningDashing = true;
        GetComponent<Character>().renderer.material.color = new Color(1, 0, 1);
        GetComponent<Character>().moveSpeed *= lightningSpeedMultiplier;
        GetComponent<PlayerController>().collisionMask = (1 << LayerMask.NameToLayer("Obstacle"));
        yield return new WaitForSeconds(lightningDashDuration + (GameManager.Instance.dashUpgrade - 1));
        GetComponent<Character>().moveSpeed = GetComponent<Character>().originalSpeed;
        GetComponent<PlayerController>().collisionMask = (1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("Enemy"));
        lightningDashing = false;
        GetComponent<Character>().renderer.material.color = Color.white;
    }



    public IEnumerator FireDash() {
        for (int i = 0; i < 4; i++) {
            ActivateSkill(explosions);
            yield return new WaitForSeconds(0.03f);
        }
    }


    void FrostDash() {
        GetPos();
        Player player = GetComponent<Player>();
        if (GameManager.Instance.dashToMouse) {
            dashDirection = (targetPos - transform.position).normalized;

        }
        else {
            if (player.input.x == 0 && player.input.y == 0) {
                player.input = player.oldInput;
            }
            dashDirection = player.input;
        }

        Vector3 dest = transform.position + dashDirection * blinkDistance;

        float hereToThere = Vector2.Distance(transform.position, dest);
        float hereToMouse = Vector2.Distance(transform.position, targetPos);

        RaycastHit2D hit = Physics2D.Linecast(transform.position, dest, blinkMask);

        if (hereToThere > hereToMouse && GameManager.Instance.dashToMouse) {

            dest = transform.position + dashDirection * hereToMouse;
            hit = Physics2D.Linecast(transform.position, dest, blinkMask);
        }

        if (hit) {
            print(hit.collider.tag);
            dest = transform.position + dashDirection * (hit.distance - 1.5f);
        }
        Debug.DrawLine(transform.position, dest, Color.red);

        transform.position = dest;


        ActivateSkill(explosions);



    }

    /*IEnumerator DashActiveTime() {
        yield return new WaitForSeconds(dashActiveTime);
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0,0,0);
    }*/

    public void SetGlobalCooldown() {
        switch (specc) {
            case 0:
                globalCooldown = 0.2f;
                break;
            case 1:
                globalCooldown = 0.2f;
                break;
            case 2:
                globalCooldown = 0.3f;
                break;
            case 3:
                globalCooldown = 0.1f;
                break;
        }
    }

}
