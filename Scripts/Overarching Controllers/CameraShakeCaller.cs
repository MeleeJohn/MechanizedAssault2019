using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeCaller : MonoBehaviour {
    public MainMenuController MMC;

    private void Start() {
        StartCoroutine(CallCameraShake());
    }

    IEnumerator CallCameraShake() {
        yield return new WaitForSeconds(4.75f);
        if (MMC.MMS == MainMenuStage.OpeningScene){
            Debug.Log("Calling CameraShake");
            StartCoroutine(MMC.CameraShakeMethod(1.5f, 0.15f));
        }
    }
}
