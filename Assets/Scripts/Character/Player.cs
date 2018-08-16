using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player :Character {
    [HideInInspector]
    public Vector3 mousePos;
    [HideInInspector]
    public Camera cam;
    [HideInInspector]
    public Text feedbackText;
    [HideInInspector]
    public float rayLength;
    [HideInInspector]
    public Vector2 oldInput;
    private Rigidbody2D rigidbody;
    public Vector2 input;

    public LayerMask collisionMask;
    public GameObject feedbackCanvas;
    public float originalMaxHealth;

    public Text currencyText;
    public int currency;

    public Text healthBarText;



    // Use this for initialization
    public override void Start() {
        rigidbody = GetComponentInChildren<Rigidbody2D>();
        cam = Camera.main;
        feedbackText = feedbackCanvas.GetComponentInChildren<Text>();
        feedbackCanvas.SetActive(false);

        UpdateHealth();
        base.Start();
        UpdateCurrentHealthBar();
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();

        Movement();
        MousePos();

     

        //currentHealthBar.rectTransform.localScale = new Vector3 (currentHealth / maxHealth, 1, 1);
    }

    public void UpdateHealth() {
        if (GameManager.Instance.healthUpgrade != 0)
            maxHealth = Mathf.Round(originalMaxHealth * GameManager.Instance.healthUpgrade);
    }

    public void GetCurrency(int pickUp) {
        currency += pickUp;

        currencyText.text = currency.ToString();
    }

    void Movement() {

        input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;
        if (input.x != 0 || input.y != 0) {
            oldInput = input;
        }
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x,targetVelocityX,ref velocityXSmoothing,accelerationTimeGround);

        float targetVelocityY = input.y * moveSpeed;
        velocity.y = Mathf.SmoothDamp(velocity.y,targetVelocityY,ref velocityYSmoothing,accelerationTimeGround);

        controller.PlayerMove(velocity * Time.deltaTime,input);

    }

    public void Dash(float dashSpeed) {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")).normalized;

        if (input.x == 0 && input.y == 0) {
            input = oldInput;
        }


        float targetVelocityX = input.x * dashSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x,targetVelocityX,ref velocityXSmoothing,accelerationTimeGround);

        float targetVelocityY = input.y * dashSpeed;
        velocity.y = Mathf.SmoothDamp(velocity.y,targetVelocityY,ref velocityYSmoothing,accelerationTimeGround);

        controller.PlayerMove(velocity * Time.deltaTime,input);




        /*mousePos = cam.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        float targetVelocityX = (mousePos.x - transform.position.x) * dashSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGround);

        float targetVelocityY = (mousePos.y - transform.position.y) * dashSpeed;
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeGround);

        controller.PlayerMove(velocity * Time.deltaTime, mousePos);*/

    }

    void MousePos() {
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.z - cam.transform.position.z));
    }

    void RayToMousePos() {


        Vector3 rayOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin,mousePos - rayOrigin,rayLength,collisionMask);

        if (hit) {
            print(hit.collider.tag);
        }
        Debug.DrawRay(rayOrigin,mousePos - rayOrigin,Color.blue);


    }

    public IEnumerator TextDuration() {
        yield return new WaitForSeconds(2);
        feedbackCanvas.SetActive(false);
    }

    public override void TakeDamage(float dmg) {
        base.TakeDamage(dmg);
    }

    public override void UpdateCurrentHealthBar() {
        currentHealthBar.rectTransform.localScale = new Vector3(currentHealth / maxHealth,1,1);
        healthBarText.text = currentHealth + " / " + maxHealth;
    }

    public override void Die() {
        //överskugga i enemy och player för animation och vad som händer
        GameManager.Instance.currency /= 2;
        GameManager.Instance.UpdateCurrencyText();

        StopAnim();
        GetComponent<Character>().enabled = false;
        GetComponent<CastSkill>().enabled = false;
        currentHealth = 0;
        renderer.material.color = Color.gray;
        feedbackText.text = "You died!";
        if (GameManager.Instance.activatedJul) {
            feedbackText.text = "Merry Christmas!";
        }
        
        feedbackText.color = Color.red;
        feedbackCanvas.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(RespawnTimer());



    }

    IEnumerator RespawnTimer() {
        yield return new WaitForSeconds(2);
        if (Application.loadedLevel == 4) {
            Application.LoadLevel(Application.loadedLevel);
        } else {
            
            GetComponent<Character>().enabled = true;
            GetComponent<CastSkill>().enabled = true;
            feedbackCanvas.SetActive(false);
            transform.position = startPos;
            
            isDead = false;
            isStunned = false;
            ResetMovement();
            ResetBurn();
            currentHealth = maxHealth;
            UpdateCurrentHealthBar();
            renderer.material.color = Color.white;

        }
    }

}
