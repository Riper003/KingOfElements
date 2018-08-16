using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkillSword : Skill {



    public override void SpecialEffect() {
        if (opponent.isFrozen == true) {
            opponent.Unfreeze();
        }

    }
}
