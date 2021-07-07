using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RhinoLevels {Private, Corporal, Sergeant, MasterSergeant};

public class RHINO_BaseClass : MonoBehaviour {
    public RhinoLevels Level;

    public GameObject rhinoCorporalHorn;
    public GameObject rhinoSergeantHorn;
    public GameObject rhinoMasterSergeantHorn;

    void Start () {
        switch (Level) {

            case (RhinoLevels.Private):

                break;

            case (RhinoLevels.Corporal):
                rhinoCorporalHorn.SetActive(true);
                break;

            case (RhinoLevels.Sergeant):
                rhinoSergeantHorn.SetActive(true);
                break;

            case (RhinoLevels.MasterSergeant):
                rhinoMasterSergeantHorn.SetActive(true);
                break;
        }
	}

	void Update () {
		
	}
}
