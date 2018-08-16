using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostExplosion : Skill {


    public override void SpecialEffect() {
        opponent.Freeze();
    }

}
