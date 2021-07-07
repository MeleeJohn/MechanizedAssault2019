using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingCollider : MonoBehaviour {

    public PlayerController PC;
    public bool autoAim;
    // Use this for initialization
    void Start () {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //Auto aim player pref
        //0 is Auto Aim off
        //1 is Auto Aim on
        if(PlayerPrefs.GetInt("Auto Aim") == 1) {
            autoAim = true;
        } else {
            autoAim = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ontrigger Enter");
        if(autoAim == false) { 
            if (other.tag == "Enemy" && PC.Targeting == false)
            {
                //Debug.Log("Ontrigger Enter POSSIBLE ENEMY");
                PC.PossibleEnemy = other.gameObject;
            }
        } else if(autoAim == true) {
            if (other.tag == "Enemy" && PC.Targeting == false){
                PC.PossibleEnemy = other.gameObject;
                PC.TargetEnemy();
            }
        }

        if (other.tag == "Enemy") {
            other.GetComponent<Enemy>().Scanned = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (autoAim == false) {
            if (other.tag == "Enemy" && PC.Targeting == false) {
                PC.PossibleEnemy = other.gameObject;
            }
        }

        if (other.tag == "Enemy" && other.GetComponent<Enemy>().Scanned != true) {
            other.GetComponent<Enemy>().Scanned = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {   
        if (other.tag == "Enemy" && PC.Targeting == true){
            PC.UnTargeted();
        }

        if (other.tag == "Enemy" && PC.PossibleEnemy != null && PC.Targeting != true) {
            PC.PossibleEnemy = null;
        }
    }
}
