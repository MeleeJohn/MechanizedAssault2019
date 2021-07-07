using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TestController : MonoBehaviour {
    public GameObject playerVC;

    public GameObject dashFrame;
    public GameObject assaultFrame;
    public GameObject titanFrame;

    public PlayableDirector scenePlayableDirector;
    public PlayerController Player;
    public GameObject playerCamera;
    public Canvas playerCanvas;
    public GameObject playerCanvasOBJ;
    public float sceneTimelineLength;

    public int enemyCount;

    public EnviroSky ES;
    [Header("Controls")]
    public bool controlsOnScreen;
    public GameObject keyboardControls;

    [Header("Mission Over Items")]
    public AudioSource quickToneSource;
    public AudioClip missionFailedTone;
    public AudioClip missionCompletedTone;

    [Header("Scene Transition")]
    public bool sceneChangeActive = false;
    public GameObject screenFader;
    public AsyncOperation async;

    [Header("Weather System")]
    public EnviroSky enviroSky;
    public int timeHour;
    public int timeMinute;
    public int timeSeconds;

    public int FrameSelectionTestNumber;
    private void Awake() {
        //Debug.Log("PLayer Freame choice" + PlayerPrefs.GetInt("FrameChoice"));
        //PlayerPrefs.SetInt("FrameChoice", FrameSelectionTestNumber);
        switch (PlayerPrefs.GetInt("FrameChoice")) {
            case 1:
                //Instantiate(dashFrame);
                dashFrame.SetActive(true);
                break;

            case 2:
                //Instantiate(assaultFrame);
                assaultFrame.SetActive(true);
                break;

            case 3:
                //Instantiate(titanFrame);
                titanFrame.SetActive(true);
                break;
        }
    }

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //Player.canMove = true;
        StartCoroutine(Player.FrameStartUp());
        //enviroSky.currentHour = 22f;
        enviroSky.SetTime(2018,1,timeHour,timeMinute,timeSeconds);
        //StartCoroutine(LevelOpeningCutscene(sceneTimelineLength));
    }

	void Update () {
        /*if (controlsOnScreen == false && Input.GetKeyDown(KeyCode.Escape)) {
            keyboardControls.SetActive(true);
            playerCanvasOBJ.SetActive(false);
            controlsOnScreen = true;
        } else if (controlsOnScreen == true && Input.GetKeyDown(KeyCode.Escape)) {
            keyboardControls.SetActive(false);
            playerCanvasOBJ.SetActive(true);
            controlsOnScreen = false;
        }*/

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            LoadScene(0);
        }

        if(enemyCount <= 0 && sceneChangeActive != true) {
            sceneChangeActive = true;
            LoadScene(0);
        }
    }

    private IEnumerator LevelOpeningCutscene(float WaitTime) {
        yield return new WaitForSeconds(0.25f);
        playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();
        playerCanvasOBJ = GameObject.FindGameObjectWithTag("Player Canvas");
        playerCamera = GameObject.FindGameObjectWithTag("Player Camera");
        playerCamera.SetActive(false);
        playerCanvas.renderMode = RenderMode.WorldSpace;         
        yield return new WaitForSeconds(WaitTime);
        StartCoroutine(Player.FrameStartUp());
        playerCamera.SetActive(true);
        Player.Camera = playerCamera;
        ES.PlayerCamera = Player.Camera.GetComponent<Camera>();
        ES.currentHour = 10f;
        Player.canMove = true;
        playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    public void LoadScene(int SceneNumber) {
        Player.canMove = false;
        PlayerPrefs.SetInt("FromMission", 1);
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));
    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        yield return new WaitForSeconds(0.75f);
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            yield return null;
        }
    }

    public IEnumerator MissionFailed() {
        quickToneSource.PlayOneShot(missionFailedTone);
        yield return new WaitForSeconds(7.8f);
        LoadScene(0);
    }
}
