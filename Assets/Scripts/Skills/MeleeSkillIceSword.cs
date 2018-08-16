using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkillIceSword : Skill {

	public float slowAmount;
	public float slowDuration;
    //public int damageMulti;
    //private bool multiplied = false;

  
    public override void SpecialEffect() {
        if (opponent.isFrozen == true) {
            opponent.Unfreeze();
        }        
        opponent.SlowMoveSpeed(slowAmount, slowDuration);
        /*if(gameObject.tag == "PlayerSkill") {
            GetComponentInParent<CastSkill>().rangedCD = GetComponentInParent<CastSkill>().rangedTimer;
            GetComponentInParent<CastSkill>().dashIcon.fillAmount = 0.95f;
        } */       
    }
}
