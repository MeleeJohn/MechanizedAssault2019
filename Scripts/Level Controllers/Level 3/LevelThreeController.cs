using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelThreeController : MonoBehaviour
{
    [Header("Player Variables")]
    public GameObject playerVC;
    public PlayerController PC;
    public GameObject playerSpawnPoint;

    [Header("Player Frame")]
    public GameObject dashFrame;
    public GameObject assaultFrame;
    public GameObject titanFrame;

    [Header("Level Boss")]
    public Rover_Enemy enemyHawk;

    [Header("Cinematic Objects")]
    public GameObject cinematicOpeningTimeline;
    public Animator cinematicBars;

    [Header("Ending Cinematic")]
    public GameObject endingCinematicTimeline;
    public GameObject endingCinematicBlackScreen;
    public GameObject endingCinematicPlayerEndPosition;

    [Header("Scene Transition")]
    public GameObject cinematicScreenFader;
    public GameObject screenFader;
    public AsyncOperation async;

    [Header("Weather System")]
    public EnviroSky enviroSky;
    public int timeHour;
    public int timeMinute;
    public int timeSeconds;

    [Header("Mission Over Items")]
    public AudioController levelAudioController;
    public AudioSource quickToneSource;
    public AudioClip missionFailedTone;
    public AudioClip missionCompletedTone;

    // Start is called before the first frame update
    void Start()
    {
 
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
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0) && PlayerPrefs.GetInt("DebugMode") == 1) {
            PlayerPrefs.SetInt("FromMission", 1);
            LoadScene(0);
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

    IEnumerator OpeningCinematic() {
        cinematicOpeningTimeline.SetActive(true);
        enemyHawk.Cs = EnemyAILifeStates.Paused;
        PC.playerCanvas.gameObject.SetActive(false);
        PC.GM.canPause = false;
        yield return new WaitForSeconds(0.25f);
        PC.HUDsystem.useRadar = false;
        PC.HUDsystem.useIndicators = false;
        PC.radarPanel.SetActive(false);
        PC.indicatorPanel.SetActive(false);
        cinematicBars.SetBool("BarsIN", false);
        PC.Anim.SetBool("Arms down", true);
        yield return new WaitForSeconds(16f);
        PC.playerCanvas.gameObject.SetActive(true);
        cinematicOpeningTimeline.SetActive(false);
        PC.Camera.SetActive(true);
        PC.VirtualCamera.SetActive(true);
        cinematicBars.SetBool("BarsIN", true);
        PC.Anim.SetBool("Arms down", false);
        PC.Anim.SetBool("ArmUp", true);
        yield return new WaitForSeconds(2.25f);
        PC.Anim.SetBool("ArmUp", false);
        StartCoroutine(enemyHawk.BossStartUp());
        StartCoroutine(PC.FrameStartUp());
    }

    public IEnumerator EndingCinematic() {
        endingCinematicBlackScreen.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        PC.canMove = false;
        yield return new WaitForSeconds(1.0f);
        PC.canMove = false;
        PC.RB.constraints = RigidbodyConstraints.FreezeAll;
        PC.playerCanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        PC.RB.angularVelocity = Vector3.zero;
        PC.transform.position = endingCinematicPlayerEndPosition.transform.position;
        PC.transform.localEulerAngles = Vector3.zero;
        PC.LeftArm.transform.localEulerAngles = Vector3.zero;
        PC.RightArm.transform.localEulerAngles = Vector3.zero;
        PC.upperBody.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(0.25f);
        PC.RB.angularVelocity = Vector3.zero;
        PC.transform.position = endingCinematicPlayerEndPosition.transform.position;
        PC.transform.localEulerAngles = Vector3.zero;
        PC.LeftArm.transform.localEulerAngles = Vector3.zero;
        PC.RightArm.transform.localEulerAngles = Vector3.zero;
        PC.upperBody.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(0.25f);
        PC.RB.angularVelocity = Vector3.zero;
        PC.transform.position = endingCinematicPlayerEndPosition.transform.position;
        PC.transform.localEulerAngles = Vector3.zero;
        PC.LeftArm.transform.localEulerAngles = Vector3.zero;
        PC.RightArm.transform.localEulerAngles = Vector3.zero;
        PC.upperBody.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(0.25f);
        PC.RB.angularVelocity = Vector3.zero;
        PC.transform.position = endingCinematicPlayerEndPosition.transform.position;
        PC.transform.localEulerAngles = Vector3.zero;
        PC.LeftArm.transform.localEulerAngles = Vector3.zero;
        PC.RightArm.transform.localEulerAngles = Vector3.zero;
        PC.upperBody.transform.localEulerAngles = Vector3.zero;
        endingCinematicBlackScreen.SetActive(false);
        endingCinematicTimeline.SetActive(true);
        cinematicBars.SetBool("BarsIN", false);
        yield return new WaitForSeconds(47.0f);
        cinematicBars.SetBool("BarsIN", true);
        levelAudioController.audioSource.volume = 0.075f;
        quickToneSource.PlayOneShot(missionCompletedTone);
        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetInt("Completed Level 3",1);
        LoadScene(6);
    }

    public IEnumerator MissionFailed() {
        levelAudioController.audioSource.volume = 0.075f;
        quickToneSource.PlayOneShot(missionFailedTone);
        yield return new WaitForSeconds(7.8f);
        LoadScene(0);
    }
}
