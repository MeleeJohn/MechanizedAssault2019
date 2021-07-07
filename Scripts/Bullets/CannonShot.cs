using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShot : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        Vector3 rayDir = GetComponent<Rigidbody>().velocity;
        Debug.DrawRay (transform.position, rayDir, Color.red);
		RaycastHit shotHit;
		if(Physics.Raycast(transform.position, rayDir, out shotHit,100f)){
			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
				shotHit.collider.GetComponent<Enemy> ().Hit (100);
                this.gameObject.SetActive(false);
			} else if (shotHit.collider.tag == "Level Asset" || shotHit.collider.tag == "Ground") {
                this.gameObject.SetActive(false);
            }
		}
	}

	void OnTriggerEnter (Collider other)
	{

	}

    public IEnumerator WaitTillInActive(float WaitTime) {
        yield return new WaitForSeconds(WaitTime);
        Debug.Log("Destroyed Wait till");
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        this.gameObject.SetActive(false);
    }
}
