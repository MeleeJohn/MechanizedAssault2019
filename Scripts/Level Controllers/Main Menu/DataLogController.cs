using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class DataLogController : MonoBehaviour
{
    [Header("EventSystem")]
    public EventSystem eventSystem;
    [Header("Data Log Canvas")]
    public GameObject dataLogCanvas;
    public TextMeshProUGUI viewportTextField;
    [Header("Top Level Buttons")]
    public GameObject connectionButton;
    public GameObject connectionButtonFocus;
    public GameObject topLevelButtons;
    public GameObject topLevelButtonFocus;
    [Header("Data screen Buttons")]
    public Scrollbar verticalScrollBar;
    public Navigation verticlaScrollBarNavigation;
    [Header("Frame Buttons")]
    public GameObject frameButtons;
    public GameObject frameButtonFocus;
    [Header("Weapon Buttons")]
    public GameObject weaponButtons;
    public GameObject weaponButtonFocus;
    public GameObject mainWeaponButton;
    public GameObject shoulderWeaponButton;
    [Header("Main Weapon Buttons")]
    public GameObject mainWeaponButtons;
    public GameObject mainWeaponButtonFocus;
    [Header("Shoulder Weapon Buttons")]
    public GameObject shoulderWeaponButtons;
    public GameObject shoulderWeaponButtonFocus;
    [Header("Enemy Buttons")]
    public GameObject enemyButtons;
    public GameObject enemyButtonFocus;
    [Header("Mission Buttons")]
    public GameObject missionButtons;
    public GameObject missionButtonFocus;

    #region Frame Info Region
    [FoldoutGroup("Frame Info")]
    [Header("Dash Frame Info")]
    [TextArea]
    public string dashFrameInfo;
    [FoldoutGroup("Frame Info")]
    [Header("Assault Frame Info")]
    [TextArea]
    public string assaultFrameInfo;
    [FoldoutGroup("Frame Info")]
    [Header("Titan Frame Info")]
    [TextArea]
    public string titanFrameInfo;
    #endregion

    #region Enemy Info Region
    [FoldoutGroup("Enemy Info")]
    [Header("Rover Info")]
    [TextArea]
    public string roverInfo;
    [FoldoutGroup("Enemy Info")]
    [Header("Rhino Frame Info")]
    [TextArea]
    public string rhinoFrameInfo;
    [FoldoutGroup("Enemy Info")]
    [Header("Rhino Boss Frame Info")]
    [TextArea]
    public string rhinoBossFrameInfo;
    [FoldoutGroup("Enemy Info")]
    [Header("Magnus Frame Info")]
    [TextArea]
    public string magnusFrameInfo;
    #endregion

    #region Main Weapon Info Region
    [FoldoutGroup("Main Weapon Info")]
    [Header("SMG Info")]
    [TextArea]
    public string smgInfo;
    [FoldoutGroup("Main Weapon Info")]
    [Header("Assault Rifle Info")]
    [TextArea]
    public string assaultRifleInfo;
    [FoldoutGroup("Main Weapon Info")]
    [Header("Shotgun Info")]
    [TextArea]
    public string shotgunInfo;
    [FoldoutGroup("Main Weapon Info")]
    [Header("Marksman Rifle Info")]
    [TextArea]
    public string marksmanRifleInfo;
    [FoldoutGroup("Main Weapon Info")]
    [Header("Sniper Rifle Info")]
    [TextArea]
    public string sniperRifleInfo;
    [FoldoutGroup("Main Weapon Info")]
    [Header("Minigun Info")]
    [TextArea]
    public string minigunInfo;
    [FoldoutGroup("Main Weapon Info")]
    [Header("Short Sword Info")]
    [TextArea]
    public string shortSwordInfo;
    #endregion

    #region Shoulder Weapon Info
    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Missile Launcher Info")]
    [TextArea]
    public string missileLauncherInfo;

    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Anime Missile Launcher Info")]
    [TextArea]
    public string animeMissileLauncherInfo;

    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Grenade Launcher Info")]
    [TextArea]
    public string grenadeLauncherInfo;

    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Stryker Cannon Info")]
    [TextArea]
    public string strykerCannonInfo;

    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Shoulder Cannon Info")]
    [TextArea]
    public string shoulderCannonInfo;

    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Railgun Info")]
    [TextArea]
    public string railgunInfo;

    [FoldoutGroup("Shoulder Weapon Info")]
    [Header("Shield Emitter Info")]
    [TextArea]
    public string shieldEmitterInfo;

    #endregion

    #region Mission Info
    [FoldoutGroup("Mission Info")]
    [Header("Mission One Info")]
    [TextArea]
    public string missionOneInfo;

    [FoldoutGroup("Mission Info")]
    [Header("Mission Two Info")]
    [TextArea]
    public string missionTwoInfo;

    [FoldoutGroup("Mission Info")]
    [Header("Mission Three Info")]
    [TextArea]
    public string missionThreeInfo;
    #endregion
    [Header("In-Menu Variables")]
    public bool inDataLog;
    public int dataPositionNumber;

    void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        Navigation verticlaScrollBarNavigation = verticalScrollBar.navigation;
        verticlaScrollBarNavigation.mode = Navigation.Mode.Explicit;
    }

    void Update()
    {
        
    }

    public void OpenDataLog() {
        if (inDataLog == false) {
            inDataLog = true;
            connectionButton.SetActive(!inDataLog);
            eventSystem.SetSelectedGameObject(topLevelButtonFocus);
            dataLogCanvas.SetActive(inDataLog);
        } else {
            inDataLog = false;
            connectionButton.SetActive(!inDataLog);
            eventSystem.SetSelectedGameObject(connectionButtonFocus);
            dataLogCanvas.SetActive(inDataLog);
        }
    }
    #region OpenSectionButtons
    public void OpenFrameButtons() {
        dataPositionNumber = 2;
        eventSystem.SetSelectedGameObject(frameButtonFocus);
        ChooseDataPosition();
    }

    public void OpenWeaponButtons() {
        dataPositionNumber = 3;
        eventSystem.SetSelectedGameObject(weaponButtonFocus);
        ChooseDataPosition();
    }

    public void OpenEnemyButtons() {
        dataPositionNumber = 4;
        eventSystem.SetSelectedGameObject(enemyButtonFocus);
        ChooseDataPosition();
    }

    public void OpenMissionButtons() {
        dataPositionNumber = 5;
        eventSystem.SetSelectedGameObject(missionButtonFocus);
        ChooseDataPosition();
    }

    public void OpenMainWeaponButtons() {
        dataPositionNumber = 6;
        eventSystem.SetSelectedGameObject(mainWeaponButtonFocus);
        ChooseDataPosition();
    }

    public void OpenShoulderWeaponButtons() {
        dataPositionNumber = 7;
        eventSystem.SetSelectedGameObject(shoulderWeaponButtonFocus);
        ChooseDataPosition();
    }
    #endregion

    #region FrameLogButtons
    public void DashFrameInfo() {
        viewportTextField.text = dashFrameInfo;
    }
    public void AssaultFrameInfo() {
        viewportTextField.text = assaultFrameInfo;
    }
    public void TitanFrameInfo() {
        viewportTextField.text = titanFrameInfo;
    }
    #endregion

    #region EnemyLogButtons
    public void RoverEnemyInfo() {
        viewportTextField.text = roverInfo;
    }
    public void RhinoEnemyInfo() {
        viewportTextField.text = rhinoFrameInfo;
    }
    public void RhinoBossEnemyInfo() {
        viewportTextField.text = rhinoBossFrameInfo;
    }
    public void MagnusEnemyInfo() {
        viewportTextField.text = magnusFrameInfo;
    }
    #endregion

    #region WeaponLogButtons

    #endregion

    #region MissionLogButtons

    #endregion

    public void BackButton() {
        /*if(dataPositionNumber != 1){
            dataPositionNumber = 1;
            ChooseDataPosition();
            eventSystem.SetSelectedGameObject(topLevelButtonFocus);
        }*/
        switch (dataPositionNumber) {
            case 2:
                dataPositionNumber = 1;
                ChooseDataPosition();
                eventSystem.SetSelectedGameObject(topLevelButtonFocus);
                break;

            case 3:
                dataPositionNumber = 1;
                ChooseDataPosition();
                eventSystem.SetSelectedGameObject(topLevelButtonFocus);
                break;

            case 4:
                dataPositionNumber = 1;
                ChooseDataPosition();
                eventSystem.SetSelectedGameObject(topLevelButtonFocus);
                break;

            case 5:
                dataPositionNumber = 1;
                ChooseDataPosition();
                eventSystem.SetSelectedGameObject(topLevelButtonFocus);
                break;

            case 6:
                dataPositionNumber = 3;
                ChooseDataPosition();
                eventSystem.SetSelectedGameObject(weaponButtonFocus);
                break;

            case 7:
                dataPositionNumber = 3;
                ChooseDataPosition();
                eventSystem.SetSelectedGameObject(weaponButtonFocus);
                break;


        }
    }

    public void ChooseDataPosition() {
        topLevelButtons.SetActive(false);
        frameButtons.SetActive(false);
        weaponButtons.SetActive(false);
        enemyButtons.SetActive(false);
        missionButtons.SetActive(false);
        mainWeaponButtons.SetActive(false);
        shoulderWeaponButtons.SetActive(false);
        switch (dataPositionNumber) {
            case 1:
                topLevelButtons.SetActive(true);
                break;

            case 2:
                frameButtons.SetActive(true);
                verticlaScrollBarNavigation.selectOnUp = frameButtonFocus.GetComponent<Button>();
                verticalScrollBar.navigation = verticlaScrollBarNavigation;
                break;

            case 3:
                weaponButtons.SetActive(true);
                //verticlaScrollBarNavigation.selectOnUp = weaponButtonFocus.GetComponent<Button>();
                //verticalScrollBar.navigation = verticlaScrollBarNavigation;
                break;

            case 4:
                enemyButtons.SetActive(true);
                verticlaScrollBarNavigation.selectOnUp = enemyButtonFocus.GetComponent<Button>();
                verticalScrollBar.navigation = verticlaScrollBarNavigation;
                break;

            case 5:
                missionButtons.SetActive(true);
                verticlaScrollBarNavigation.selectOnUp = missionButtonFocus.GetComponent<Button>();
                verticalScrollBar.navigation = verticlaScrollBarNavigation;
                break;

            case 6:
                mainWeaponButtons.SetActive(true);
                verticlaScrollBarNavigation.selectOnUp = mainWeaponButtonFocus.GetComponent<Button>();
                verticalScrollBar.navigation = verticlaScrollBarNavigation;
                break;

            case 7:
                shoulderWeaponButtons.SetActive(true);
                verticlaScrollBarNavigation.selectOnUp = shoulderWeaponButtonFocus.GetComponent<Button>();
                verticalScrollBar.navigation = verticlaScrollBarNavigation;
                break;
        }
    }
}
