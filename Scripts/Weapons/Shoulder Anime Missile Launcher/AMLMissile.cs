using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMLMissile : MonoBehaviour {
    public bool startFunction;
    public GameObject Parent;
    public PlayerController pc;
    public Transform target;
    public GameObject SpawnPoint;
    public GameObject chosenExplosion;
    public GameObject parentSpawn;
    
    private float speed = 120f;

    public bool lockedOn;

    void Awake() {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (pc.targetedLeftEnemy != null || pc.targetedRightEnemy != null) {
            target = pc.targetedRightEnemy.transform;
        }
    }

    void Start() {

    }

    void Update() {
        if(startFunction == true){
            if (lockedOn == true) {
                float step = speed * Time.deltaTime;
                transform.LookAt(target);
                if (target != null) {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                } else {
                    lockedOn = false;
                }
            } else if (pc.targetedLeftEnemy == null || pc.targetedRightEnemy == null) {

                lockedOn = false;
                GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.VelocityChange);
                StartCoroutine(DestroyMissle(3f, this.transform.position));
            }
        }
        if (chosenExplosion.activeSelf == false) {
            chosenExplosion.SetActive(true);
        } else if (chosenExplosion.activeSelf == true) {
            chosenExplosion.transform.position = this.transform.position;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            Debug.Log("Hit Enemy");
            other.GetComponent<Enemy>().Hit(10);
            StartCoroutine(DestroyMissle(0f, other.gameObject.transform.position));
        } else if (other.tag == "Ground" || other.tag == "Level Asset") {
            Debug.Log("Hit Level Asset");
            StartCoroutine(DestroyMissle(0f, other.gameObject.transform.position));
        }
    }

    public IEnumerator DestroyMissle(float waitTime, Vector3 DyingLocation) {
        yield return new WaitForSeconds(waitTime);
        //transform.parent = parentSpawn.transform;
        //this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.transform.eulerAngles = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        DyingLocation = this.gameObject.transform.position;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //Explosion.SetActive(true);
        target = null;
        //chosenExplosion.transform.position = DyingLocation;
        if(chosenExplosion != null) {
            yield return new WaitForSeconds(0.5f);
            chosenExplosion.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            chosenExplosion.transform.GetChild(0).gameObject.SetActive(false);
        } else {
            yield return new WaitForSeconds(2.5f);
        }
        //Debug.Log("Destroying Explosion");
        Debug.Log("Velocity is: " + this.gameObject.GetComponent<Rigidbody>().velocity);
        Debug.Log("Angular Velocity is: " + this.gameObject.GetComponent<Rigidbody>().angularVelocity);
        this.gameObject.SetActive(false);
    }

    public IEnumerator GetTarget() {
        yield return new WaitForSeconds(1.0f);
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        if(target != null){
            lockedOn = true;
        }
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        startFunction = true;
    }
}
