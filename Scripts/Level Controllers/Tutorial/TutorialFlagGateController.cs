using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateType{Ground, Flight };

public class TutorialFlagGateController : MonoBehaviour {

    public TutorialController TC;
    public GateType GT;
    public bool completedGate;
    public SpriteRenderer hologramRope;
    public GameObject hudNavigationMarkerGreen;
    public GameObject hudNavigationMarkerRed;
    public Color Green;
    public Color Red;

    public bool TaskCompleted;
    public AudioSource completionSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(completedGate == true) {
            hologramRope.color = Green;
            hudNavigationMarkerRed.SetActive(false);
            hudNavigationMarkerGreen.SetActive(true);
        } else {
            hologramRope.color = Red;
            hudNavigationMarkerRed.SetActive(true);
            hudNavigationMarkerGreen.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Collided with something");
        if(other.tag == "Player" && completedGate == false) {
            completedGate = true;
            if(GT == GateType.Ground){
                completionSound.Play();
                TC.completedGroundGates++;
            } else if(GT == GateType.Flight) {
                completionSound.Play();
                TC.completedFlightGates++;
            }
        }
    }
}
