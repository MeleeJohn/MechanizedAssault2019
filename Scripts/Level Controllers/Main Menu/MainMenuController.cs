using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public enum MainMenuStage {OpeningScene, fullSelection, missionSelection, frameSelection, upperFrameSelection,rightWeaponSelection, leftWeaponSelection, Settings, dataPort};
public enum FrameSelection {DASH, Assault, Titan};

public class MainMenuController : MonoBehaviour {

    public GameManager GM;
    public MainMenuSliderController MMSC;
    public Animator Anim;
    //public int SelectionStage;
    public MainMenuStage MMS = MainMenuStage.fullSelection;
    public MainMenuStage previousMenuStage;
    public TextMeshProUGUI gameVersionText;

    [Header("Arrays")]
    public GameObject[] missionSelectItems;

    [Header("Opening Title")]
    public TextMeshProUGUI openingInstructions;
    public GameObject openingSceneItems;
    public float openingPosedFrameSelction;
    public GameObject posedDashFrame;
    public GameObject posedAssaultFrame;
    public GameObject posedTitanFrame;
    public GameObject openingFadeObject;
    public GameObject[] FrameLights;
    public GameObject alloyGamesSplashScreen;

    [Header("Camera and Camera positions and Rotations")]
    public Vector3 LookingRotation;
    public bool cameraShaking;
    public bool canMoveCamera;
    public GameObject mainCamera;
    public GameObject selectedCameraPosition;
    public GameObject openingCameraPosition;
    public GameObject startingCameraPoint;
    public GameObject frameCameraPoint;
    public GameObject frameUpperCameraPoint;
    public GameObject SettingsCameraPoint;
    public GameObject MissionSelectCameraPoint;
    public GameObject dataBoardCameraPoint;

    [Header("Frame Selection")]
    [FoldoutGroup("Frame and Weapons")]
    public GameObject movingPlatform;
    [FoldoutGroup("Frame and Weapons")]
    public FrameSelection FS;
    [FoldoutGroup("Frame and Weapons")]
    private int frameNumber = 1;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject FrameSelectionObjects;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject DashFrame;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject AssaultFrame;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject TitanFrame;
    [FoldoutGroup("Frame and Weapons")]
    public int autoAimChoice;
    [FoldoutGroup("Frame and Weapons")]
    public int frameExtraArmorModifier = 0;
    [FoldoutGroup("Frame and Weapons")]
    public int frameExtraFuelModifier = 0;
    [FoldoutGroup("Frame and Weapons")]
    public int frameIncreasedShieldCapacity = 0;
    [FoldoutGroup("Frame and Weapons")]
    public int frameIncreasedScannerTime = 0;

    [Header("Frame Stats and Descriptions")]
    [FoldoutGroup("Frame and Weapons")]
    public Text frameDescriptionBox;
    [FoldoutGroup("Frame and Weapons")]
    public string dashDescription;
    [FoldoutGroup("Frame and Weapons")]
    public string assaultDescription;
    [FoldoutGroup("Frame and Weapons")]
    public string titanDescription;

    [FoldoutGroup("Frame and Weapons")]
    [Header("Weapon Selection")]
    public string[] weaponTypes;
    [FoldoutGroup("Frame and Weapons")]
    public string[] weaponNames;
    //public GameObject WeaponSelectionObjects;

    #region Left Weapon Variables
    [FoldoutGroup("Frame and Weapons")]
    public int leftExtraAmmoVariable = 0;
    [FoldoutGroup("Frame and Weapons")]
    public int leftWeaponNumber;
    [FoldoutGroup("Frame and Weapons")]
    public int selectedLeftWeaponNumber;
    [FoldoutGroup("Frame and Weapons")]
    public TextMeshProUGUI leftWeaponNameTextBox;
    [FoldoutGroup("Frame and Weapons")]
    public TextMeshProUGUI leftWeaponTypeTextBox;
    [FoldoutGroup("Frame and Weapons")]
    public TextMeshProUGUI leftWeaponMagazineSizeText;
    [FoldoutGroup("Frame and Weapons")]
    public Slider leftWeaponDamage;
    [FoldoutGroup("Frame and Weapons")]
    public Slider leftWeaponRateOfFire;
    [FoldoutGroup("Frame and Weapons")]
    public Slider leftWeaponRange;

    [FoldoutGroup("Frame and Weapons")]
    public Button leftConfirmWeapon;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftShotgun;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftAssaultRifle;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftSubMachinegun;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftSniperRifle;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftMinigun;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftMarksmanRifle;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject leftShortSword;
    [Space]
    #endregion

    #region Right Weapon Variables
    [FoldoutGroup("Frame and Weapons")]
    public int rightExtraAmmoVariable = 0;
    [FoldoutGroup("Frame and Weapons")]
    public int rightWeaponNumber;
    [FoldoutGroup("Frame and Weapons")]
    public int selectedRightWeaponNumber;
    [FoldoutGroup("Frame and Weapons")]
    public TextMeshProUGUI rightWeaponNameTextBox;
    [FoldoutGroup("Frame and Weapons")]
    public TextMeshProUGUI rightWeaponTypeTextBox;
    [FoldoutGroup("Frame and Weapons")]
    public TextMeshProUGUI rightWeaponMagazineSizeText;
    [FoldoutGroup("Frame and Weapons")]
    public Slider rightWeaponDamage;
    [FoldoutGroup("Frame and Weapons")]
    public Slider rightWeaponRateOfFire;
    [FoldoutGroup("Frame and Weapons")]
    public Slider rightWeaponRange;

    [FoldoutGroup("Frame and Weapons")]
    public Button rightConfirmWeapon;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightShotgun;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightAssaultRifle;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightSubMachinegun;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightSniperRifle;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightMinigun;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightMarksmanRifle;
    [FoldoutGroup("Frame and Weapons")]
    public GameObject rightShortSword;
    #endregion

    #region Shoulder Weapon Variables
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject dashAML;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject dashMML;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject dashSE;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject assaultGrenadeLauncher;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject assaultStrykerCannon;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject assaultSE;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject titanShoulderCannon;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject titanRailgun;
    [FoldoutGroup("Shoulder Weapons")]
    public GameObject titanSE;
    [FoldoutGroup("Shoulder Weapons")]
    public int shoulderWeaponSpot;
    [FoldoutGroup("Shoulder Weapons")]
    public string[] shoulderWeaponNames;
    [FoldoutGroup("Shoulder Weapons")]
    public string[] shoulderWeaponDescriptions;
    [FoldoutGroup("Shoulder Weapons")]
    public TextMeshProUGUI shoulderWeaponName;
    [FoldoutGroup("Shoulder Weapons")]
    public TextMeshProUGUI shoulderWeaponDescription;
    [FoldoutGroup("Shoulder Weapons")]
    public TextMeshProUGUI shoulderWeaponChargeTime;
    #endregion

    [Header("Data Log")]
    public GameObject dataLogConnectingObject;

    [Header("Music")]
    public AudioController AC;

    [Header("Settings")]
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public AudioClip buttonClickFX;
    public AudioClip buttonClickFX02;
    public Slider masterVolumeSlider;
    public Slider soundEffectSlider;
    public Slider musicSlider;
    public Toggle debugModeToggle;
    public GameObject gameProgressResetWarning;
    public GameObject mainSettingsPanel;
    public GameObject volumeSettingsPanel;
    public GameObject extraOptionsPanel;
    public bool inResetProgressWarning;

    float master;
    float music;
    float soundEffects;

    [Header("UI Focus Items")]
    public EventSystem eventSystem;
    public GameObject mainMenuFocusObject;
    public GameObject missionSelectFocusObject;
    public GameObject frameSelectFocusObject;
    public GameObject settingsFocusObject;
    public GameObject dataLogFocusObject;
    public GameObject resetWarningFocusObject;
    public GameObject resetWarningPanelFocusObject;
    public GameObject debugModeToggleFocusObject;
    public GameObject masterVolumeFocusObject;

    [FoldoutGroup("Frame and Weapons")]
    [Header("Booleans")]
    public bool canRotate;
    public CursorLockMode wantedMode;

    [Header("Scene Transition")]
    public GameObject screenFader;
    public AsyncOperation async;

    [Header("Weather System")]
    public EnviroSky enviroSky;
    public int timeHour;
    public int timeMinute;
    public int timeSeconds;

    void Awake() {
        //GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioMixer = Resources.Load("Audio/GameAudioMixer") as AudioMixer;
        /*masterVolumeSlider = GameObject.FindGameObjectWithTag("MasterVolumeSlider").GetComponent<Slider>();
        soundEffectSlider = GameObject.FindGameObjectWithTag("SoundEffectSlider").GetComponent<Slider>();
        musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();*/
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
    }

    // Use this for initialization
    void Start () {
        gameVersionText.text = "V " + Application.version.ToString();
        audioMixer.GetFloat("Master", out master);
        audioMixer.GetFloat("Music", out music);
        audioMixer.GetFloat("SoundsFX", out soundEffects);

        Debug.Log("Master: " + master);
        Debug.Log("Music: " + music);
        Debug.Log("SoundFX: " + soundEffects);

        masterVolumeSlider.value = master;
        soundEffectSlider.value = soundEffects;
        musicSlider.value = music;
        enviroSky.SetTime(2018, 1, timeHour, timeMinute, timeSeconds);
        //enviroSky.currentHour = 3f;
        //enviroSky.PlayerCamera = mainCamera.GetComponent<Camera>();
        
        if (PlayerPrefs.GetInt("FromMission") == 1) {
            MMS = MainMenuStage.fullSelection;
            eventSystem.SetSelectedGameObject(mainMenuFocusObject);
            mainCamera.transform.position = startingCameraPoint.transform.position;
            openingFadeObject.SetActive(false);
            openingSceneItems.SetActive(false);
            alloyGamesSplashScreen.SetActive(false);
            PlayerPrefs.SetInt("FromMission", 0);
        } else {
            StartCoroutine(OpeningScreenFade());
            openingPosedFrameSelction = Random.Range(0f,3f);
            if (Input.GetJoystickNames().Length > 0) {
                
                Cursor.lockState = wantedMode;
                Cursor.visible = (CursorLockMode.Locked != wantedMode);
                openingInstructions.text = "Press 'Start'";
            } else {
                openingInstructions.text = "Press 'Enter'";
            }
            openingSceneItems.SetActive(false);
            if (openingPosedFrameSelction > 0f && openingPosedFrameSelction < 1.1f) {
                posedDashFrame.SetActive(true);
            } else if (openingPosedFrameSelction > 1f && openingPosedFrameSelction < 2.1f) {
                posedAssaultFrame.SetActive(true);
            } else if (openingPosedFrameSelction > 2f && openingPosedFrameSelction < 3f) {
                posedTitanFrame.SetActive(true);
            }
        }
        //PlayerPrefs.SetInt("Completed Tutorial", 0);

    }

    // Update is called once per frame
     void Update () {

        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKey(KeyCode.RightControl) && MMS == MainMenuStage.OpeningScene) {
            PlayerPrefs.SetInt("Completed Tutorial", 1);
        }

        if (Input.GetButtonDown("Start") && MMS == MainMenuStage.OpeningScene && canMoveCamera == true) {
            //Debug.Log("Pressing Enter");
            if(PlayerPrefs.GetInt("Completed Tutorial") == 1) {
                MMS = MainMenuStage.fullSelection;
                eventSystem.SetSelectedGameObject(mainMenuFocusObject);
                openingSceneItems.SetActive(false);
            } else if(PlayerPrefs.GetInt("Completed Tutorial") == 0) {
                //Debug.Log("Going to tutorial");
                LoadScene(4);
            }
        }

        if (Input.GetButtonDown("Cancel") && MMS != MainMenuStage.OpeningScene) {
            BackButton(previousMenuStage);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) && wantedMode == CursorLockMode.Locked) {
            switch (MMS) {

                case MainMenuStage.fullSelection:
                    eventSystem.SetSelectedGameObject(mainMenuFocusObject);
                    break;

                case MainMenuStage.missionSelection:
                    eventSystem.SetSelectedGameObject(missionSelectFocusObject);
                    break;

                case MainMenuStage.frameSelection:
                    eventSystem.SetSelectedGameObject(frameSelectFocusObject);
                    break;

                case MainMenuStage.Settings:
                    eventSystem.SetSelectedGameObject(settingsFocusObject);
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) && PlayerPrefs.GetInt("DebugMode") == 1) {
            LoadScene(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            LoadScene(6);
        }

        if (MMS != MainMenuStage.upperFrameSelection){
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, selectedCameraPosition.transform.position, 0.02f);
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, Quaternion.Euler(LookingRotation), 0.02f);
        }

        switch (MMS) {
            case MainMenuStage.OpeningScene:
                if(cameraShaking == false){
                    selectedCameraPosition = openingCameraPosition;
                }
                LookingRotation = new Vector3(-8f, -9f, 0f);
                break;

            case MainMenuStage.fullSelection:
                LookingRotation = new Vector3(0f, -180f, 0f);
                selectedCameraPosition = startingCameraPoint;
                break;

            case MainMenuStage.missionSelection:
                LookingRotation = new Vector3(0f, 90f, 0f);
                selectedCameraPosition = MissionSelectCameraPoint;
                break;

            case MainMenuStage.frameSelection:
                LookingRotation = new Vector3(0f, -90f, 0f);
                if(!Input.GetKey(KeyCode.Tab)){
                    selectedCameraPosition = frameCameraPoint;
                }
                break;

            case MainMenuStage.Settings:
                selectedCameraPosition = SettingsCameraPoint;
                break;

            case MainMenuStage.dataPort:
                selectedCameraPosition = dataBoardCameraPoint;
                LookingRotation = new Vector3(0f, 180f, 0f);
                break;
        }

        if (Input.GetButtonDown("frameView")) {
            if (MMS == MainMenuStage.upperFrameSelection) {
                StartCoroutine(UpperFrameSelection());
            } else if (MMS == MainMenuStage.frameSelection) {
                StartCoroutine(UpperFrameSelection());
            }
        }

        #region Shoulder Selector
        switch (FS) {

            case FrameSelection.DASH:
                switch (shoulderWeaponSpot) {
                    case 1:
                        dashAML.SetActive(false);
                        dashMML.SetActive(true);
                        shoulderWeaponName.text = shoulderWeaponNames[0];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[0];
                        break;

                    case 2:
                        dashAML.SetActive(true);
                        dashMML.SetActive(false);
                        dashSE.SetActive(false);
                        shoulderWeaponChargeTime.text = "N/A";
                        shoulderWeaponName.text = shoulderWeaponNames[1];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[1];
                        break;

                    case 3:
                        dashAML.SetActive(false);
                        dashSE.SetActive(true);
                        shoulderWeaponChargeTime.text = "4.0 Secs";
                        shoulderWeaponName.text = shoulderWeaponNames[6];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[6];
                        break;
                }
                break;

            case FrameSelection.Assault:
                switch (shoulderWeaponSpot) {
                    case 1:
                        assaultGrenadeLauncher.SetActive(true);
                        assaultStrykerCannon.SetActive(false);
                        shoulderWeaponName.text = shoulderWeaponNames[2];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[2];
                        break;

                    case 2:
                        assaultGrenadeLauncher.SetActive(false);
                        assaultStrykerCannon.SetActive(true);
                        assaultSE.SetActive(false);
                        shoulderWeaponChargeTime.text = "N/A";
                        shoulderWeaponName.text = shoulderWeaponNames[3];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[3];
                        break;

                    case 3:
                        assaultSE.SetActive(true);
                        assaultStrykerCannon.SetActive(false);
                        shoulderWeaponChargeTime.text = "4.0 Secs";
                        shoulderWeaponName.text = shoulderWeaponNames[6];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[6];
                        break;
                }
                break;

            case FrameSelection.Titan:
                switch (shoulderWeaponSpot) {
                    case 1:
                        titanShoulderCannon.SetActive(true);
                        titanRailgun.SetActive(false);
                        shoulderWeaponChargeTime.text = "N/A";
                        shoulderWeaponName.text = shoulderWeaponNames[4];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[4];
                        break;

                    case 2:
                        titanShoulderCannon.SetActive(false);
                        titanSE.SetActive(false);
                        titanRailgun.SetActive(true);
                        shoulderWeaponChargeTime.text = "N/A";
                        shoulderWeaponName.text = shoulderWeaponNames[5];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[5];
                        break;

                    case 3:
                        titanSE.SetActive(true);
                        titanRailgun.SetActive(false);
                        shoulderWeaponChargeTime.text = "4.0 Secs";
                        shoulderWeaponName.text = shoulderWeaponNames[6];
                        shoulderWeaponDescription.text = shoulderWeaponDescriptions[6];
                        break;
                }
                break;
        }
        #endregion
    }

    public IEnumerator UpperFrameSelection() {
        Debug.Log("Upper frame selection");
        
        if (MMS == MainMenuStage.upperFrameSelection) {
            yield return new WaitForSeconds(0.1f);
            canRotate = false;
            mainCamera.transform.position = frameCameraPoint.transform.position;
            mainCamera.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
            MMS = MainMenuStage.frameSelection;
        } else  if(MMS == MainMenuStage.frameSelection){
            yield return new WaitForSeconds(0.1f);
            canRotate = true;
            mainCamera.transform.position = frameUpperCameraPoint.transform.position;
            mainCamera.transform.localEulerAngles = new Vector3(16.322f, -108.892f, -2.097f);
            MMS = MainMenuStage.upperFrameSelection;
        }
    }

    public void ToMissionSelect() {
        eventSystem.SetSelectedGameObject(missionSelectFocusObject);
        previousMenuStage = MMS;
        MMS = MainMenuStage.missionSelection;
    }

    public void MissionSelectItemsActivation() {
        StartCoroutine(ActivateMultipleObjects(missionSelectItems, 2f, true));
    }

    public void MissionSelectItemsDeactivation() {
        StartCoroutine(ActivateMultipleObjects(missionSelectItems, 2f, false));
    }

    public void ToMainMenu() {
        eventSystem.SetSelectedGameObject(mainMenuFocusObject);
        MMS = MainMenuStage.fullSelection;
    }

    public void ToFrameSelection() {
        previousMenuStage = MMS;
        MMS = MainMenuStage.frameSelection;
        eventSystem.SetSelectedGameObject(frameSelectFocusObject);
        shoulderWeaponSpot = 1;
        StartCoroutine(FrameOut());
    }

    public void ToDataPortSelection() {
        previousMenuStage = MMS;
        MMS = MainMenuStage.dataPort;
        dataLogConnectingObject.SetActive(false);
        dataLogFocusObject.SetActive(true);
        eventSystem.SetSelectedGameObject(dataLogFocusObject);
        //shoulderWeaponSpot = 1;
    }

    public void SettingsButton() {
        if (MMS != MainMenuStage.Settings) {
            previousMenuStage = MMS;
            MMS = MainMenuStage.Settings;
            eventSystem.SetSelectedGameObject(settingsFocusObject);
        } else if (MMS == MainMenuStage.Settings) {
            previousMenuStage = MMS;
            MMS = MainMenuStage.fullSelection;
            eventSystem.SetSelectedGameObject(mainMenuFocusObject);
        }
    }

    public void BackButton(MainMenuStage lastStage) {
        switch (lastStage) {
            case MainMenuStage.fullSelection:
                MMS = MainMenuStage.fullSelection;
                eventSystem.SetSelectedGameObject(mainMenuFocusObject);
                ToMainMenu();
                break;

            case MainMenuStage.missionSelection:
                MMS = MainMenuStage.missionSelection;
                eventSystem.SetSelectedGameObject(missionSelectFocusObject);
                ToMissionSelect();
                break;

            case MainMenuStage.frameSelection:
                MMS = MainMenuStage.frameSelection;
                eventSystem.SetSelectedGameObject(frameSelectFocusObject);
                ToFrameSelection();
                break;

            case MainMenuStage.Settings:
                MMS = MainMenuStage.Settings;
                eventSystem.SetSelectedGameObject(settingsFocusObject);
                SettingsButton();
                break;
        }
    }

    public IEnumerator ActivateMultipleObjects(GameObject[] ObjectArray, float WaitTime, bool ActivationState) {
        if(WaitTime == 0) {
            for (int i = 0; i < ObjectArray.Length; i++) {
                ObjectArray[i].SetActive(ActivationState);
            }
        } else {
            yield return new WaitForSeconds(WaitTime);
            for (int i = 0; i<ObjectArray.Length; i++) {
                ObjectArray[i].SetActive(ActivationState);
            }
        }
    }

    public void WeaponFrameNextButton() {
        switch (MMS) {
            case MainMenuStage.fullSelection:
                eventSystem.SetSelectedGameObject(frameSelectFocusObject);
                StartCoroutine(FrameOut());
                MMS = MainMenuStage.frameSelection;
                break;

            case MainMenuStage.missionSelection:
                eventSystem.SetSelectedGameObject(frameSelectFocusObject);
                StartCoroutine(FrameOut());
                MMS = MainMenuStage.frameSelection;
                break;

            case MainMenuStage.frameSelection:
                FrameSelectionObjects.SetActive(false);
                StartCoroutine(FrameInWeaponsOut());
                MMS = MainMenuStage.rightWeaponSelection;
                break;

            case MainMenuStage.rightWeaponSelection:
                MMS = MainMenuStage.leftWeaponSelection;
                break;

            case MainMenuStage.leftWeaponSelection:
                StartCoroutine(WeaponsIn());
                break;
        }

    }

    public void WeaponFrameBackButton() {
        switch (MMS) {

            case MainMenuStage.frameSelection:
                MMS = MainMenuStage.fullSelection;
                break;

            case MainMenuStage.rightWeaponSelection:
                StartCoroutine(FrameOutWeaponsIn());
                MMS = MainMenuStage.frameSelection;
                break;

            case MainMenuStage.leftWeaponSelection:
                MMS = MainMenuStage.rightWeaponSelection;
                break;
        }
    }

    #region Frame and Weapon Select
    #region All Arming Cage Animations
    public IEnumerator ArmingCageTest() {
        yield return new WaitForSeconds(2.0f);
        Anim.SetBool("Cage Opening", true);
        yield return new WaitForSecondsRealtime(13.0f);
        Anim.SetBool("Cage Opening", false);
        Anim.SetBool("Cage Closing", true);
        yield return new WaitForSeconds(6.5f);
    }

    public IEnumerator FrameOut() {
        Anim.SetBool("Cage Opening", true);
        yield return new WaitForSeconds(4.0f);
        FrameSelectionObjects.SetActive(true);
        frameDescriptionBox.text = dashDescription;
        FrameSelectionObjects.SetActive(true);
    }

    public IEnumerator FrameIn() {
        Anim.SetBool("Cage Closing", true);
        yield return new WaitForSeconds(2.0f);
        Anim.SetBool("Cage Closing", false);
        Anim.SetBool("Weapons Retract/Frame Out", false);
    }

    public IEnumerator FrameInWeaponsOut() {
        FrameSelectionObjects.SetActive(false);
        Anim.SetBool("Cage Opening", false);
        Anim.SetBool("Weapons Out", true);
        Anim.SetBool("Weapons Retract/Frame Out", false);
        yield return new WaitForSeconds(3.0f);
    }

    public IEnumerator FrameOutWeaponsIn() {
        Anim.SetBool("Weapons Retract/Frame Out", true);
        Anim.SetBool("Weapons Out", false);
        yield return new WaitForSeconds(5.0f);
        FrameSelectionObjects.SetActive(true);
    }

    public IEnumerator WeaponsIn() {
        Anim.SetBool("Weapons IN", true);
        Anim.SetBool("Weapons Out", false);
        screenFader.SetActive(true);
        yield return new WaitForSeconds(1.3f);
    }
    #endregion

    public void DashSelect() {
        frameNumber = 1;
        shoulderWeaponSpot = 1;
        FS = FrameSelection.DASH;
        AssaultFrame.SetActive(false);
        TitanFrame.SetActive(false);
        movingPlatform.transform.eulerAngles = new Vector3(0f,90f,0f);
        frameDescriptionBox.text = dashDescription; 
        DashFrame.SetActive(true);

    }

    public void AssaultSelect() {
        frameNumber = 2;
        shoulderWeaponSpot = 1;
        FS = FrameSelection.Assault;
        DashFrame.SetActive(false);
        TitanFrame.SetActive(false);
        movingPlatform.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        frameDescriptionBox.text = assaultDescription;
        AssaultFrame.SetActive(true);

    }

    public void TitanSelect() {
        frameNumber = 3;
        shoulderWeaponSpot = 1;
        FS = FrameSelection.Titan;
        DashFrame.SetActive(false);
        AssaultFrame.SetActive(false);
        movingPlatform.transform.eulerAngles = new Vector3(0f, 90f, 0f);
        frameDescriptionBox.text = titanDescription;
        TitanFrame.SetActive(true);
    }

    public void FrameExtraArmor() {
        if (frameExtraArmorModifier == 1) {
            MMSC.frameArmorModifier -= 10;
            MMSC.frameSpeedModifier += 9;
            MMSC.frameWeightModifier -= 15;
            MMSC.frameFlightModifier += 7;
            frameExtraArmorModifier = 0;
        } else if (frameExtraArmorModifier == 0) {
            MMSC.frameArmorModifier += 10;
            MMSC.frameSpeedModifier -= 9;
            MMSC.frameWeightModifier += 15;
            MMSC.frameFlightModifier -= 7;
            frameExtraArmorModifier = 1;
        }
    }

    public void FrameExtraFuel() {
        if (frameExtraFuelModifier == 1) {
            MMSC.frameSpeedModifier += 5;
            MMSC.frameFlightModifier -= 13;
            MMSC.frameWeightModifier -= 10;
            frameExtraFuelModifier = 0;
        } else if (frameExtraFuelModifier == 0) {
            MMSC.frameSpeedModifier -= 5;
            MMSC.frameFlightModifier += 13;
            MMSC.frameWeightModifier += 10;
            frameExtraFuelModifier = 1;
        }
    }

    public void FrameIncreasedShieldCapacity() {
        if(frameIncreasedShieldCapacity == 1) {
            frameIncreasedShieldCapacity = 0;
        } else if(frameIncreasedShieldCapacity == 0) {
            frameIncreasedShieldCapacity = 1;
        }
    }

    public void FrameIncreasedScannerTime() {
        if (frameIncreasedScannerTime == 1) {
            frameIncreasedScannerTime = 0;
        } else if (frameIncreasedScannerTime == 0) {
            frameIncreasedScannerTime = 1;
        }
    }

    public void LeftWeaponButtonNext() {
        if (leftWeaponNumber == 0 || leftWeaponNumber < 6) {
            leftWeaponNumber++;
            switch (leftWeaponNumber) {
                case 0:
                    leftWeaponNameTextBox.text = weaponNames[0];
                    leftWeaponTypeTextBox.text = weaponTypes[0];
                    leftWeaponMagazineSizeText.text = "100";
                    leftAssaultRifle.SetActive(false);
                    leftSubMachinegun.SetActive(true);
                    break;

                case 1:
                    leftWeaponNameTextBox.text = weaponNames[1];
                    leftWeaponTypeTextBox.text = weaponTypes[1];
                    leftWeaponMagazineSizeText.text = "50";
                    leftSubMachinegun.SetActive(false);
                    leftAssaultRifle.SetActive(true);
                    leftShotgun.SetActive(false);
                    break;

                case 2:
                    leftConfirmWeapon.interactable = true;
                    leftWeaponNameTextBox.text = weaponNames[2];
                    leftWeaponTypeTextBox.text = weaponTypes[2];
                    leftWeaponMagazineSizeText.text = "12";
                    leftAssaultRifle.SetActive(false);
                    leftShotgun.SetActive(true);
                    leftMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    leftConfirmWeapon.interactable = true;
                    leftWeaponNameTextBox.text = weaponNames[3];
                    leftWeaponTypeTextBox.text = weaponTypes[3];
                    leftWeaponMagazineSizeText.text = "35";
                    leftShotgun.SetActive(false);
                    leftMarksmanRifle.SetActive(true);
                    leftSniperRifle.SetActive(false);
                    break;

                case 4:
                    leftWeaponNameTextBox.text = weaponNames[4];
                    leftWeaponTypeTextBox.text = weaponTypes[4];
                    leftWeaponMagazineSizeText.text = "10";
                    leftMarksmanRifle.SetActive(false);
                    leftSniperRifle.SetActive(true);
                    leftMinigun.SetActive(false);
                    break;

                case 5:
                    leftWeaponNameTextBox.text = weaponNames[5];
                    leftWeaponTypeTextBox.text = weaponTypes[5];
                    leftWeaponMagazineSizeText.text = "150";
                    leftSniperRifle.SetActive(false);
                    leftMinigun.SetActive(true);
                    leftShortSword.SetActive(false);
                    break;

                case 6:
                    leftWeaponNameTextBox.text = weaponNames[6];
                    leftWeaponTypeTextBox.text = weaponTypes[6];
                    leftWeaponMagazineSizeText.text = "---";
                    leftMinigun.SetActive(false);
                    leftShortSword.SetActive(true);
                    break;
            }
        }
    }

    public void LeftWeaponButtonBack() {
        if(leftWeaponNumber > 0 || leftWeaponNumber == 6) {
            leftWeaponNumber--;
            switch (leftWeaponNumber) {
                case 0:
                    leftWeaponNameTextBox.text = weaponNames[0];
                    leftWeaponTypeTextBox.text = weaponTypes[0];
                    leftWeaponMagazineSizeText.text = "100";
                    leftAssaultRifle.SetActive(false);
                    leftSubMachinegun.SetActive(true);
                    break;

                case 1:
                    leftConfirmWeapon.interactable = true;
                    leftWeaponNameTextBox.text = weaponNames[1];
                    leftWeaponTypeTextBox.text = weaponTypes[1];
                    leftWeaponMagazineSizeText.text = "50";
                    leftSubMachinegun.SetActive(false);
                    leftAssaultRifle.SetActive(true);
                    leftShotgun.SetActive(false);
                    break;

                case 2:
                    leftConfirmWeapon.interactable = true;
                    leftWeaponNameTextBox.text = weaponNames[2];
                    leftWeaponTypeTextBox.text = weaponTypes[2];
                    leftWeaponMagazineSizeText.text = "12";
                    leftAssaultRifle.SetActive(false);
                    leftShotgun.SetActive(true);
                    leftMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    leftWeaponNameTextBox.text = weaponNames[3];
                    leftWeaponTypeTextBox.text = weaponTypes[3];
                    leftWeaponMagazineSizeText.text = "35";
                    leftShotgun.SetActive(false);
                    leftMarksmanRifle.SetActive(true);
                    leftSniperRifle.SetActive(false);
                    break;

                case 4:
                    leftWeaponNameTextBox.text = weaponNames[4];
                    leftWeaponTypeTextBox.text = weaponTypes[4];
                    leftWeaponMagazineSizeText.text = "10";
                    leftMarksmanRifle.SetActive(false);
                    leftSniperRifle.SetActive(true);
                    leftMinigun.SetActive(false);
                    break;

                case 5:
                    leftWeaponNameTextBox.text = weaponNames[5];
                    leftWeaponTypeTextBox.text = weaponTypes[5];
                    leftWeaponMagazineSizeText.text = "150";
                    leftSniperRifle.SetActive(false);
                    leftMinigun.SetActive(true);
                    leftShortSword.SetActive(false);
                    break;

                case 6:
                    leftWeaponNameTextBox.text = weaponNames[6];
                    leftWeaponTypeTextBox.text = weaponTypes[6];
                    leftWeaponMagazineSizeText.text = "---";
                    leftMinigun.SetActive(false);
                    leftShortSword.SetActive(true);
                    break;
            }
        }
    }

    public void RightWeaponButtonNext() {
        if (rightWeaponNumber == 0 || rightWeaponNumber < 6) {
            rightWeaponNumber++;
            switch (rightWeaponNumber) {
                case 0:
                    rightWeaponNameTextBox.text = weaponNames[0];
                    rightWeaponTypeTextBox.text = weaponTypes[0];
                    rightWeaponMagazineSizeText.text = "100";
                    rightAssaultRifle.SetActive(false);
                    rightSubMachinegun.SetActive(true);
                    break;

                case 1:
                    rightWeaponNameTextBox.text = weaponNames[1];
                    rightWeaponTypeTextBox.text = weaponTypes[1];
                    rightWeaponMagazineSizeText.text = "50";
                    rightSubMachinegun.SetActive(false);
                    rightAssaultRifle.SetActive(true);
                    rightShotgun.SetActive(false);
                    break;

                case 2:
                    rightConfirmWeapon.interactable = true;
                    rightWeaponNameTextBox.text = weaponNames[2];
                    rightWeaponTypeTextBox.text = weaponTypes[2];
                    rightWeaponMagazineSizeText.text = "12";
                    rightAssaultRifle.SetActive(false);
                    rightShotgun.SetActive(true);
                    rightMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    rightConfirmWeapon.interactable = true;
                    rightWeaponNameTextBox.text = weaponNames[3];
                    rightWeaponTypeTextBox.text = weaponTypes[3];
                    rightWeaponMagazineSizeText.text = "35";
                    rightShotgun.SetActive(false);
                    rightMarksmanRifle.SetActive(true);
                    rightSniperRifle.SetActive(false);
                    break;

                case 4:
                    rightWeaponNameTextBox.text = weaponNames[4];
                    rightWeaponTypeTextBox.text = weaponTypes[4];
                    rightWeaponMagazineSizeText.text = "10";
                    rightMarksmanRifle.SetActive(false);
                    rightSniperRifle.SetActive(true);
                    rightMinigun.SetActive(false);
                    break;

                case 5:
                    rightWeaponNameTextBox.text = weaponNames[5];
                    rightWeaponTypeTextBox.text = weaponTypes[5];
                    rightWeaponMagazineSizeText.text = "150";
                    rightSniperRifle.SetActive(false);
                    rightMinigun.SetActive(true);
                    rightShortSword.SetActive(false);
                    break;

                case 6:
                    rightWeaponNameTextBox.text = weaponNames[6];
                    rightWeaponTypeTextBox.text = weaponTypes[6];
                    rightWeaponMagazineSizeText.text = "---";
                    rightShortSword.SetActive(true);
                    rightMinigun.SetActive(false);
                    break;
            }
        }
    }

    public void RightWeaponButtonBack() {
        if (rightWeaponNumber > 0 || rightWeaponNumber == 6) {
            rightWeaponNumber--;
            switch (rightWeaponNumber) {
                case 0:
                    rightWeaponNameTextBox.text = weaponNames[0];
                    rightWeaponTypeTextBox.text = weaponTypes[0];
                    rightWeaponMagazineSizeText.text = "100";
                    rightAssaultRifle.SetActive(false);
                    rightSubMachinegun.SetActive(true);
                    break;

                case 1:
                    rightConfirmWeapon.interactable = true;
                    rightWeaponNameTextBox.text = weaponNames[1];
                    rightWeaponTypeTextBox.text = weaponTypes[1];
                    rightWeaponMagazineSizeText.text = "50";
                    rightSubMachinegun.SetActive(false);
                    rightAssaultRifle.SetActive(true);
                    rightShotgun.SetActive(false);
                    break;

                case 2:
                    rightConfirmWeapon.interactable = true;
                    rightWeaponNameTextBox.text = weaponNames[2];
                    rightWeaponTypeTextBox.text = weaponTypes[2];
                    rightWeaponMagazineSizeText.text = "12";
                    rightAssaultRifle.SetActive(false);
                    rightShotgun.SetActive(true);
                    rightMarksmanRifle.SetActive(false);
                    break;

                case 3:
                    rightWeaponNameTextBox.text = weaponNames[3];
                    rightWeaponTypeTextBox.text = weaponTypes[3];
                    rightWeaponMagazineSizeText.text = "35";
                    rightShotgun.SetActive(false);
                    rightMarksmanRifle.SetActive(true);
                    rightSniperRifle.SetActive(false);
                    break;

                case 4:
                    rightWeaponNameTextBox.text = weaponNames[4];
                    rightWeaponTypeTextBox.text = weaponTypes[4];
                    rightWeaponMagazineSizeText.text = "10";
                    rightMarksmanRifle.SetActive(false);
                    rightSniperRifle.SetActive(true);
                    rightMinigun.SetActive(false);
                    break;

                case 5:
                    rightWeaponNameTextBox.text = weaponNames[5];
                    rightWeaponTypeTextBox.text = weaponTypes[5];
                    rightWeaponMagazineSizeText.text = "150";
                    rightSniperRifle.SetActive(false);
                    rightMinigun.SetActive(true);
                    rightShortSword.SetActive(false);
                    break;

                case 6:
                    rightWeaponNameTextBox.text = weaponNames[6];
                    rightWeaponTypeTextBox.text = weaponTypes[6];
                    rightWeaponMagazineSizeText.text = "---";
                    rightShortSword.SetActive(true);
                    rightMinigun.SetActive(false);
                    break;
            }
        }
    }

    public void SelectLeftWeapon() {
        selectedLeftWeaponNumber = leftWeaponNumber;
    }

    public void SelectRightWeapon() {
        selectedRightWeaponNumber = rightWeaponNumber;
    }

    public void LeftWeaponExtraAmmo() {
        if(leftExtraAmmoVariable == 1) {
            MMSC.frameSpeedModifier += 4;
            MMSC.frameWeightModifier -= 8;
            MMSC.frameFlightModifier += 5;
            leftExtraAmmoVariable = 0;
        } else if (leftExtraAmmoVariable == 0) {
            MMSC.frameSpeedModifier -= 4;
            MMSC.frameWeightModifier += 8;
            MMSC.frameFlightModifier -= 5;
            leftExtraAmmoVariable = 1;
        }
    }

    public void RightWeaponExtraAmmo() {
        if (rightExtraAmmoVariable == 1) {
            MMSC.frameSpeedModifier += 4;
            MMSC.frameWeightModifier -= 8;
            MMSC.frameFlightModifier += 5;
            rightExtraAmmoVariable = 0;
        } else if (rightExtraAmmoVariable == 0) {
            MMSC.frameSpeedModifier -= 4;
            MMSC.frameWeightModifier += 8;
            MMSC.frameFlightModifier -= 5;
            rightExtraAmmoVariable = 1;
        }
    }

    public void ShoulderWeaponSelectBack() {
        if (shoulderWeaponSpot > 1) {
            shoulderWeaponSpot--;
        }
    }

    public void ShoulderWeaponSelectNext() {
        if(shoulderWeaponSpot < 3) {
            shoulderWeaponSpot++;
        }
    }

    #endregion

    public void turnOnLights() {
        for(int i = 0; i<FrameLights.Length; i++) {
            FrameLights[i].SetActive(true);
        }
    }

    public void turnOffLights() {
        for (int i = 0; i < FrameLights.Length; i++) {
            FrameLights[i].SetActive(false);
        }
    }

    #region Settings Methods

    public void SettingsIntoVolume() {
        mainSettingsPanel.SetActive(false);
        volumeSettingsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(masterVolumeFocusObject);
    }

    public void SettingsIntoExtras() {
        mainSettingsPanel.SetActive(false);
        extraOptionsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(debugModeToggleFocusObject);
    }

    public void SettingsIntoMain() {
        mainSettingsPanel.SetActive(true);
        volumeSettingsPanel.SetActive(false);
        extraOptionsPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(settingsFocusObject);
    }

    public void changeMasterVolumeSlider() {
        audioMixer.SetFloat("Master", masterVolumeSlider.value);
    }

    public void changeSoundEffectVolumeSlider() {
        audioMixer.SetFloat("SoundsFX", soundEffectSlider.value);
    }

    public void changeMusicVolumeSlider() {
        audioMixer.SetFloat("Music", musicSlider.value);
    }

    public void OpenFeedbackForm() {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSefXhBTVAUjt1rDct9Kummh8I1lZuYUUv8cgaMV1WJOuIqPjw/viewform?usp=sf_link");
    }

    public void ResetGameProgressButtonWarning() {
        if(inResetProgressWarning == false) {
            inResetProgressWarning = true;
            eventSystem.SetSelectedGameObject(resetWarningPanelFocusObject);
            gameProgressResetWarning.SetActive(true);
        } else if(inResetProgressWarning == true) {
            inResetProgressWarning = false;
            eventSystem.SetSelectedGameObject(resetWarningFocusObject);
            gameProgressResetWarning.SetActive(true);
        }
    }

    public void ResetGame() {
        PlayerPrefs.SetInt("DebugMode", 0);
        PlayerPrefs.SetInt("Completed Tutorial", 0);
        PlayerPrefs.DeleteKey("Completed Mission 1");
        PlayerPrefs.DeleteKey("Completed Mission 2");
        PlayerPrefs.DeleteKey("Completed Mission 3");
        DeletePlayerValues();
        LoadScene(0);
    }

    public void DebugMode() {
        if(debugModeToggle.isOn == false){
            PlayerPrefs.SetInt("DebugMode", 0);
            Debug.Log(PlayerPrefs.GetInt("DebugMode"));
        } else if (debugModeToggle.isOn == true) {
            PlayerPrefs.SetInt("DebugMode", 1);
            Debug.Log(PlayerPrefs.GetInt("DebugMode"));
        }
    }

    #endregion

    public void ButtonClickFXPlay() {
        audioSource.PlayOneShot(buttonClickFX);
    }

    public void ButtonClickFXPlayTwo() {
        audioSource.PlayOneShot(buttonClickFX02);
    }

    public IEnumerator OpeningScreenFade() {
        yield return new WaitForSeconds(3.0f);
        alloyGamesSplashScreen.SetActive(false);
        yield return new WaitForSeconds(4.5f);
        openingFadeObject.SetActive(false);
        openingSceneItems.SetActive(true);
        canMoveCamera = true;
    }

    public void LoadScene( int SceneNumber) {
        StorePlayerValues();
        screenFader.SetActive(true);
        StartCoroutine(LoadLevelWithBar(SceneNumber));
        
    }

    IEnumerator LoadLevelWithBar(int LevelNumber) {
        async = SceneManager.LoadSceneAsync(LevelNumber);
        while (!async.isDone) {
            yield return null;
        }
    }

    public void DeletePlayerValues() {
        PlayerPrefs.SetInt("FrameChoice", 1);
        PlayerPrefs.SetInt("LeftWeaponChoice", 0);
        PlayerPrefs.DeleteKey("LeftExtraAmmoVariable");
        PlayerPrefs.SetInt("RightWeaponChoice", 0);
        PlayerPrefs.DeleteKey("RightExtraAmmoVariable");
        PlayerPrefs.SetInt("ShoulderWeaponChoice", 0);
        PlayerPrefs.DeleteKey("FrameExtraArmorVariable");
        PlayerPrefs.DeleteKey("FrameExtraFuelVariable");
        PlayerPrefs.DeleteKey("FrameIncreasedShieldCapactiy");
        PlayerPrefs.DeleteKey("FrameIncreasedScannerTime");
        PlayerPrefs.DeleteKey("AutoAimChoice");
    }

    public void StorePlayerValues() {
        PlayerPrefs.SetInt("FrameChoice", frameNumber);
        PlayerPrefs.SetInt("LeftWeaponChoice", selectedLeftWeaponNumber);
        PlayerPrefs.SetInt("LeftExtraAmmoVariable", leftExtraAmmoVariable);
        PlayerPrefs.SetInt("RightWeaponChoice", selectedRightWeaponNumber);
        PlayerPrefs.SetInt("RightExtraAmmoVariable", rightExtraAmmoVariable);
        PlayerPrefs.SetInt("ShoulderWeaponChoice", shoulderWeaponSpot);
        PlayerPrefs.SetInt("FrameExtraArmorVariable", frameExtraArmorModifier);
        PlayerPrefs.SetInt("FrameExtraFuelVariable", frameExtraFuelModifier);
        PlayerPrefs.SetInt("FrameIncreasedShieldCapactiy", frameIncreasedShieldCapacity);
        PlayerPrefs.SetInt("FrameIncreasedScannerTime", frameIncreasedScannerTime);
        PlayerPrefs.SetInt("AutoAimChoice", autoAimChoice);
        Debug.Log("Auto Aim is set to " + PlayerPrefs.GetInt("AutoAimChoice"));
    }

    public void CreditsButton() {
        LoadScene(6);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public IEnumerator CameraShakeMethod(float duration, float magnitude) {
        cameraShaking = true;
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsedTime = 0.0f;

        while(elapsedTime < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.localPosition = originalPos;
        cameraShaking = false;
    }
}
