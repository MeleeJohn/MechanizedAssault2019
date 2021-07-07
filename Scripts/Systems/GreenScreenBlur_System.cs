using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GreenScreenBlur_System : MonoBehaviour
{

    public PostProcessVolume PPV;
    public DepthOfField PDOF;

    // Start is called before the first frame update
    void Start()
    {
        PDOF = PPV.profile.GetSetting<DepthOfField>();
        StartCoroutine(BlurMovement());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BlurMovement(){
        //PDOF.focusDistance;
        while (PDOF.focusDistance.value < 25f) {
            if(PDOF.focusDistance.value > 13) {
                PDOF.focusDistance.value += 0.35f;
            } else {
                PDOF.focusDistance.value += 0.85f;
            }
            yield return new WaitForSeconds(0.12f);
        }
    }
}
