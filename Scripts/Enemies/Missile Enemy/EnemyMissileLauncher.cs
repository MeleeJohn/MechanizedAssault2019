using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MonoBehaviour {

    public GameObject[] missilesPool;

	// Use this for initialization
	void Start () {
        StartCoroutine(TestLaunch());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator TestLaunch() {
        yield return new WaitForSeconds(5.0f);
        missilesPool[0].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        missilesPool[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);
        missilesPool[2].SetActive(true);
    }
}
