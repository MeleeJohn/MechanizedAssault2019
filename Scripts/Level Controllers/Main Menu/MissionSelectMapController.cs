using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public enum MissionSelectCameraPosition {startingPosition, missionOne, missionTwo, missionThree, missionFour, missionFive, missionSix};

public class MissionSelectMapController : MonoBehaviour {

    [Header("Main Menu Controller")]
    public MainMenuController MMC;

    [Header("Mission Select")]
    public MissionSelectCameraPosition MSCP;
    public GameObject missionSelectGlobe;
    public Vector3 currentSelectedGlobeRotation;
    public GameObject currentMissionSelectCameraPosition;
    public GameObject startingCameraPosition;
    public bool missionConfirmUp;
    public GameObject missionConfirmationItems;

    public TextMeshProUGUI missionTitle;
    public int missionNumberSelect = -1;
    public Button missionStartButton;
    public TextMeshProUGUI missionDescription;
    public string currentText;
    public float textTypeDelay;
    public bool TypeTextWorking;
    public GameObject requiredDashFrame;

    [FoldoutGroup("Test Level and Tutorial Items")]
    public Vector3 tutroialTestLevelRotation;
    [FoldoutGroup("Test Level and Tutorial Items")]
    public string testLevelDescription;
    [FoldoutGroup("Test Level and Tutorial Items")]
    public string testLevelTitle;
    [FoldoutGroup("Test Level and Tutorial Items")]
    public string tutorialDescription;
    [FoldoutGroup("Test Level and Tutorial Items")]
    public string tutorialTitle;

    [FoldoutGroup("Mission One Items")]
    public Vector3 missionOneRotation;
    [FoldoutGroup("Mission One Items")]
    public bool missionOneHovering;
    [FoldoutGroup("Mission One Items")]
    public string missionOneDescription;
    [FoldoutGroup("Mission One Items")]
    public string missionOneTitle;

    [FoldoutGroup("Mission Two Items")]
    public Vector3 missionTwoRotation;
    [FoldoutGroup("Mission Two Items")]
    public bool missionTwoHovering;
    [FoldoutGroup("Mission Two Items")]
    public string missionTwoDescription;
    [FoldoutGroup("Mission Two Items")]
    public string missionTwoTitle;

    [FoldoutGroup("Mission Three Items")]
    public Vector3 missionThreeRotation;
    [FoldoutGroup("Mission Three Items")]
    public bool missionThreeHovering;
    [FoldoutGroup("Mission Three Items")]
    public string missionThreeDescription;
    [FoldoutGroup("Mission Three Items")]
    public string missionThreeTitle;

    [FoldoutGroup("Mission Four Items")]
    public Vector3 missionFourRotation;
    [FoldoutGroup("Mission Four Items")]
    public bool missionFourHovering;
    [FoldoutGroup("Mission Four Items")]
    public string missionFourDescription;
    [FoldoutGroup("Mission Four Items")]
    public string missionFourTitle;

    [FoldoutGroup("Mission Five Items")]
    public Vector3 missionFiveRotation;
    [FoldoutGroup("Mission Five Items")]
    public bool missionFiveHovering;
    [FoldoutGroup("Mission Five Items")]
    public string missionFiveDescription;
    [FoldoutGroup("Mission Five Items")]
    public string missionFiveTitle;

    [FoldoutGroup("Mission Six Items")]
    public Vector3 missionSixRotation;
    [FoldoutGroup("Mission Six Items")]
    public bool missionSixHovering;
    [FoldoutGroup("Mission Six Items")]
    public string missionSixDescription;
    [FoldoutGroup("Mission Six Items")]
    public string missionSixTitle;

    [FoldoutGroup("Mission Seven Items")]
    public Vector3 missionSevenRotation;
    [FoldoutGroup("Mission Seven Items")]
    public bool missionSevenHovering;
    [FoldoutGroup("Mission Seven Items")]
    public string missionSevenDescription;
    [FoldoutGroup("Mission Seven Items")]
    public string missionSevenTitle;

    void Update () {

        missionSelectGlobe.transform.rotation = Quaternion.Lerp(missionSelectGlobe.transform.rotation, Quaternion.Euler(currentSelectedGlobeRotation), 0.07f);
        switch (missionNumberSelect) {
            case -2:
                currentSelectedGlobeRotation = tutroialTestLevelRotation;
                missionTitle.text = testLevelTitle;
                //missionDescription.text = missionOneDescription;
                if (TypeTextWorking == false){
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(testLevelDescription, missionDescription));
                }
                missionStartButton.interactable = true;
                requiredDashFrame.SetActive(false);
                break;

            case -1:
                currentSelectedGlobeRotation = tutroialTestLevelRotation;
                missionTitle.text = tutorialTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(tutorialDescription, missionDescription));
                }
                //missionDescription.text = missionOneDescription;
                missionStartButton.interactable = true;
                requiredDashFrame.SetActive(true);
                break;

            case 0:
                currentSelectedGlobeRotation = missionOneRotation;
                missionTitle.text = missionOneTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionOneDescription, missionDescription));
                }
                requiredDashFrame.SetActive(true);
                //missionDescription.text = missionOneDescription;
                missionStartButton.interactable = true;
                break;

            case 1:
                currentSelectedGlobeRotation = missionTwoRotation;
                missionTitle.text = missionTwoTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionTwoDescription, missionDescription));
                }
                requiredDashFrame.SetActive(false);
                //missionDescription.text = missionTwoDescription;

                if (PlayerPrefs.GetInt("Completed Mission 1") == 1){
                    missionStartButton.interactable = true;
                } else {
                    missionStartButton.interactable = false;
                }
                //missionStartButton.interactable = true;
                break;

            case 2:
                currentSelectedGlobeRotation = missionThreeRotation;
                missionTitle.text = missionThreeTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionThreeDescription, missionDescription));
                }
                //missionDescription.text = missionThreeDescription;
                if (PlayerPrefs.GetInt("Completed Mission 2") == 1) {
                    missionStartButton.interactable = true;
                } else {
                    missionStartButton.interactable = true;
                }
                //missionStartButton.interactable = true;
                break;

            case 3:
                currentSelectedGlobeRotation = missionFourRotation;
                missionTitle.text = missionFourTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionFourDescription, missionDescription));
                }
                //missionDescription.text = missionFourDescription;
                missionStartButton.interactable = false;
                break;

            case 4:
                currentSelectedGlobeRotation = missionFiveRotation;
                missionTitle.text = missionFiveTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionFiveDescription, missionDescription));
                }
                //missionDescription.text = missionFiveDescription;
                missionStartButton.interactable = false;
                break;

            case 5:
                currentSelectedGlobeRotation = missionSixRotation;
                missionTitle.text = missionSixTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionSixDescription, missionDescription));
                }
                //missionDescription.text = missionSixDescription;
                missionStartButton.interactable = false;
                break;

            case 6:
                currentSelectedGlobeRotation = missionSevenRotation;
                missionTitle.text = missionSevenTitle;
                if (TypeTextWorking == false) {
                    missionDescription.text = "";
                    TypeTextWorking = true;
                    StartCoroutine(TypeText(missionSevenDescription, missionDescription));
                }
                //missionDescription.text = missionSevenDescription;
                missionStartButton.interactable = false;
                break;
        }
    }

    public void MissionSelectNextButton() {
        if(missionNumberSelect == -2 || missionNumberSelect < 6) {
            missionNumberSelect++;
        } else if (missionNumberSelect == 5){
            missionNumberSelect = 0;
        }
        if (TypeTextWorking == true) {
            missionDescription.text = "";
            TypeTextWorking = false;
        }
    }

    public void MissionSelectBackButton() {
        if (missionNumberSelect > -2 || missionNumberSelect == 6) {
            missionNumberSelect--;
        } else if (missionNumberSelect == -2) {
            missionNumberSelect = 5;
        }
        if (TypeTextWorking == true) {
            missionDescription.text = "";
            TypeTextWorking = false;
        }
    }

    public void SelectMission() {
        //if(missionConfirmUp == true){
            switch (missionNumberSelect) {

                case -2:
                    TestSceneSelect();
                    break;

                case -1:
                    TutorialSelect();
                    break;

                case 0:
                    MissionOneSelect();
                    break;

                case 1:
                    MissionTwoSelect();
                    break;

                case 2:
                    MissionThreeSelect();
                    break;

                case 3:
                    MissionFourSelect();
                    break;

                case 4:
                    MissionFiveSelect();
                    break;

                case 5:
                    MissionSixSelect();
                    break;

                case 6:
                    MissionSixSelect();
                    break;
            }
        /*} else if(missionConfirmUp == false) {
            missionConfirmationItems.SetActive(true);
            missionStartButton.interactable = false;
            missionConfirmUp = true;
        }*/
    }

    public void MissionConfirmBackButton() {
        missionConfirmUp = false;
        missionConfirmationItems.SetActive(false);
        missionStartButton.interactable = true;
    }

    public void MissionSelectUnHover() {
        MSCP = MissionSelectCameraPosition.startingPosition;
    }

    public void MissionOneSelectHover() {
        if(missionOneHovering != true){
        MSCP = MissionSelectCameraPosition.missionOne;
        missionOneHovering = true;
        } else if(missionOneHovering == true){
            MissionSelectUnHover();
            missionOneHovering = false;
        }
    }

    public void TestSceneSelect() {
        //PlayerPrefs.SetInt("Selected Mission",1);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(5);
    }

    public void TutorialSelect() {
        //PlayerPrefs.SetInt("Selected Mission",1);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(4);
    }

    public void MissionOneSelect() {
        //PlayerPrefs.SetInt("Selected Mission",1);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(1);
    }

    public void MissionTwoSelectHover() {
        if (missionTwoHovering != true) {
            MSCP = MissionSelectCameraPosition.missionTwo;
            missionTwoHovering = true;
        } else if (missionTwoHovering == true) {
            MissionSelectUnHover();
            missionTwoHovering = false;
        }
    }

    public void MissionTwoSelect() {
        //PlayerPrefs.SetInt("Selected Mission", 2);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(2);
    }

    public void MissionThreeSelectHover() {
        if (missionThreeHovering != true) {
            MSCP = MissionSelectCameraPosition.missionThree;
            missionThreeHovering = true;
        } else if (missionThreeHovering == true) {
            MissionSelectUnHover();
            missionThreeHovering = false;
        }
    }

    public void MissionThreeSelect() {
        //PlayerPrefs.SetInt("Selected Mission", 3);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(3);
    }

    public void MissionFourSelectHover() {
        if (missionFourHovering != true) {
            MSCP = MissionSelectCameraPosition.missionFour;
            missionFourHovering = true;
        } else if (missionFourHovering == true) {
            MissionSelectUnHover();
            missionFourHovering = false;
        }
    }

    public void MissionFourSelect() {
        //PlayerPrefs.SetInt("Selected Mission", 4);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(4);
    }

    public void MissionFiveSelectHover() {
        if (missionFiveHovering != true) {
            MSCP = MissionSelectCameraPosition.missionFive;
            missionFiveHovering = true;
        } else if (missionFiveHovering == true) {
            MissionSelectUnHover();
            missionFiveHovering = false;
        }
    }

    public void MissionFiveSelect() {
        //PlayerPrefs.SetInt("Selected Mission", 5);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(5);
    }

    public void MissionSixSelectHover() {
        if (missionSixHovering != true) {
            MSCP = MissionSelectCameraPosition.missionSix;
            missionSixHovering = true;
        } else if (missionSixHovering == true) {
            MissionSelectUnHover();
            missionSixHovering = false;
        }
    }

    public void MissionSixSelect() {
        //PlayerPrefs.SetInt("Selected Mission", 5);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(6);
    }

    public void MissionSevenSelect() {
        //PlayerPrefs.SetInt("Selected Mission", 5);
        //MMC.WeaponFrameNextButton();
        MMC.LoadScene(7);
    }

    public IEnumerator TypeText(string stringInput, TextMeshProUGUI textMeshText) {
        textMeshText.gameObject.SetActive(true);
        for (int i = 0; i < stringInput.Length+1; i++) {
            currentText = stringInput.Substring(0, i);
            missionDescription.text = currentText;
            yield return new WaitForSeconds(textTypeDelay);
        }
    }
}
