using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareObject : MonoBehaviour {

    public GameObject flareEmitter;
    public ParticleSystem flareParticles;
    public PlayerController PC;
    public GameObject currentMissileGrabbed;


    private void Update() {
        //SearchMissile();
        if(flareEmitter == null) {
            flareEmitter = GameObject.FindGameObjectWithTag("Flare emitter");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "EnemyMissile") {
            if(other.gameObject.GetComponent<EnemyMissile>().Dying == false){
                StartCoroutine(other.gameObject.GetComponent<EnemyMissile>().DestroyObject(0.0f));
            }
        }
    }

    public IEnumerator SetActiveWait() {
        yield return new WaitForSeconds(5.0f);
        flareParticles.Stop();
        this.GetComponent<LensFlare>().enabled = false;
        this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        this.transform.position = flareEmitter.transform.position;
        this.gameObject.SetActive(false);
    }
}
