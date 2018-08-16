using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkillFireball : Skill {
    public Rigidbody2D explosion;
    private bool hit;
    // Use this for initialization
    public override void Start() {
        hit = false;
        base.Start();
    }


    public override void SpecialEffect() {
        if (!hit) {

            Rigidbody2D clone;
            float activeTime = explosion.GetComponent<Skill>().activeTime;
            clone = Instantiate(explosion, transform.position, transform.rotation) as Rigidbody2D;
            if (this.tag == "EnemySkill") {
                clone.tag = "EnemySkill";
            } else if (this.tag == "PlayerSkill") {
                clone.tag = "PlayerSkill";
            }
            hit = true;
            
            Destroy(clone.gameObject, activeTime);
            gameObject.SetActive(false);
            Destroy(gameObject, activeTime + 0.1f);
        }
    }

}
