using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {
	public PlayerController pc;
	public Transform target;
	public GameObject SpawnPoint;
    public GameObject chosenExplosion;

    public float speed = 20f;

    public bool lockedOn;
	//private Rigidbody rb;
	// Use this for initialization
	void Awake ()
	{
		pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        if (pc.targetedLeftEnemy != null || pc.targetedRightEnemy != null) {
			target = pc.targetedRightEnemy.transform;
		}
		//SpawnPoint = GameObject.FindGameObjectWithTag("Missle_Launcher").GetComponent<MissleLauncher>().missleSpawn;
		//rb = GetComponent<Rigidbody>();
	}

	void Start () {
		//pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		//target = pc.Enemy.transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//target = pc.Enemy.transform;

        if(chosenExplosion.activeSelf == false) {
            chosenExplosion.SetActive(true);
        } else if (chosenExplosion.activeSelf == true) {
            chosenExplosion.transform.position = this.transform.position;
        }
        if (lockedOn == true) {
			float step = speed * Time.deltaTime;
			transform.LookAt (target);
                if(target != null) { 
			        transform.position = Vector3.MoveTowards (transform.position, target.position, step);
                } else {
                    lockedOn = false;
                }
        } else if (pc.targetedLeftEnemy == null || pc.targetedRightEnemy == null) {
            //rb.velocity = SpawnPoint.transform.forward * speed;
            lockedOn = false;
            GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.VelocityChange);
            StartCoroutine(DestroyMissle(3f, this.transform.position));
        }
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Enemy") {
            Debug.Log("Hit Enemy");
			other.GetComponent<Enemy>().Hit(20);
            StartCoroutine(DestroyMissle(0f, other.gameObject.transform.position));
		} else if (other.tag == "Ground" || other.tag == "Level Asset"){
            Debug.Log("Hit Level Asset");
            StartCoroutine(DestroyMissle(0f, other.gameObject.transform.position));
        }
	}

    public IEnumerator DestroyMissle(float waitTime, Vector3 DyingLocation)
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.gameObject.transform.eulerAngles = Vector3.zero;
        DyingLocation = this.gameObject.transform.position;
        chosenExplosion.transform.position = DyingLocation;
        yield return new WaitForSeconds(0.5f);
        chosenExplosion.transform.GetChild(0).gameObject.SetActive(true);
        //chosenExplosion.transform.position = DyingLocation;
        yield return new WaitForSeconds(2.0f);
        chosenExplosion.SetActive(false);
        Debug.Log("Destroying Explosion");
        target = null;
        this.gameObject.SetActive(false);
        //Instantiate(Explosion, transform.position, Quaternion.identity);
        //Destroy(this.gameObject);
    }
}
