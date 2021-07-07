using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreMenuController : MonoBehaviour
{
    public GameObject screenFader;
    public Slider loadingBar;
    public AsyncOperation async;

    void Start()
    {
        LoadScene(0); 
    }

    public void LoadScene(int SceneNumber) {
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));
    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        yield return new WaitForSeconds(0.75f);
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}
