
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dummy :Character {


	CastSkill attack;


	public Type enemyType;

	public enum Type {
		Melee, Ranged, Exploding
	}

	// Use this for initialization
	public override void Start() {
		base.Start();

		attack = GetComponent<CastSkill> ();

	/*	switch (enemyType) {
		case Type.Melee:
			attackRange = meleeAttackRange;
			castTime = meleeCastTime;
			enemyAttack = attack.melees;
			cooldown = meleeCooldown;
			break;
		case Type.Ranged:
			attackRange = rangedAttackRange;
			castTime = rangedCastTime;
			enemyAttack = attack.projectiles;
			cooldown = rangedCooldown;
			break;
		case Type.Exploding:
			attackRange = explodingAttackRange;
			castTime = explodingCastTime;
			enemyAttack = attack.explosions;

			break;
		} */
	}

	// Update is called once per frame
	public override void Update() {
		base.Update();

	}
		


	public override void TakeDamage(float dmg) {
		base.TakeDamage(dmg);
	}
		

	public override void Die() {
		isDead = false;
		currentHealth = maxHealth;
	}
	public override void UpdateCurrentHealthBar() {
		//currentHealthBar.rectTransform.localScale = new Vector3 (currentHealth / maxHealth, 1, 1);
		base.UpdateCurrentHealthBar();
	}



}
