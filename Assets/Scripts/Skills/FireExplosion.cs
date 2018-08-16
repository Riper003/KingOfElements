using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : Skill {

    private Animator anim;
    public Vector3 aoe;
    public int dotDamage;
    public int amounts;
    public float frequency;

    public override void Start() {
        if(tag == "PlayerSkill") {
            aoe = new Vector3(2 + (GameManager.Instance.aoeUpgrade / 2),2 + (GameManager.Instance.aoeUpgrade / 2),1);
            
        }else {
            aoe = new Vector3(2 ,2 ,1);
        }
        GetComponent<Transform>().localScale = aoe;
        base.Start();
    }

    public override void SpecialEffect() {
        if (opponent.isFrozen == true) {
            opponent.Unfreeze();
        }
        opponent.Burn(dotDamage, frequency, amounts);
    }
}
