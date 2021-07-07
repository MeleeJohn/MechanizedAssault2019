using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAlphaFade : MonoBehaviour {

    private int x= 1;
    private Text text;

	void Start (){
        text = GetComponent<Text>();
        AlphaFade();
	}
	
    void AlphaFade() {
        for(int i = 0; i>-1; i = i+x) {
            text.color = new Color(255, 0, 0, i);
            if(i == 255) {
                x = -1;
            }
        }
        AlphaFade();
    }
}
