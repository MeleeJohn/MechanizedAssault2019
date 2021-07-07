using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionBoundingBox : MonoBehaviour
{

    public GameObject boundingBoxCanvas;

    [Header("Timer Properties")]
    public float timerBaseValue;
    private float timerValue;
    public TextMeshProUGUI timerText;
    public bool runningTimer;

    [Header("Player Properties")]
    public PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerValue <= 0 && runningTimer == true) {
            runningTimer = false;
            Debug.Log("Timer == 0");
            //StartCoroutine(PC.DeathMethod());
            TimeOut();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player" && runningTimer == false) {
            timerValue = timerBaseValue;
            boundingBoxCanvas.SetActive(true);
            PC = other.gameObject.GetComponent<PlayerController>();
            runningTimer = true;
            StartCoroutine(TimerCountdown());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && runningTimer == true) {
            runningTimer = false;
            TimerReset();
        }
    }

    private IEnumerator TimerCountdown() {
        timerText.text = timerValue.ToString() + " secs";
        if(runningTimer == true) {
            timerValue -= 1.0f;
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(TimerCountdown());
        }
    }

    private void TimerReset(){
        boundingBoxCanvas.SetActive(false);
        timerValue = timerBaseValue;
    }

    private void TimeOut() {
        GameObject levelController = GameObject.FindGameObjectWithTag("Level Controller");

        if (levelController.GetComponent<TestController>() != null) {
            StartCoroutine(PC.DeathMethod());
        }

        if (levelController.GetComponent<TutorialController>() != null) {
            StartCoroutine(levelController.GetComponent<TutorialController>().ResetPlayer());
            runningTimer = false;
            TimerReset();
        }

        if (levelController.GetComponent<LevelOneController>() != null) {
            StartCoroutine(PC.DeathMethod());
        }

        if (levelController.GetComponent<LevelTwoController>() != null) {
            StartCoroutine(PC.DeathMethod());
        }

        if (levelController.GetComponent<LevelThreeController>() != null) {
            StartCoroutine(PC.DeathMethod());
        }
    }
}
