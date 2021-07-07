using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame_TestScript : MonoBehaviour {

	public GameObject leftArm;
	public GameObject rightArm;
	public GameObject targetedEnemy;
	public Animator animatorController;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		rightArm.transform.LookAt(targetedEnemy.transform,rightArm.transform.right);
		leftArm.transform.LookAt(targetedEnemy.transform,leftArm.transform.right);
		if (Input.GetKeyDown (KeyCode.R)) {
			animatorController.SetBool("CanReload",true);
		}
	}
}
