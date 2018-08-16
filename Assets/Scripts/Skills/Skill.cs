using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill :MonoBehaviour {

    public int damage;
    public float cooldown;
    public float activeTime;
    public float projectileSpeed;
    public float slowWhileAttacking;
    public float distanceFromPlayer;

    public Image skillIcon;

    public AudioClip sound;
    public float volume = 0.2f;

    [HideInInspector]
    public Character opponent;
    public Breakable breakable;

    public TypeOfSkill skillType;

    public enum TypeOfSkill {
        Melee, Ranged, Explosion, Dash
    }

    public virtual void Start() {
        if(sound != null)
            GameManager.Instance.OtherSounds(sound,volume);
    }

    void OnTriggerEnter2D(Collider2D other) {
        opponent = other.gameObject.GetComponent<Character>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (other.tag == "Obstacle" && skillType == TypeOfSkill.Ranged) {

            SpecialEffect();


            Destroy(gameObject);
        }

        if (other.tag == "Obstacle" && skillType == TypeOfSkill.Dash) {
            player.GetComponent<Collider2D>().enabled = true;
            player.transform.SetParent(null);

            Destroy(gameObject);
        }

        if (this.tag == "PlayerSkill" && other.tag == "Enemy") {
            if (!opponent.isDead && skillType != TypeOfSkill.Dash) {
                GameManager.Instance.HitEffect(opponent.transform, opponent.transform.localScale);
                GameManager.Instance.EnemyHurtSound(GameManager.Instance.enemyDamageSound, volume);
            }   
            if(skillType == TypeOfSkill.Melee && opponent.isFrozen && GameManager.Instance.shatterSound != null) {
                    GameManager.Instance.OtherSounds(GameManager.Instance.shatterSound, 0.2f);
            }         
            opponent.TakeDamage(damage);
            SpecialEffect();
            


        } else if (this.tag == "EnemySkill" && other.tag == "Player") {
            if (!opponent.isDead && skillType != TypeOfSkill.Dash) {
                GameManager.Instance.HitEffect(opponent.transform, opponent.transform.localScale);
                GameManager.Instance.PlayerHurtSound(GameManager.Instance.playerDamageSound, volume);
            }
            opponent.TakeDamage(damage);
            if (!opponent.isDead) {
                SpecialEffect();
            }

        } else if(this.tag == "PlayerSkill" && other.tag == "Breakable") {
            breakable = other.gameObject.GetComponent<Breakable>();
            breakable.Break();
        }

    }

    /*void PlayHitSound() {
        if (!opponent.isDead) {
            GameManager.Instance.HitSounds(opponent.damageSound, volume);
        }
    }*/

    public virtual void SpecialEffect() {

        //överskugga i individuella skills
    }

}
