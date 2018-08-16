using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkillFireSword : Skill {

    public int dotDamage;
    public int amounts;
    public float frequency;


    public override void SpecialEffect() {
        if (opponent.isFrozen == true) {
            opponent.Unfreeze();
        }

        opponent.Burn(dotDamage, frequency, amounts);
    }
}
