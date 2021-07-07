using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_RollCameraController : MonoBehaviour
{
    public GameObject OpeningCamera;
    public GameObject BRollCamera;
    public GameObject frameSelectorBRollTimeline;
    public GameObject insideHangerBRollTimeline1;
    public GameObject insideHangerBRollTimeline2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            frameSelectorBRollTimeline.SetActive(true);
            StartCoroutine(BRollCutOff(10f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            insideHangerBRollTimeline1.SetActive(true);
            StartCoroutine(BRollCutOff(10f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            insideHangerBRollTimeline2.SetActive(true);
            StartCoroutine(BRollCutOff(10f));
        }

        
    }

    public IEnumerator BRollCutOff(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        frameSelectorBRollTimeline.SetActive(false);
        insideHangerBRollTimeline1.SetActive(false);
        insideHangerBRollTimeline2.SetActive(false);
        BRollCamera.SetActive(false);
        OpeningCamera.SetActive(true);
    }
}
