using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCrate : MonoBehaviour {

    private Animator crateAnim;
    public GameObject RedFlare;

	void Start () {
        crateAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
            if (hit.distance < 3f && hit.collider.tag == "Ground") {
                crateAnim.SetBool("Crate Open", true);
                RedFlare.SetActive(false);
            }
        }
    }
}
