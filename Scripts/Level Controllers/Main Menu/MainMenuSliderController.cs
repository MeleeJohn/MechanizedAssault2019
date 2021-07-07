    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuSliderController : MonoBehaviour {

    public MainMenuController MMC;
    public FrameSelection FS;

    [Header("Frame Stat Boxes")]
    public Text frameArmorTextbox;

    [Header("Frame Aiming type")]
    public Toggle aimingTypeToggle;
    public TextMeshProUGUI aimingTypeDescription;

    [Header("Slider Colors")]
    public Color Green;
    public Color Blue;
    public Color Red;

    [Header("Frame Sliders")]
    public Slider frameArmorSlider;
    public int frameArmorNumber;
    public Slider frameSpeedSlider;
    public int frameSpeedNumber;
    public Slider frameWeightSlider;
    public int frameWeightNumber;
    public Slider frameFlightSlider;
    public int frameFlightNumber;

    [Header("Frame Modifier Numbers")]
    public int frameArmorModifier;
    public int frameSpeedModifier;
    public int frameWeightModifier;
    public int frameFlightModifier;

    [Header("Left Weapon Sliders")]
    public Slider leftDamageSlider;
    public Slider leftRateOfFireSlider;
    public Slider leftRangeSlider;

    [Header("Right Weapon Sliders")]
    public Slider rightDamageSlider;
    public Slider rightRateOfFireSlider;
    public Slider rightRangeSlider;

    [Header("Shoulder Sliders")]
    public Slider shoulderDamageSlider;
    public Slider shoulderRateOfFireSlider;
    public Slider shoulderRangeSlider;
    public Slider shoulderChargeTimeSlider;

    void Start () {
		
	}
	
	void Update () {
        FS = MMC.FS;

        switch (MMC.leftWeaponNumber) {
            case 0:
                leftDamageSlider.value = 20;
                leftRateOfFireSlider.value = 80;
                leftRangeSlider.value = 25;
                break;

            case 1:
                leftDamageSlider.value = 25;
                leftRateOfFireSlider.value = 40;
                leftRangeSlider.value = 35;
                break;

            case 2:
                leftDamageSlider.value = 30;
                leftRateOfFireSlider.value = 40;
                leftRangeSlider.value = 32;

                break;

            case 3:
                leftDamageSlider.value = 60;
                leftRateOfFireSlider.value = 25;
                leftRangeSlider.value = 70;

                break;

            case 4:
                leftDamageSlider.value = 90;
                leftRateOfFireSlider.value = 15;
                leftRangeSlider.value = 90;

                break;

            case 5:
                leftDamageSlider.value = 22;
                leftRateOfFireSlider.value = 65;
                leftRangeSlider.value = 35;

                break;

            case 6:
                leftDamageSlider.value = 65;
                leftRateOfFireSlider.value = 45;
                leftRangeSlider.value = 10;

                break;
        }

        switch (MMC.rightWeaponNumber) {
            case 0:
                rightDamageSlider.value = 20;
                rightRateOfFireSlider.value = 80;
                rightRangeSlider.value = 25;
                break;

            case 1:
                rightDamageSlider.value = 25;
                rightRateOfFireSlider.value = 40;
                rightRangeSlider.value = 35;

                break;

            case 2:
                rightDamageSlider.value = 30;
                rightRateOfFireSlider.value = 40;
                rightRangeSlider.value = 32;

                break;

            case 3:
                rightDamageSlider.value = 60;
                rightRateOfFireSlider.value = 25;
                rightRangeSlider.value = 70;

                break;

            case 4:
                rightDamageSlider.value = 90;
                rightRateOfFireSlider.value = 15;
                rightRangeSlider.value = 90;

                break;

            case 5:
                rightDamageSlider.value = 22;
                rightRateOfFireSlider.value = 65;
                rightRangeSlider.value = 35;

                break;

            case 6:
                rightDamageSlider.value = 65;
                rightRateOfFireSlider.value = 45;
                rightRangeSlider.value = 10;

                break;
        }

        switch (FS) {

            case FrameSelection.DASH:
                frameArmorTextbox.text = "1750";
                frameArmorSlider.value = 40 + frameArmorModifier;
                frameSpeedSlider.value = 85 + frameSpeedModifier;
                frameWeightSlider.value = 45 + frameWeightModifier;
                frameFlightSlider.value = 80 + frameFlightModifier;

                switch (MMC.shoulderWeaponSpot) {
                    case 1:
                        shoulderDamageSlider.value = 50f;
                        shoulderRangeSlider.value = 75f;
                        shoulderRateOfFireSlider.value = 40f;
                        shoulderChargeTimeSlider.value = 0f;
                        break;

                    case 2:
                        shoulderDamageSlider.value = 40f;
                        shoulderRangeSlider.value = 75f;
                        shoulderRateOfFireSlider.value = 80f;
                        shoulderChargeTimeSlider.value = 0f;
                        break;
                }

                break;

            case FrameSelection.Assault:
                frameArmorTextbox.text = "2000";
                frameArmorSlider.value = 60 + frameArmorModifier;
                frameSpeedSlider.value = 65 + frameSpeedModifier;
                frameWeightSlider.value = 59 + frameWeightModifier;
                frameFlightSlider.value = 60 + frameFlightModifier;

                switch (MMC.shoulderWeaponSpot) {
                    case 1:
                        shoulderDamageSlider.value = 63f;
                        shoulderRangeSlider.value = 60f;
                        shoulderRateOfFireSlider.value = 35f;
                        shoulderChargeTimeSlider.value = 0f;
                        break;

                    case 2:
                        shoulderDamageSlider.value = 43f;
                        shoulderRangeSlider.value = 68f;
                        shoulderRateOfFireSlider.value = 60f;
                        shoulderChargeTimeSlider.value = 0f;
                        break;
                }

                break;

            case FrameSelection.Titan:
                frameArmorTextbox.text = "2500";
                frameArmorSlider.value = 80 + frameArmorModifier;
                frameSpeedSlider.value = 45 + frameSpeedModifier;
                frameWeightSlider.value = 75 + frameWeightModifier;
                frameFlightSlider.value = 40 + frameFlightModifier;

                switch (MMC.shoulderWeaponSpot) {
                    case 1:

                        break;

                    case 2:

                        break;
                }

                break;
        }

        if (aimingTypeToggle.isOn == false) {
            aimingTypeToggle.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "MANUAL";
            aimingTypeDescription.text = "The pilot must press the lockon button while an enemy is within the reticle area on screen.";
            MMC.autoAimChoice = 0;
        } else if (aimingTypeToggle.isOn == true) {
            aimingTypeToggle.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "AUTOMATIC";
            aimingTypeDescription.text = "The frame will automatically lockon to an enemy that enters the reticle area on screen.";
            MMC.autoAimChoice = 1;
        }
    }
}
