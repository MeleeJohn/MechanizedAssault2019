using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeWeaponBladeScript : MonoBehaviour {

    public float Damage;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {
            other.gameObject.GetComponent<Enemy>().Hit(Damage);
        }
    }
}
