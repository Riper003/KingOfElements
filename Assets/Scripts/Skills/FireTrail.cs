using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : Skill {


    public int dotDamage;
    public int amounts;
    public float frequency;

    public override void SpecialEffect() {
        if (opponent.isFrozen == true) {
            opponent.Unfreeze();
        }
        if (!opponent.standingInFire) {
            opponent.standingInFire = true;
            opponent.TrailBurn(dotDamage, frequency, amounts);            
        }        
    }
}
