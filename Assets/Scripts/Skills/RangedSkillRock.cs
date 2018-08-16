using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkillRock :Skill {


    public override void SpecialEffect() {
        if(opponent != null) {
            
            if (opponent.isFrozen == true) {

                opponent.Unfreeze();
            }
        }
        


        Destroy(gameObject);
    }

}
