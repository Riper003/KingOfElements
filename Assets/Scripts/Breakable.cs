using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    public GameObject[] itemDrops;
    public int chanceToDropHeal;
    public AudioClip[] sounds;
    public float volume = 0.2f;

    public void Break() {
        GameObject clone;
        Vector3 charPos = transform.position;
        if (Random.Range(1, 100) < chanceToDropHeal && itemDrops[0] != null) {
            if (chanceToDropHeal == 100)
                clone = Instantiate(itemDrops[0],charPos,transform.rotation,null);
            else
                clone = Instantiate(itemDrops[0], new Vector3(charPos.x, charPos.y - 1, 0), transform.rotation, null);
        }
        
        GameManager.Instance.OtherSounds(sounds[Random.Range(0, sounds.Length)], volume);
        
        if(itemDrops[1] != null) {
            
            clone = Instantiate(itemDrops[1], charPos, transform.rotation, null);
        }        
        Destroy(gameObject);
    }

}
