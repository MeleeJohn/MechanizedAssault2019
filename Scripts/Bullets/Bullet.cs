using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Damage;
    public GameObject Player;
    private float DistanceToPlayer = 0f;
    [Header("Weapon Range")]
    public float shortRange;
    public float mediumRange;
    public float longRange;
    public float outOfRange;

    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        DistanceToPlayer = Vector3.Distance(this.transform.position, Player.transform.position);
        Vector3 rayDir = GetComponent<Rigidbody>().velocity;
        Debug.DrawRay(transform.position, rayDir, Color.red);
        RaycastHit shotHit;

        if (Physics.Raycast(transform.position, rayDir, out shotHit, 20f))
        {
            if (shotHit.collider.tag == "Enemy"){
                #region Bullet Dropoff

                if(DistanceToPlayer < shortRange) {
                    CloseHit(Damage, shotHit);
                    Debug.Log("Destroyed close range");
                    /*this.GetComponent<TrailRenderer>().enabled = false;
                    this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    this.gameObject.transform.position = Vector3.zero;
                    this.gameObject.SetActive(false);*/
                    StartCoroutine(WaitInActive(0.0f));
                } else if(DistanceToPlayer >= shortRange && DistanceToPlayer < mediumRange) {
                    MediumHit(Damage, shotHit);
                    Debug.Log("Destroyed medium range");
                    /*this.GetComponent<TrailRenderer>().enabled = false;
                    this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    this.gameObject.transform.position = Vector3.zero;
                    this.gameObject.SetActive(false);*/
                    StartCoroutine(WaitInActive(0.0f));
                } else if (DistanceToPlayer >= mediumRange && DistanceToPlayer < longRange) {
                    LongRangeHit(Damage, shotHit);
                    Debug.Log("Destroyed long range");
                    /*this.GetComponent<TrailRenderer>().enabled = false;
                    this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    this.gameObject.transform.position = Vector3.zero;
                    this.gameObject.SetActive(false);*/
                    StartCoroutine(WaitInActive(0.0f));
                }
                #endregion
            }

            if (shotHit.collider.tag == "EnemyMissile") {
                if(shotHit.collider.GetComponent<EnemyMissile>().Dying == false){
                    Debug.Log("HIT THE MISSISLE");
                    StartCoroutine(shotHit.collider.GetComponent<EnemyMissile>().DestroyObject(0.0f));
                }
            }
        }
    }

    public void CloseHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage * 1.5f);
    }

    public void MediumHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage);
    }

    public void LongRangeHit(float Damage, RaycastHit shotHit) {
        shotHit.collider.GetComponent<Enemy>().Hit(Damage -0.5f);
    }

    public IEnumerator WaitInActive(float waitTime) {
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("BF Normalized bullet velocity" + this.GetComponent<Rigidbody>().velocity.magnitude);
        //Debug.Log("BF Normalized bullet Angular velocity" + this.GetComponent<Rigidbody>().angularVelocity.magnitude);
        yield return new WaitForSeconds(waitTime);
        this.GetComponent<TrailRenderer>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Normalized bullet velocity" + this.GetComponent<Rigidbody>().velocity.magnitude);
        //Debug.Log("Normalized bullet Angular velocity" + this.GetComponent<Rigidbody>().angularVelocity.magnitude);
        this.gameObject.transform.position = Vector3.zero;
        //this.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}