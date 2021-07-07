using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    [Header("Scene Transition")]
    public GameObject sceneTrasiitonItems;
    public Slider loadingBar;
    public AsyncOperation async;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            LoadMechSelect();
        }
	}

    public void LoadMechSelect() {
        LoadScene(1);
    }

    public void LoadScene(int SceneNumber) {
        sceneTrasiitonItems.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));

    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}
