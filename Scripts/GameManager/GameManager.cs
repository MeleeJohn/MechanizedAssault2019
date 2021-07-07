using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
using Sirenix.OdinInspector;

public enum GameState { Playing, Paused };
public enum LevelControllerType { TestLevel, LevelOne, LevelTwo, LevelThree, LevelFour, LevelFive, LevelSix, Tutorial };

public class GameManager : MonoBehaviour {

    static GameManager gm;
    [EnumPaging]
    public LevelControllerType LCT;

    [ShowIf("LCT", LevelControllerType.TestLevel)]
    public TestController testLevelController;
    [ShowIf("LCT", LevelControllerType.Tutorial)]
    public TutorialController TC;
    [ShowIf("LCT", LevelControllerType.LevelOne)]
    public LevelOneController levelOneController;
    [ShowIf("LCT", LevelControllerType.LevelTwo)]
    public LevelTwoController levelTwoController;
    [ShowIf("LCT", LevelControllerType.LevelThree)]
    public LevelThreeController levelThreeController;

    public bool canPause = true;
    public GameState gameState;
    public bool inMenu;
    //Controller input
    public bool playerIndexSet = false;
    public PlayerIndex playerIndex;
    public GamePadState state;
    public GamePadState prevState;

    public GameObject pauseMenuObject;
    public PlayerController PC;
    public Canvas playerCanvas;
    public GameObject playerCanvasOBJ;

    public GameObject keyBoardControls;
    public GameObject controllerControls;
    public bool controlsUp;
    public EventSystem eventSystem;
    public GameObject resumeMissionButton;
    public GameObject controlsButton;
    void Awake() {

    }

    // Use this for initialization
    void Start() {
        if (gm == null) {
            gm = this;
        } else {
            Destroy(this.gameObject);
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        DontDestroyOnLoad(gm);

        /*if (inMenu == false) {
            PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerCanvasOBJ = GameObject.FindGameObjectWithTag("Player Canvas");
            
        }*/
    }

    // Update is called once per frame
    void Update() {
        #region ControllerSetup
        if (!playerIndexSet || !prevState.IsConnected) {
            //print ("index set or is connected are false");
            for (int i = 0; i < 4; ++i) {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected) {
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        //print ("Prev state is " + prevState);
        state = GamePad.GetState(playerIndex);
        //print ("state is " + state);

        if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) {
            //Debug.Log ("A button pressed in manager");
        }

        if (!prevState.IsConnected) {
            Cursor.visible = true;
            //print ("Prevstate isnt is connected");
        }

        if (prevState.IsConnected) {
            Cursor.visible = false;
            //print ("Prevstate is connected");
        }
        #endregion

        if (eventSystem == null) {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        }

        if (SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "Green Screen") {
            inMenu = true;
        } else {
            inMenu = false;
        }

        if (SceneManager.GetActiveScene().name != "Main Menu") {
            if (prevState.IsConnected == false) {
                if (inMenu == false) {
                    if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.Paused && controlsUp != true) {
                        gameState = GameState.Playing;
                        pauseMenuObject.SetActive(false);
                        UnpauseGame();
                    } else if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.Playing && canPause == true) {
                        gameState = GameState.Paused;
                        pauseMenuObject.SetActive(true);
                        eventSystem.SetSelectedGameObject(resumeMissionButton);
                        PauseGame();
                    }
                }
            }

            if (prevState.IsConnected == true) {
                if (inMenu == false) {
                    if (Input.GetButtonDown("Start") && gameState == GameState.Paused && controlsUp != true) {
                        gameState = GameState.Playing;
                        pauseMenuObject.SetActive(false);
                        UnpauseGame();
                    } else if (Input.GetButtonDown("Start") && gameState == GameState.Playing && canPause == true) {
                        gameState = GameState.Paused;
                        pauseMenuObject.SetActive(true);
                        eventSystem.SetSelectedGameObject(resumeMissionButton);
                        PauseGame();
                    }
                }

            }
        }

        /*switch (SceneManager.GetActiveScene().name) {

            case "Main Menu":
                if()
                break;
        }*/

        if(controlsUp == true) {
            eventSystem.SetSelectedGameObject(controlsButton);
        } else {

        }
    }

    public void ControlScreen() {
        if (controlsUp == false) {
            controlsUp = true;
            if (prevState.IsConnected == true) {
                controllerControls.SetActive(true);
            } else {
                keyBoardControls.SetActive(true);
            }
        } else {
            controlsUp = false;
            if (prevState.IsConnected == true) {
                controllerControls.SetActive(false);
            } else {
                keyBoardControls.SetActive(false);
            }
        }
    }

    public void PauseGame() {
        PC.VirtualCamera.GetComponent<PlayerCamera>().gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
        PC.VirtualCamera.GetComponent<PlayerCamera>().gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = null;
        Time.timeScale = 0f;
        //playerCanvas.renderMode = RenderMode.WorldSpace;
        playerCanvas.gameObject.SetActive(false);
        //playerCanvas.gameObject.layer = 5;
        PC.canMove = false;
    }

    public void UnpauseGame() {
        if (controlsUp == false) {
            Time.timeScale = 1f;
            PC.VirtualCamera.GetComponent<PlayerCamera>().gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = PC.followPoint;
            PC.VirtualCamera.GetComponent<PlayerCamera>().gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = PC.lookPoint;
            //playerCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            playerCanvas.gameObject.SetActive(true);
            pauseMenuObject.SetActive(false);
            //playerCanvas.gameObject.layer = 11;
            PC.canMove = true;
        }
    }

    public void ForfeitMissionButton() {
        Time.timeScale = 1f;
        PC.VirtualCamera.GetComponent<PlayerCamera>().gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = PC.followPoint;
        PC.VirtualCamera.GetComponent<PlayerCamera>().gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = PC.lookPoint;
        playerCanvas.gameObject.SetActive(true);
        pauseMenuObject.SetActive(false);
        PC.canMove = true;
        switch (SceneManager.GetActiveScene().name) {
            case "Test_Scene":
                testLevelController = GameObject.FindGameObjectWithTag("Level Controller").GetComponent<TestController>();
                testLevelController.LoadScene(0);
                break;
            case "Tutorial":
                TC = GameObject.FindGameObjectWithTag("Level Controller").GetComponent<TutorialController>();
                TC.LoadScene(0);
                break;
            case "Level 1":
                levelOneController = GameObject.FindGameObjectWithTag("Level Controller").GetComponent<LevelOneController>();
                levelOneController.LoadScene(0);
                break;
            case "Level 2":
                levelTwoController = GameObject.FindGameObjectWithTag("Level Controller").GetComponent<LevelTwoController>();
                levelTwoController.LoadScene(0);
                break;
            case "Level 3":
                levelThreeController = GameObject.FindGameObjectWithTag("Level Controller").GetComponent<LevelThreeController>();
                levelThreeController.LoadScene(0);
                break;
        }
    }
}
