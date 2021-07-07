 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunShot : MonoBehaviour
{
    public RailgunCharge RC;
    void Start() {

    }

    void Update() {
        Vector3 rayDir = GetComponent<Rigidbody>().velocity;
        Debug.DrawRay(transform.position, rayDir, Color.red);
        RaycastHit shotHit;
        if (Physics.Raycast(transform.position, rayDir, out shotHit, 100f)) {
            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                switch (RC) {
                    case RailgunCharge.Low:
                        shotHit.collider.GetComponent<Enemy>().Hit(150);
                        break;

                    case RailgunCharge.Normal:
                        shotHit.collider.GetComponent<Enemy>().Hit(190);
                        break;

                    case RailgunCharge.OverCharged:
                        shotHit.collider.GetComponent<Enemy>().Hit(240);
                        break;
                }
                this.gameObject.SetActive(false);
            } else if (shotHit.collider.tag == "Level Asset" || shotHit.collider.tag == "Ground") {
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other) {

    }

    public IEnumerator WaitTillInActive(float WaitTime) {
        yield return new WaitForSeconds(WaitTime);
        Debug.Log("Destroyed Wait till");
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        this.gameObject.SetActive(false);
    }
}