using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour {

    public PlayerController PC;
    public TutorialController TC;
	// Use this for initialization
	void Start () {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (TC.cutsceneBool == true){
            PC.gameObject.transform.position = this.transform.position;
            PC.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            TC.cutsceneBool = true;
            PC.RB.velocity = new Vector3(0, 0, 0);
            PC.gameObject.transform.position = this.transform.position;
        }
    }
}
