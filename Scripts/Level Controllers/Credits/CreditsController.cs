using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsController : MonoBehaviour
{
    public GameManager GM;
    public GameObject cinematicScreenFader;
    public GameObject screenFader;
    public AsyncOperation async;
    public TextMeshProUGUI skipCreditsText;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        StartCoroutine(CreditsTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (!GM.prevState.IsConnected) {
            skipCreditsText.text = "Press 'ESC' to skip";
            if (Input.GetKeyDown(KeyCode.Escape)) {
                LoadScene(0);
            }
        } else if (GM.prevState.IsConnected) {
            skipCreditsText.text = "Press 'Start' to skip";
            if (Input.GetButtonDown("Start")) {
                LoadScene(0);
            }
        }
    }

    public void LoadScene(int SceneNumber) {
        //screenFader.SetActive(true);
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));

    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        PlayerPrefs.SetInt("FromMission", 1);
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            yield return null;
        }
    }

    IEnumerator CreditsTimer() {
        yield return new WaitForSeconds(52.0f);

        LoadScene(0);
    }
}
