using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public enum LevelDirection{Flanking, Highway, Unchosen};

public class LevelTwoController : MonoBehaviour {

    [Header("Player Variables")]
    public GameObject playerVC;
    public GameObject playerCamera;
    public PlayerController PC;
    public GameObject playerSpawnPoint;

    [Header("Player Frame")]
    public GameObject dashFrame;
    public GameObject assaultFrame;
    public GameObject titanFrame;

    [Header("Level Enemies")]
    public GameObject levelObjectiveText;
    public int enemiesCount;
    public TextMeshProUGUI enemyCountText;

    [Header("Level Points")]
    public GameObject levelEndPoint;

    [Header("Scene Transition")]
    public GameObject cinematicScreenFader;
    public GameObject screenFader;
    public AsyncOperation async;
    public float playerDistanceToEndPoint;

    [Header ("Subtitles")]
    public GameObject subtitlesObects;
    public Text currentTalker;
    public Text subtitlesText;
    public Animator cinematicBars;
    public bool cutsceneRunning;
    [Header("Cinematic TimeLines")]
    public GameObject levelEndCinematic;
    public GameObject levelCanvas;

    [Header("Mission Over Items")]
    public AudioController levelAudioController;
    public AudioSource quickToneSource;
    public AudioClip missionFailedTone;
    public AudioClip missionCompletedTone;

    [Header("Weather System")]
    public EnviroSky enviroSky;
    public int timeHour;
    public int timeMinute;
    public int timeSeconds;

    [Header("Objective Objects")]
    public GameObject objectiveSmoke;
    // Use this for initialization
    void Start () {
        //Debug.Log("PLayer Freame choice" + PlayerPrefs.GetInt("FrameChoice"));
        switch (PlayerPrefs.GetInt("FrameChoice")) {
            case 1:
                //Debug.Log("apawning frame 1");
                //Instantiate(dashFrame);
                dashFrame.SetActive(true);
                break;

            case 2:
                //Debug.Log("apawning frame 2");
                //Instantiate(assaultFrame);
                assaultFrame.SetActive(true);
                break;

            case 3:
                //Debug.Log("apawning frame 3");
                //Instantiate(titanFrame);
                titanFrame.SetActive(true);
                break;
        }

        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PC.canMove = false;
        PC.gameObject.transform.position = playerSpawnPoint.transform.position;
        //PC.gameObject.GetComponent<Animator>().enabled = false;
        StartCoroutine(OpeningCinematic());
        enviroSky.SetTime(2018, 1, timeHour, timeMinute, timeSeconds);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0) && PlayerPrefs.GetInt("DebugMode") == 1) {
            PlayerPrefs.SetInt("FromMission", 1);
            LoadScene(0);
        }
        playerDistanceToEndPoint = Vector3.Distance(PC.gameObject.transform.position, levelEndPoint.transform.position);

        if (enemiesCount <= 0 && cutsceneRunning == false) {
            objectiveSmoke.SetActive(true);
        }

        if (playerDistanceToEndPoint < 15f && enemiesCount <= 0 && cutsceneRunning == false) {
            StartCoroutine(EndingCinematic());
        }

        enemyCountText.text = enemiesCount.ToString();
	}

    public void LoadScene(int SceneNumber) {
        //screenFader.SetActive(true);
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));

    }

    IEnumerator OpeningCinematic() {
        PC.GM.canPause = false;
        yield return new WaitForSeconds(0.25f);
        levelObjectiveText.SetActive(false);
        PC.HUDsystem.useRadar = false;
        PC.HUDsystem.useIndicators = false;
        PC.radarPanel.SetActive(false);
        PC.indicatorPanel.SetActive(false);
        cinematicBars.SetBool("BarsIN", false);
        PC.Anim.SetBool("Arms down", true);
        currentTalker.text = "Convoy Leader";
        subtitlesText.text = "Hey pilot! Give us a hand here! Our convoy was ambushed while on our way to the coast for Operation Seafiring.";
        yield return new WaitForSeconds(8.25f);
        currentTalker.text = "Convoy Leader";
        subtitlesText.text = "When you head out there watch out for landmines that the enemies have placed.";
        yield return new WaitForSeconds(8.25f);
        cinematicBars.SetBool("BarsIN", true);
        PC.Anim.SetBool("Arms down", false);
        PC.Anim.SetBool("ArmUp", true);
        yield return new WaitForSeconds(2.25f);
        PC.Anim.SetBool("ArmUp", false);
        PC.GM.canPause = true;
        levelObjectiveText.SetActive(true);
        subtitlesObects.SetActive(false);
        StartCoroutine(PC.FrameStartUp());
    }

    IEnumerator EndingCinematic() {
        objectiveSmoke.SetActive(false);
        cutsceneRunning = true;
        PC.GM.canPause = false;
        PC.canMove = false;
        PC.RB.angularVelocity = Vector3.zero;
        PC.RB.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.25f);
        PC.gameObject.transform.position = levelEndPoint.transform.position;
        PC.gameObject.transform.localEulerAngles = Vector3.zero;
        PC.upperBody.transform.localEulerAngles = Vector3.zero;
        PC.playerCanvas.gameObject.SetActive(false);
        levelEndCinematic.SetActive(true);
        levelObjectiveText.SetActive(false);
        subtitlesObects.SetActive(true);
        PC.Anim.SetBool("Arms down", true);
        cinematicBars.SetBool("BarsIN", false);
        currentTalker.text = "Convoy Leader";
        subtitlesText.text = "Thanks for the help frame leader, we're on our way out!";
        yield return new WaitForSeconds(8.25f);
        levelAudioController.audioSource.volume = 0.075f;
        quickToneSource.PlayOneShot(missionCompletedTone);
        yield return new WaitForSeconds(2.00f);
        PlayerPrefs.SetInt("Completed Mission 2", 1);
        LoadScene(0);
    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        PlayerPrefs.SetInt("FromMission", 1);
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            yield return null;
        }
    }

    public IEnumerator MissionFailed() {
        levelAudioController.audioSource.volume = 0.075f;
        quickToneSource.PlayOneShot(missionFailedTone);
        yield return new WaitForSeconds(7.8f);
        LoadScene(0);
    }
}
