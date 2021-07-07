using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoBossAimingCollider : MonoBehaviour
{
    public Rover_Enemy RE;
    // Use this for initialization
    void Start() {
        RE = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rover_Enemy>();
    }

    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Ontrigger Enter");
        //if (autoAim == false) {
            if (other.tag == "Player" && RE.Targeting == false) {
                //Debug.Log("Ontrigger Enter POSSIBLE ENEMY");
                RE.PossibleEnemy = other.gameObject;
                RE.TargetEnemy();
        }
        /*} else if (autoAim == true) {
            if (other.tag == "Enemy" && RE.Targeting == false) {
                RE.PossibleEnemy = other.gameObject;
                RE.TargetEnemy();
            }
        }*/

    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && RE.Targeting == false) {
            RE.PossibleEnemy = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player" && RE.Targeting == true) {
            RE.UnTargeted();
        }

        if (other.tag == "Player" && RE.PossibleEnemy != null && RE.Targeting != true) {
            RE.PossibleEnemy = null;
        }
    }
}
