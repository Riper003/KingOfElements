using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkillLightningBolt : Skill {
   
    public int maxBounceCount;    
    public float bounceMaxLength;
    public GameObject lightningArea;
    private bool hit;

    public override void Start() {
        base.Start();
        hit = false;
    }


    public override void SpecialEffect() {
       
        if (gameObject.tag == "PlayerSkill")
            maxBounceCount += GameManager.Instance.aoeUpgrade;

        if (!hit) {
            if (opponent != null) {

                lightningArea = Instantiate(lightningArea,opponent.transform.position,opponent.transform.rotation) as GameObject;
                LightningField lightningField = lightningArea.GetComponent<LightningField>();
                if (gameObject.tag == "PlayerSkill")
                    lightningArea.tag = "PlayerSkill";
                if (gameObject.tag == "EnemySkill")
                    lightningArea.tag = "EnemySkill";

                lightningField.damage = damage;
                lightningField.maxBounceCount = maxBounceCount;
                lightningField.bounceMaxLength = bounceMaxLength;
                lightningField.SetCurrentBounceToZero();

                hit = true;
            }
        }



    
        Destroy(gameObject);


    }
}
