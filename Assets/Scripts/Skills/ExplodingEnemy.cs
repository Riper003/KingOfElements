using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Skill {

    private Animator anim;

    public override void SpecialEffect() {
        if (opponent.isFrozen == true) {

            opponent.Unfreeze();
        }

        

    }
}
