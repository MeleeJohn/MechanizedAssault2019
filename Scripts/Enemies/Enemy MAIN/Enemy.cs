using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public GameObject Player;
    public Transform playerCameraObj;
    //public Camera playerCamera;

    public bool isTrainingDummy;
    public bool Targeted;
    public bool Scanned;

    public float Health;
    public float baseHealth;
    public Slider HealthBar;
    public float damageCombo;
    public Text damageNumber;
    public bool isAlive;
    public bool damageNumberReset;
    public string enemyRank;
    public TutorialController TC;

    public GameObject redHUDElement;
    public GameObject yellowHUDElement;

    public GameObject[] damageItemsArray;
    void Start() {
        isAlive = true;
        Health = baseHealth;

    }

    void Update() {
        if (Player == null) {
            Player = GameObject.FindGameObjectWithTag("Player");
        } else {
            if (isTrainingDummy == false) {
                if (Targeted == true && GetComponent<Rover_Enemy>().isBoss == false) {
                    if (damageCombo < 10) {
                        Player.GetComponent<PlayerController>().enemyHitPoints.text = "00" + damageCombo;
                    } else if (damageCombo < 100 && damageCombo > 9) {
                        Player.GetComponent<PlayerController>().enemyHitPoints.text = "0" + damageCombo;
                    } else if (damageCombo > 99) {
                        Player.GetComponent<PlayerController>().enemyHitPoints.text = damageCombo.ToString();
                    }
                    Player.GetComponent<PlayerController>().enemyType.text = enemyRank;
                    if (damageNumberReset == false) {
                        damageNumberReset = true;
                        StartCoroutine(DamageNumberReset());
                    }
                    //redHUDElement.SetActive(true);
                    //yellowHUDElement.SetActive(false);
                } else if (GetComponent<Rover_Enemy>() != null && GetComponent<Rover_Enemy>().isBoss == true && Targeted == true) {
                    Player.GetComponent<PlayerController>().enemyHitPoints.text = "UNKOWN";

                } else {
                    //redHUDElement.SetActive(false);
                    //yellowHUDElement.SetActive(true);
                }
            } else if (isTrainingDummy == true) {
                if (Targeted == true) {
                    if (damageCombo < 10) {
                        Player.GetComponent<PlayerController>().enemyHitPoints.text = "00" + damageCombo;
                    } else if (damageCombo < 100 && damageCombo > 9) {
                        Player.GetComponent<PlayerController>().enemyHitPoints.text = "0" + damageCombo;
                    } else if (damageCombo > 99) {
                        Player.GetComponent<PlayerController>().enemyHitPoints.text = damageCombo.ToString();
                    }
                    Player.GetComponent<PlayerController>().enemyType.text = enemyRank;
                    if (damageNumberReset == false) {
                        damageNumberReset = true;
                        StartCoroutine(DamageNumberReset());
                    }
                    //redHUDElement.SetActive(true);
                    //yellowHUDElement.SetActive(false);
                } else if (GetComponent<Rover_Enemy>() != null && Targeted == true) {
                    Player.GetComponent<PlayerController>().enemyHitPoints.text = "UNKOWN";

                } else {
                    //redHUDElement.SetActive(false);
                    //yellowHUDElement.SetActive(true);
                }
            }
            if (isTrainingDummy == false) {
                if (GetComponent<Rover_Enemy>() != null && GetComponent<Rover_Enemy>().isBoss == true) {
                    DamageEffects();
                }
            }

            if (isTrainingDummy == true && Health <= 0) {
                TC.shotDownDrones++;
                if (Targeted == true) {
                    PlayerController PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                    PC.targetedLeftEnemy = null;
                    PC.targetedRightEnemy = null;
                    PC.Targeting = false;
                }
                isAlive = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void Hit(float Damage) {
        //Debug.Log("Enemy damage = " + Damage);
        damageCombo += Damage;
        //damageNumber.text = damageCombo.ToString();
        Health -= Damage;
        //HealthBar.value = Health;
    }

    private void DamageEffects() {
        if (Health < baseHealth - 450 && damageItemsArray[0].activeSelf == false) {
            damageItemsArray[0].SetActive(true);
        } else if (Health < baseHealth - 450 * 2 && damageItemsArray[1].activeSelf == false) {
            damageItemsArray[1].SetActive(true);
        } else if (Health < baseHealth - 450 * 3) {
            damageItemsArray[2].SetActive(true);
        } else if (Health < baseHealth - 450 * 4) {
            damageItemsArray[3].SetActive(true);
        } else if (Health < baseHealth - 450 * 5) {
            damageItemsArray[4].SetActive(true);
        } else if (Health < baseHealth - 450 * 6) {
            damageItemsArray[5].SetActive(true);
        }

    }

    private IEnumerator DamageNumberReset() {
        yield return new WaitForSeconds(2.0f);
        //Player.GetComponent<PlayerController>().enemyHitPoints.text = "000";
        damageCombo = 0;
        damageNumberReset = false;
    }
}
