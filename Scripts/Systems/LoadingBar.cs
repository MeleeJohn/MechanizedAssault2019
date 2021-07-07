using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingBar : MonoBehaviour
{
    public string loadingTextString;
    public string currentText;
    public TextMeshProUGUI loadingText;
    public float textTypeDelay;

    void Start()
    {
        StartCoroutine(LoadingLoop());
    }

    private IEnumerator LoadingLoop() {
        currentText = "";
        loadingText.text = currentText;
        for (int i = 0; i < loadingTextString.Length; i++) {
            currentText = loadingTextString.Substring(0, i);
            loadingText.text = currentText;
            yield return new WaitForSeconds(textTypeDelay);
        }
        StartCoroutine(LoadingLoop());
    }
}
