using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneCutsceneTrigger : MonoBehaviour {

    public LevelOneController LOC;
    //public bool cutsceneRunning;
    public GameObject cutscenePLayerPosition;
    private bool cutsceneTriggered;
	void Start () {

	}
	
	void Update () {
	    if(LOC.cutsceneRunning == true) {
             LOC.PC.gameObject.transform.position = cutscenePLayerPosition.transform.position;
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && LOC.cutsceneRunning == false && cutsceneTriggered == false) {
            cutsceneTriggered = true;
            StartCoroutine(LOC.MidCinematic());
        }
    }
}
