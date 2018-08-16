using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSkillIcicle : Skill {

    public float slowAmount;
    public float slowDuration;
	public int pierceAmount;

    public AudioClip shatter;

	private int pierces = 0;

    // Use this for initialization
    public override void Start() {
        pierceAmount += GameManager.Instance.aoeUpgrade;
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void SpecialEffect() {  
        if(opponent != null) {
            
            opponent.SlowMoveSpeed(slowAmount,slowDuration);
			pierces++;
        }
		if (pierces >= pierceAmount)
			Destroy (gameObject);
    }

    private void OnDestroy() {
        GameManager.Instance.OtherSounds(shatter,volume);
    }

}