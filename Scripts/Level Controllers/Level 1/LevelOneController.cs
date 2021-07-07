using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelOneController : MonoBehaviour {

    //Spawn Enemies set number variable subtrac

    public Animator Anim;
    public GameObject Elevator;
    public PlayerController PC;
    public GameObject playerSpawnPoint;
    public GameObject playerCamera;
    public Canvas playerCanvas;
    public GameObject playerCanvasOBJ;
    public EnviroSky ES;
    [Header("Scene Transition")]
    public GameObject cinematicScreenFader;
    public GameObject screenFader;
    public Slider loadingBar;
    public AsyncOperation async;

    public GameObject subtitlesObects;
    public Text currentTalker;
    public Text subtitlesText;

    public AudioSource travelMusicController;
    public AudioController CombatMusicController;
    [Header("Scene Timelines")]
    public Animator cinematicBars;
    public bool cutsceneRunning;
    public GameObject openingSceneVC;
    public GameObject openingSceneTimeline;
    public GameObject elevatorExplosionTimeline;
    public GameObject elevatorFire;
    public GameObject groundTroopsFire;
    public GameObject worldCamera;
    public GameObject endingCinematicTimeline;

    [Header("Objective Objects")]
    public GameObject objectiveSmoke;
    public GameObject endObjectiveSmoke;
    public TextMeshProUGUI objectiveText;

    [Header("Enemies")]
    public int enemiesCount;
    public GameObject[] levelEnemies;
    bool endingCInematicRunning = false;
    public int enemyWave;

    [Header("Mission Over Items")]
    public AudioSource quickToneSource;
    public AudioClip missionFailedTone;
    public AudioClip missionCompletedTone;

    [Header("Weather System")]
    public EnviroSky enviroSky;
    public int timeHour;
    public int timeMinute;
    public int timeSeconds;
    // Use this for initialization
    private void Awake() {
        PlayerPrefs.SetInt("FrameChoice", 1);
    }

    void Start () {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PC.canMove = false;
        PC.gameObject.transform.position = playerSpawnPoint.transform.position;
        PC.gameObject.GetComponent<Animator>().enabled = false;
        enviroSky.SetTime(2018, 1, timeHour, timeMinute, timeSeconds);
        
        StartCoroutine(OpeningCinematic());
        //StartCoroutine(PC.FrameStartUp());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha0) && PlayerPrefs.GetInt("DebugMode") == 1) {
            PlayerPrefs.SetInt("FromMission", 1);
            LoadScene(0);
        }
        if(enemyWave >= 3) {
            enemyWave = 2;
        }

        if(enemiesCount <= 2 && enemyWave == 1) {
            enemyWave = 2;
             StartCoroutine(SpawnEnemies(2,0,6));
        }

        if (enemiesCount <= 1 && cutsceneRunning == false && enemyWave == 2) {
            endObjectiveSmoke.SetActive(true);
        }

        if (enemiesCount <= 1 && enemyWave == 2) {
            subtitlesObects.SetActive(true);
            objectiveText.text = "Retreat to the base entrance";
            if(cutsceneRunning == false){
                currentTalker.text = "COMMANDER";
                subtitlesText.text = "Frame Leader if you can hear me we're retreating from the area, get back to the base now!";
            }
            float distance = Vector3.Distance(playerSpawnPoint.transform.position, PC.gameObject.transform.position);
            if(distance <= 60f && endingCInematicRunning != true) {
                endingCInematicRunning = true;
                StartCoroutine(EndCinematic());
            }
            //Start ending sequence
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            //StartCoroutine(EleveatorAnim());
        }
    }

    public IEnumerator EleveatorAnim() {
        yield return new WaitForSeconds(0.7f);
        PC.canMove = false;
        PC.gameObject.transform.parent = Elevator.transform;
        yield return new WaitForSeconds(3.0f);
        Anim.SetBool("Elevator On", true);
        yield return new WaitForSeconds(6.0f);
        PC.gameObject.transform.parent = null;
        PC.canMove = true;

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

    IEnumerator OpeningCinematic() {
        PC.GM.canPause = false;
        yield return new WaitForSeconds(0.25f);
        PC.HUDsystem.useRadar = false;
        PC.HUDsystem.useIndicators = false;
        PC.HUDsystem.PlayerCamera = worldCamera.GetComponent<Camera>();
        PC.HUDsystem.PlayerController = worldCamera.transform;
        PC.radarPanel.SetActive(false);
        PC.indicatorPanel.SetActive(false);
        cinematicBars.SetBool("BarsIN", false);
        playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();
        playerCanvasOBJ = GameObject.FindGameObjectWithTag("Player Canvas");
        playerCamera = GameObject.FindGameObjectWithTag("Player Camera");
        PC.Anim.SetBool("Arms down", true);
        playerCamera.SetActive(false);
        enviroSky.Player = worldCamera;
        enviroSky.PlayerCamera = worldCamera.GetComponent<Camera>();
        //playerCanvas.renderMode = RenderMode.WorldSpace;
        playerCanvas.targetDisplay = 3;
        //playerCanvas.gameObject.SetActive(false);
        currentTalker.text = "COMMANDER";
        subtitlesText.text = "For this assignment you're going to be callsign Frame Leader, understood?";
        yield return new WaitForSeconds(13f);
        currentTalker.text = "COMMANDER";
        subtitlesText.text = "There were some radar blips to your north west, we've marked the location go check it out.";
        yield return new WaitForSeconds(12f);
        playerCamera.SetActive(true);
        PC.Camera = playerCamera;
        //ES.PlayerCamera = PC.Camera.GetComponent<Camera>();
        //ES.currentHour = 10f;
        openingSceneTimeline.SetActive(false);
        openingSceneVC.SetActive(false);
        //playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        playerCanvas.targetDisplay = 0;
        PC.radarPanel.SetActive(true);
        PC.indicatorPanel.SetActive(true);
        //PC.HUDsystem.useRadar = true;
        //PC.HUDsystem.useIndicators = true;
        PC.HUDsystem.PlayerCamera = PC.Camera.GetComponent<Camera>();
        PC.HUDsystem.PlayerController = PC.gameObject.transform;
        playerCanvas.gameObject.SetActive(true);
        objectiveText.transform.parent.gameObject.SetActive(true);
        objectiveText.text = "Move to the red smoke";
        yield return new WaitForSeconds(2.0f);
        PC.Anim.SetBool("ArmUp", false);
        worldCamera.SetActive(false);
        playerCamera.SetActive(true);
        enviroSky.Player = playerCamera;
        enviroSky.PlayerCamera = playerCamera.GetComponent<Camera>();
        PC.GM.canPause = true;
        subtitlesText.text = "You're the only asset available since all the other frames are running security for the peace summit taking place in the space elevtor today.";
        yield return new WaitForSeconds(7.0f);
        PC.Anim.SetBool("Arms down", false);
        PC.Anim.SetBool("ArmUp", true);
        subtitlesText.text = "Please don't mess this up, command out.";
        yield return new WaitForSeconds(5.0f);
        cinematicBars.SetBool("BarsIN", true);
        StartCoroutine(PC.FrameStartUp());
        subtitlesObects.SetActive(false);
    }

    public IEnumerator MidCinematic() {
        travelMusicController.Stop();
        StartCoroutine(CombatMusicController.PlayMusic());
        PC.GM.canPause = false;
        cutsceneRunning = true;
        PC.canMove = false ;
        PC.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        PC.upperBody.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        PC.upperBody.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        PC.upperBody.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        PC.upperBody.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        elevatorExplosionTimeline.SetActive(true);
        //PC.Anim.SetBool("inCinematic", true);
        PC.Anim.gameObject.GetComponent<Animator>().enabled = false;
        PC.gameObject.GetComponent<Animator>().enabled = true;
        //PC.gameObject.GetComponent<Animator>().SetBool("Level01MidCinematic", true);
        cinematicBars.SetBool("BarsIN", false);
        objectiveText.transform.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        playerCamera.SetActive(false);
        worldCamera.SetActive(true);
        enviroSky.Player = worldCamera;
        enviroSky.PlayerCamera = worldCamera.GetComponent<Camera>();
        playerCanvas.renderMode = RenderMode.WorldSpace;
        //54 Seconds total
        subtitlesObects.SetActive(true);
        currentTalker.text = "FRAME LEADER";
        subtitlesText.text = "I've arrived at the target point command.";
        yield return new WaitForSeconds(7f);
        currentTalker.text = "COMMANDER";
        subtitlesText.text = "Frame leader hold on we may have a situation here, stand by.";
        yield return new WaitForSecondsRealtime(14f);
        currentTalker.text = "FRAME LEADER";
        subtitlesText.text = "Command come in! Command do you read me!?";
        yield return new WaitForSecondsRealtime(7f);
        currentTalker.text = "COMMANDER";
        subtitlesText.text = "It's- *static* get ou- *static*";
        yield return new WaitForSecondsRealtime(14f);
        currentTalker.text = "COMMANDER";
        subtitlesText.text = "Command to Frame Leader, do you read me? We've lost contact with the security team, whats your status? Over.";
        yield return new WaitForSecondsRealtime(14f);
        currentTalker.text = "COMMANDER";
        subtitlesText.text = "If you can hear this you have multiple hostile units heading to your position!.";
        PC.gameObject.GetComponent<Animator>().SetBool("Level01MidCinematic", false);
        //PC.Anim.SetBool("Arms down", false);
        //PC.Anim.SetBool("ArmUp", true);
        //PC.Anim.SetBool("inCinematic", false);
        PC.Anim.gameObject.GetComponent<Animator>().enabled = true;
        PC.gameObject.GetComponent<Animator>().enabled = false;
        PC.Anim.gameObject.GetComponent<Animator>().gameObject.transform.localPosition = new Vector3(-0.07f,-0.240839f,0.06f);
        worldCamera.SetActive(false);
        playerCamera.SetActive(true);
        enviroSky.Player = playerCamera;
        enviroSky.PlayerCamera = playerCamera.GetComponent<Camera>();
        PC.Camera = playerCamera;
        PC.canMove = true;
        elevatorExplosionTimeline.SetActive(false);
        objectiveText.transform.parent.gameObject.SetActive(true);
        objectiveText.text = "Eleminate the enemies";
        playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        cutsceneRunning = false;
        yield return new WaitForSeconds(2.0f);
        PC.Anim.SetBool("ArmUp", false);
        cinematicScreenFader.SetActive(false);
        elevatorFire.SetActive(true);
        groundTroopsFire.SetActive(true);
        PC.GM.canPause = true;
        StartCoroutine(SpawnEnemies(1,0,5));
        //enemyWave = 1;
        cinematicBars.SetBool("BarsIN", true);
        yield return new WaitForSeconds(5.0f);
        //CombatMusic.Play();
        //CombatMusic.loop = true;
        subtitlesObects.SetActive(false);
    }

    public IEnumerator EndCinematic() {
        endObjectiveSmoke.SetActive(false);
        PC.GM.canPause = false;
        cutsceneRunning = true;
        PC.canMove = false;
        objectiveText.transform.parent.gameObject.SetActive(false);
        PC.Anim.gameObject.GetComponent<Animator>().enabled = false;
        PC.gameObject.GetComponent<Animator>().enabled = true;
        PC.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        PC.upperBody.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        endingCinematicTimeline.SetActive(true);
        playerCamera.SetActive(false);
        worldCamera.SetActive(true);
        enviroSky.Player = worldCamera;
        enviroSky.PlayerCamera = worldCamera.GetComponent<Camera>();
        playerCanvas.renderMode = RenderMode.WorldSpace;
        playerCanvas.gameObject.SetActive(false);
        PC.Anim.gameObject.GetComponent<Animator>().enabled = false;
        subtitlesObects.SetActive(true);
        yield return new WaitForSecondsRealtime(7f);
        currentTalker.text = "UNKNOWN";
        subtitlesText.text = "Magnus, what's your status?.";
        yield return new WaitForSecondsRealtime(5f);
        currentTalker.text = "MAGNUS";
        subtitlesText.text = "The enemy is retreating, I've sent you the battle data.";
        yield return new WaitForSecondsRealtime(6f);
        currentTalker.text = "UNKNOWN";
        subtitlesText.text = "Thank you, the UEF has got a storm coming now, more orders to come. Stand by.";
        yield return new WaitForSecondsRealtime(3f);
        currentTalker.text = "";
        subtitlesText.text = "";
        yield return new WaitForSecondsRealtime(3f);
        currentTalker.text = "MAGNUS";
        subtitlesText.text = "Modifying this battle data is just too easy.";
        CombatMusicController.audioSource.volume = 0.075f;
        quickToneSource.PlayOneShot(missionCompletedTone);
        yield return new WaitForSecondsRealtime(7f);
        PC.Anim.gameObject.GetComponent<Animator>().enabled = false;
        PlayerPrefs.SetInt("Completed Mission 1", 1);
        LoadScene(0);
    }

    public IEnumerator SpawnEnemies(int enemySetNumber, float waitTime, int enemiesAmountNumber) {
        yield return new WaitForSeconds(waitTime);
        enemiesCount += enemiesAmountNumber;

        enemyWave++;
        levelEnemies[enemySetNumber-1].SetActive(true);
    }

    public IEnumerator MissionFailed() {
        CombatMusicController.audioSource.volume = 0.075f;
        quickToneSource.PlayOneShot(missionFailedTone);
        yield return new WaitForSeconds(7.8f);
        LoadScene(0);
    }
}
