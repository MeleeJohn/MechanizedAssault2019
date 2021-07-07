using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TutorialStages{MovementIntrodcution, FlightIntroduction, HealthUIIntroduction, CombatIntroduction, RhinoReveal};

public class TutorialController : MonoBehaviour {

    public GameManager GM;

    public grumbleAMP gA;

    [Header("Top priority variables")]
    public TutorialStages TS;
    public GameObject dashFrame;
    public bool countdownInProgress;
    public PlayerController PC;
    public Canvas playerCanvas;
    public GameObject currentObjectiveObject;
    public Text currentObjective;
    public Text currentTalker;
    public GameObject subtitlesObjectBox;
    public Text subtitlesBox;
    public string[] subtitlesArray;
    public GameObject playerSpawnPoint;
    public Text checkControlsTextBox;

    [Header("Ground Movement Stage")]
    //public GameObject gorundMovementHelperTextBox;
    public GameObject groundGates;
    public int groundGatesCount;
    public int completedGroundGates;
    public int groundGatesLeft;


    [Header("Flight Movement Stage")]
    //public GameObject flyingMovementHelperTextBox;
    public GameObject towerGates;
    public int flightGatesCount;
    public int completedFlightGates;
    public int flightGatesLeft;

    [Header("UI introduction")]
    private bool startedExplanation;

    [Header("Combat Stage")]
    public GameObject droneObjects;
    public int shotDownDrones;
    public int totalDrones;
    public int dronesLeft;
    //public bool combatTimerFinished;
    //public float combatTimerMinutes;
    //public float combatTimerSeconds;

    [Header("Rhino Reveal")]
    public bool rhinoExplanation;
    public GameObject rhinoBox;
    public GameObject redFlare;

    [Header("Animation Controllers")]
    public Animator obstacleTowersAnim;
    public Animator flagPostsAnim;

    [Header("Cutscene Items")]
    public bool cutsceneBool;
    public GameObject cutsceneTrigger;
    public GameObject cutsceneItems;

    [Header("Scene Transition")]
    public GameObject screenFader;
    public AsyncOperation async;

    private void Awake() {
        PlayerPrefs.SetInt("FrameChoice", 1);
        playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PC.gameObject.GetComponent<Animator>().enabled = false;
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    void Start () {
        currentObjectiveObject.SetActive(false);
        PC.canMove = false;
        StartCoroutine(OpeningScreen());
        PC.AmmoHolder.SetActive(false);
        PC.HealthHolder.SetActive(false);
        //gA.PlaySong(0, 0, 1f);
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.Return)) {
            obstacleTowersAnim.SetBool("Obstacle Towers Rising", true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            PlayerPrefs.SetInt("Completed Tutorial" , 1);
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            //flyingMovementHelperTextBox.SetActive(false);
            //gorundMovementHelperTextBox.SetActive(false);
            TS = TutorialStages.RhinoReveal;
        }


        switch (TS) {

            case TutorialStages.MovementIntrodcution:
                groundGatesLeft = groundGatesCount - completedGroundGates;
                currentObjective.text = "MOVE THROUGH THE GATES \n GATES LEFT: 0" + groundGatesLeft;
                if (completedGroundGates == groundGatesCount && obstacleTowersAnim.GetBool("Obstacle Towers Rising") == false) {
                    towerGates.SetActive(true);
                    obstacleTowersAnim.SetBool("Obstacle Towers Rising", true);
                    //gA.CrossFadeToNewSong(1, 0, 2.0f);
                    //gorundMovementHelperTextBox.SetActive(false);
                    currentObjectiveObject.SetActive(false);
                    StartCoroutine(WaitTime(4.0f, TutorialStages.FlightIntroduction));
                    StartCoroutine(FlightExplanation());
                    //StartCoroutine(SubtitleOnOff(10.0f, subtitlesArray[1]));
                }
                break;

            case TutorialStages.FlightIntroduction:
                flightGatesLeft = flightGatesCount - completedFlightGates;
                currentObjectiveObject.SetActive(true);
                currentObjective.text = "FLY TO THE GATES \n GATES LEFT: 0" + flightGatesLeft;
                //flyingMovementHelperTextBox.SetActive(true);
                if (completedFlightGates == flightGatesCount && obstacleTowersAnim.GetBool("Obstacle Towers Lowering") == false) {
                    obstacleTowersAnim.SetBool("Obstacle Towers Lowering", true);
                    flagPostsAnim.SetBool("Flag Posts Lowerings", true);
                    //flyingMovementHelperTextBox.SetActive(false);
                    currentObjectiveObject.SetActive(false);
                    StartCoroutine(WaitTime(2.0f, TutorialStages.HealthUIIntroduction));
                }
                break;

            case TutorialStages.HealthUIIntroduction:
                towerGates.SetActive(false);
                groundGates.SetActive(false);
                if(startedExplanation == false) {
                    startedExplanation = true;
                    StartCoroutine(UIExplanation());
                }
                break;


            case TutorialStages.CombatIntroduction:
                droneObjects.SetActive(true);
                dronesLeft = totalDrones - shotDownDrones;
                currentObjective.text = "TARGETS LEFT: " + dronesLeft;
                if (totalDrones - shotDownDrones == 0) {
                    TS = TutorialStages.RhinoReveal;
                }
                break;

            case TutorialStages.RhinoReveal:
                cutsceneTrigger.SetActive(true);
                subtitlesObjectBox.SetActive(true);
                if(cutsceneBool == false) {
                    subtitlesBox.text = "Head over to that red smoke, you've got a package on it's way";
                    redFlare.SetActive(true);
                }

                if(rhinoExplanation == false && cutsceneBool == true) {
                    rhinoExplanation = true;
                    StartCoroutine(TutorialRoundUp());
                }
                break;
        }
	}

    public IEnumerator WaitTime(float waitTimeSec, TutorialStages StageToTransferTo) {
        yield return new WaitForSeconds(waitTimeSec);
        TS = StageToTransferTo;
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player" && countdownInProgress == false) {
            countdownInProgress = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && countdownInProgress == true) {
            countdownInProgress = false;
        }
    }

    IEnumerator OpeningScreen() {
        playerCanvas.renderMode = RenderMode.WorldSpace;
        yield return new WaitForSeconds(6f);
        PC.HealthHolder.SetActive(false);
        PC.thrusterBarImage.transform.parent.gameObject.SetActive(false);
        PC.scannerBarImage.transform.parent.gameObject.SetActive(false);
        PC.AmmoHolder.SetActive(false);
        yield return new WaitForSeconds(6.50f);
        currentObjectiveObject.SetActive(true);
        //gorundMovementHelperTextBox.SetActive(true);
        PC.canMove = true;
        playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        checkControlsTextBox.gameObject.SetActive(true);
        if (!GM.prevState.IsConnected) {
            subtitlesBox.text = "Now I've set up some flags for you to manuvere through, use the 'WASD' keys to move the frame around and use your mouse to look around the area.";
            checkControlsTextBox.text = "Press the 'Esc' key to view controls.";
        } else {
            subtitlesBox.text = "Now I've set up some flags for you to manuvere through, use your left joystick to move the frame around the right joystick to look around the area.";
            checkControlsTextBox.text = "Press the 'Start' button to view controls.";
        }
        yield return new WaitForSeconds(10.00f);
        subtitlesObjectBox.SetActive(false);
        //gorundMovementHelperTextBox.SetActive(true);
    }

    IEnumerator SubtitleOnOff(float waitTime, string subtitleToPlace) {
        subtitlesObjectBox.SetActive(true);
        subtitlesBox.text = subtitleToPlace;
        yield return new WaitForSeconds(waitTime);
        subtitlesObjectBox.SetActive(false);
    }

    IEnumerator FlightExplanation() {
        subtitlesObjectBox.SetActive(true);
        subtitlesBox.text = "Apologies pilot but I'm going to be cutting power while I explain this part.";
        yield return new WaitForSeconds(7.0f);
        PC.canMove = false;
        PC.RB.angularVelocity = Vector3.zero;
        subtitlesBox.text = "Next up we'll test your flight skills. Before that i need to explain something to you.";
        yield return new WaitForSeconds(7.0f);
        PC.MiddleReticleHolder.SetActive(true);
        PC.thrusterBarImage.transform.parent.gameObject.SetActive(true);
        subtitlesBox.text = "You should've notice a new bar on your HUD, I just turned your thurster gague on so you can see how much longer you can be airborn for.";
        yield return new WaitForSeconds(10.0f);
        subtitlesBox.text = "When that bar is fully depleted your frame will start to fall out of the air, it won't hurt but it will set you back.";
        yield return new WaitForSeconds(10.0f);
        PC.canMove = true;
        if (!GM.prevState.IsConnected) {
            subtitlesBox.text = "To start flying use the 'Q' button to gain altitude.";
        } else {
            subtitlesBox.text = "To start flying use the right bumper to gain altitude.";
        }
        yield return new WaitForSeconds(5.0f);


    }

    IEnumerator UIExplanation() {
        subtitlesObjectBox.SetActive(true);
        subtitlesBox.text = "Good moves pilot, before we move into the combat simulation lets go over the combat UI.";
        yield return new WaitForSeconds(10.0f);
        PC.HealthHolder.gameObject.SetActive(true);
        subtitlesBox.text = "The blue bar above the aiming reticle is your shield bar, once that bar is depleated your frame will start taking direct damage.";
        yield return new WaitForSeconds(10.0f);
        subtitlesBox.text = "Above the shield bar is you armor integrity, if that hits zero, you're done for.";
        yield return new WaitForSeconds(7.0f);
        PC.AmmoHolder.SetActive(true);
        subtitlesBox.text = "The middle number on the top is your shoulder weapon's ammo count, the left number is your left weapon's ammo, and on the right is your right weapon's ammo count.";
        yield return new WaitForSeconds(12.0f);
        if (!GM.prevState.IsConnected) {
            subtitlesBox.text = "Now lastly, on the left corner of the ring that bar is your scanner timer. Use the '1' key to activate it and look at an enemy to reveal their current health";
            yield return new WaitForSeconds(14.0f);
        } else {
            subtitlesBox.text = "Now lastly, on the left corner of the ring that bar is your scanner timer. Press up on the D-pad to activate it and look at an enemy to reveal their current health";
            yield return new WaitForSeconds(14.0f);
        }
        yield return new WaitForSeconds(7.0f);
        subtitlesBox.text = "Since that's all finished lets get some targets up for you.";
        //gA.PlaySong(1, 0, 0.5f);
        StartCoroutine(WaitTime(7.0f, TutorialStages.CombatIntroduction));
        StartCoroutine(CombatControlsExplanation());
    }

    IEnumerator CombatControlsExplanation() {

            yield return new WaitForSeconds(4.0f);
            subtitlesBox.text = "Although you might've figured out the controls for combat already, I'll explain anyways.";
            yield return new WaitForSeconds(7.0f);
            if (!GM.prevState.IsConnected) {
                subtitlesBox.text = "Using the middle mouse button you can lock on to the enemy taht's within the aiming reticle.";
                yield return new WaitForSeconds(9.0f);
            } else {
                subtitlesBox.text = "By pressing down on the right stick you can lock on to the enemy taht's within the aiming reticle.";
                yield return new WaitForSeconds(9.0f);
            }
            if (!GM.prevState.IsConnected) {
                subtitlesBox.text = "The right mouse button fires your right arm's weapon.";
                yield return new WaitForSeconds(7.0f);
            } else {
                subtitlesBox.text = "The right mouse trigger to fire your right arm's weapon.";
                yield return new WaitForSeconds(7.0f);
            }
            if (!GM.prevState.IsConnected) {
                subtitlesBox.text = "The left mouse button fires your right arm's weapon.";
                yield return new WaitForSeconds(7.0f);
            } else {
                subtitlesBox.text = "The left mouse trigger to fire your right arm's weapon.";
                yield return new WaitForSeconds(7.0f);
            }
            subtitlesBox.text = "When you run out of ammo don't worry about reloading, the frame's automatic systems will take care of that.";
            yield return new WaitForSeconds(7.0f);
            if (!GM.prevState.IsConnected) {
                subtitlesBox.text = "Lastly use the 'E' key to birng up your shoulder weapon and use the 'F' key is used to fire your frame's shoulder mounted weapon.";
                yield return new WaitForSeconds(14.0f);
            } else {
                subtitlesBox.text = "Lastly use the 'Y' button to birng up your shoulder weapon and use the left bumper to fire your frame's shoulder mounted weapon.";
                yield return new WaitForSeconds(14.0f);
            }
            subtitlesBox.text = "Lastly the 'F' button is used to fire your frame's shoulder mounted weapon.";
            yield return new WaitForSeconds(7.0f);
        subtitlesBox.text = "If you ever forget the controls you can always check the controls screen while in the pause menu.";
        yield return new WaitForSeconds(7.0f);
        subtitlesBox.text = "Once you're finished with these guys I have a surprise for you.";
        yield return new WaitForSeconds(7.0f);
        subtitlesObjectBox.SetActive(false);

    }

    IEnumerator TutorialRoundUp() {
        PC.canMove = false;
        PC.playerCanvas.gameObject.SetActive(false);
        PC.upperBody.transform.localEulerAngles = Vector3.zero;
        PC.Camera.SetActive(false);
        cutsceneItems.SetActive(true);
        playerCanvas.renderMode = RenderMode.WorldSpace;
        currentObjectiveObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        //rhinoBox.SetActive(true);
        yield return new WaitForSeconds(7.0f);
        PC.Anim.SetBool("Arms down", true);
        subtitlesBox.text = "That right there is the new type of enemy you'll be facing out on the battlefield, the AEGD calls it the 'RHINO'.";
        yield return new WaitForSeconds(7.0f);
        subtitlesBox.text = "We just compiled the combat data from our scouts out fighting around the Colony Stations.";
        yield return new WaitForSeconds(7.0f);
        subtitlesBox.text = "From what we can tell it's a new space combat unit from the AEGD, our data shows that it hasn't been modified for ground combat.";
        yield return new WaitForSeconds(7.0f);
        subtitlesBox.text = "You can practice with this drone so you can get a-";
        yield return new WaitForSeconds(7.0f);
        currentTalker.text = "RADAR OPERATOR";
        subtitlesBox.text = "Sorry to intterupt commander but we've got a situation here.";
        yield return new WaitForSeconds(7.0f);
        currentTalker.text = "V.T.E. OPERATOR";
        subtitlesBox.text = "This better be good, we were just about fire up the new RHINO.";
        yield return new WaitForSeconds(6.0f);
        currentTalker.text = "RADAR OPERATOR";
        subtitlesBox.text = "We have a couple of unidentified radar blips on our screens that we need investigated.";
        yield return new WaitForSeconds(7.0f);
        currentTalker.text = "V.T.E. OPERATOR";
        subtitlesBox.text = "*Sigh* I guess we're going to save this for later, let's get you out of the simulator you've got a new mission now.";
        yield return new WaitForSeconds(10.0f);
        PlayerPrefs.SetInt("Completed Tutorial", 1);
        LoadScene(1);
    }

    public void LoadScene(int SceneNumber) {
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));
    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            yield return null;
        }
    }

    public IEnumerator ResetPlayer() {
        PC.canMove = false;
        yield return new WaitForSeconds(0.0f);
        PC.RB.angularVelocity = Vector3.zero;
        PC.gameObject.transform.position = playerSpawnPoint.transform.position;
        PC.gameObject.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(0.0f);
        PC.canMove = true;
    }

    /*
    IEnumerator combatTimer() {
        combatTimerSeconds--;
        if(combatTimerSeconds < 0 && combatTimerMinutes > 0){
            combatTimerMinutes--;
            combatTimerSeconds = 59;
        } else if (combatTimerSeconds == 0 && combatTimerMinutes == 0) {
            combatTimerFinished = true;
        }
        yield return new WaitForSeconds(1.0f);
        if(combatTimerSeconds < 10) {
            currentObjective.text = combatTimerMinutes + ":0" + combatTimerSeconds;
        } else {
            currentObjective.text = combatTimerMinutes + ":" + combatTimerSeconds;
        }
        if(combatTimerFinished == false) {
            StartCoroutine(combatTimer());
        }
    }*/
}
