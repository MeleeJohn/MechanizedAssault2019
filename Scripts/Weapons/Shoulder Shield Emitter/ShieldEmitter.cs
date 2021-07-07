using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldEmitter : MonoBehaviour
{
    public GameManager GM;
    public PlayerController PC;
    public WeaponStatus WS = WeaponStatus.ReadytoFire;
    public GameObject ammoTextObject;
    public TextMeshProUGUI standardAmmoText;
    public GameObject shieldAmmoText;
    public GameObject overShieldUIObjects;
    public Image overShieldEmitterBar;
    public Image overShieldChargeBar;
    private float shieldLevel;
    public bool shieldEmpty;

    [Header("Colors")]
    public Color green;
    public Color yellow;
    public Color red;

    [Header("Particle Emitters")]
    public GameObject particleEmitter01;
    public GameObject particleEmitter02;
    void Start()
    {
        //standardAmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PC.ActivateObject(PC.shoulderWeaponLowAmmoNotice, 0);
        standardAmmoText.gameObject.SetActive(false);
        shieldAmmoText.GetComponent<TextMeshProUGUI>().text = "READY";
        shieldAmmoText.GetComponent<TextMeshProUGUI>().color = green;
        overShieldUIObjects.SetActive(true);
        shieldLevel = 200f;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.F) && PC.canMove == true && WS == WeaponStatus.ReadytoFire) {
            WS = WeaponStatus.Firing;
            StartCoroutine(EmitShield());
        }

        if (GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && PC.canMove == true && WS == WeaponStatus.ReadytoFire) {
            WS = WeaponStatus.Firing;
            StartCoroutine(EmitShield());
        }

        if (PC.overShields > 0) {
            OverShieldBar();
        }
        OverShieldTimerBar();
        
        if(PC.overShields <= 0 && WS == WeaponStatus.Firing) {
            DropShields();
        }
        
        /*if (PC.overShields <= 500 / 4 && PC.shoulderWeaponLowAmmoNotice.activeSelf == false && WS == WeaponStatus.Firing) {
            PC.ActivateObject(PC.shoulderWeaponLowAmmoNotice, 1);
            if (WS != WeaponStatus.Reloading) {
                PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CHARGING";
                PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<Text>().color = red;
            }
        }*/
    }

    public IEnumerator EmitShield() {
        yield return new WaitForSeconds(0f);
        shieldAmmoText.GetComponent<TextMeshProUGUI>().text = "EMITTING";
        shieldAmmoText.GetComponent<TextMeshProUGUI>().color = yellow;
        PC.overShields = 200f;
        overShieldEmitterBar.gameObject.SetActive(true);
        //overShieldUIObjects.SetActive(true);
        particleEmitter01.SetActive(true);
        particleEmitter02.SetActive(true);
        StartCoroutine(RunOverShield());
    }

    public void DropShields() {
        //overShieldUIObjects.SetActive(false);
        overShieldEmitterBar.gameObject.SetActive(false);
        particleEmitter01.SetActive(false);
        particleEmitter02.SetActive(false);
        shieldEmpty = true;
        StartCoroutine(ShieldCharging());
    }

    public void OverShieldBar() {
        overShieldEmitterBar.fillAmount = OverShieldBarFillMap(PC.overShields, 0, 200f, 0, 1);
        float overShieldAlpha = OverShieldBarFillMap(PC.overShields, 0, 200f, 0, 210f);
        //Debug.Log(overShieldAlpha);
        overShieldEmitterBar.color = new Color(203,0,255, overShieldAlpha);
    }

    private float OverShieldBarFillMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public IEnumerator RunOverShield() {
        while (shieldLevel > 0 && PC.overShields > 0) {
            shieldLevel -= 0.50f;
            yield return new WaitForSeconds(0.05f);
        }
        DropShields();
    }

    public void OverShieldTimerBar() {
        overShieldChargeBar.fillAmount = OverShieldTimerBarFillMap(shieldLevel, 0, 200f, 0, 1f);
    }

    private float OverShieldTimerBarFillMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public IEnumerator ShieldCharging() {
        WS = WeaponStatus.Reloading;
        /*PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CHARGING";
        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<Text>().color = red;*/

        shieldAmmoText.GetComponent<TextMeshProUGUI>().text = "CHARGING";
        shieldAmmoText.GetComponent<TextMeshProUGUI>().color = red;

        while (shieldLevel < 200) {
            shieldLevel += 1f;
            yield return new WaitForSeconds(0.05f);
        }
        WS = WeaponStatus.ReadytoFire;
        Debug.Log("READY ----- GREEN");
        shieldAmmoText.GetComponent<TextMeshProUGUI>().text = "READY";
        shieldAmmoText.GetComponent<TextMeshProUGUI>().color = green;
        shieldEmpty = false;
    }
}
