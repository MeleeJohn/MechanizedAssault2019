using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlareEmitter_Test : MonoBehaviour {
    public GameManager GM;
    public PlayerController PC;
    [SerializeField]
    private int flareCount;
    public List<GameObject> flares = null;
    private bool flaring;
    private Vector3 flareDirection;
    //private float flareRechargeBaseNumber = 99;
    public TextMeshProUGUI flareCountText;
    private float flareRechargeAmount = 99;
    public Image flareEmitterBar;

    public List<GameObject> activeMissiles;

    // Use this for initialization
    void Start () {
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        flareCount = 5;
        flares = new List<GameObject>();
        activeMissiles = new List<GameObject>();
        foreach (GameObject flare in GameObject.FindGameObjectsWithTag("missileFlare")) {
            flares.Add(flare);
            flare.GetComponent<FlareObject>().PC = PC;
            flare.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        FlareEmitterBar();
        flareCountText.text = "FLR - 0" + flareCount;
        if (Input.GetKeyDown(KeyCode.B) && flaring == false && PC.canMove == true && flareCount != 0) {
            flareRechargeAmount = 0;
            flareCount--;
            flaring = true;
            StartCoroutine(FlareEmission());
        }

        if (GM.prevState.DPad.Right == XInputDotNetPure.ButtonState.Released && GM.state.DPad.Right == XInputDotNetPure.ButtonState.Pressed && flaring == false && PC.canMove == true && flareCount != 0) {
            flareRechargeAmount = 0;
            flareCount--;
            flaring = true;
            StartCoroutine(FlareEmission());
        }

        if (flareRechargeAmount < 50) {
            flareEmitterBar.color = new Color(1, 0, 0, 0.45f);
        } else if (flareRechargeAmount > 50 && flareRechargeAmount < 98) {
            flareEmitterBar.color = new Color(1, 1, 0, 0.45f);
        } else if(flareRechargeAmount >= 98) {
            flareEmitterBar.color = new Color(0,1,0,0.45f);
        }

        if(activeMissiles.Count == 0) {
            PC.missileTargeted = false;
        } else {
            PC.missileTargeted = true;
        }
    }

    private IEnumerator FlareEmission() {
        for (int i = 0; i < flares.Count; i++) {
            flares[i].SetActive(true);
            flares[i].transform.position = this.transform.position;
            flares[i].GetComponent<LensFlare>().enabled = true;
            flareDirection = new Vector3(Random.Range(-0.1f, 0.1f), 1, Random.Range(-0.1f, 0.1f));
            flares[i].GetComponent<Rigidbody>().AddForce(flareDirection * 40f, ForceMode.VelocityChange);
            StartCoroutine(flares[i].GetComponent<FlareObject>().SetActiveWait());
            yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));
            FlareMissiles();
        }

        yield return new WaitForSeconds(1.9f);
        StartCoroutine(FlareRecharge(0.02f));
    }

    private IEnumerator FlareRecharge(float waitTime) {
        while (flareRechargeAmount < 99) {
            flareRechargeAmount += 1f;
            yield return new WaitForSeconds(waitTime);

        }
        flaring = false;
    }

    public void FlareMissiles() {
        for (int i = 0; i < activeMissiles.Count; i++) {
            activeMissiles[i].GetComponent<EnemyMissile>().Flared(flares[i]);
            if(i == activeMissiles.Count && activeMissiles.Count > flares.Count) {
                i = 0;
            }
        }
    }


    public void FlareEmitterBar() {
        flareEmitterBar.fillAmount = FlareEmitterBarMap(flareRechargeAmount, 0,99 , 0, 1);
    }

    private float FlareEmitterBarMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
