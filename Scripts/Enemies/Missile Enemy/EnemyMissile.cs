using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour {

    //private GameObject flareObject;
    public Transform Target;
    public GameObject Player;
    public PlayerController PC;

    private readonly float Speed = 130f;
    [SerializeField]
    private bool openingMovement;
    public bool flaredOut = false;
    private bool targeting = false;
    public bool Dying;

    public GameObject Explosion;
    public AudioSource AS;

    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        PC = Player.GetComponent<PlayerController>();
        PC.missileTargeted = true;
        Target = Player.transform;
        targeting = true;
        openingMovement = true;
        GameObject.FindGameObjectWithTag("Flare emitter").GetComponent<FlareEmitter_Test>().activeMissiles.Add(this.gameObject);
        StartCoroutine(UpwardStart());
	}
	
	// Update is called once per frame
	void Update () {
        if(openingMovement == false && Dying == false){
            if (flaredOut == false){
                Target = Player.transform;
                float step = Speed * Time.deltaTime;
                transform.LookAt(Target);
                transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
                PC.missileTargeted = true;
            } else {
                float step = Speed * Time.deltaTime;
                transform.LookAt(Target);
                transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
                //StartCoroutine(DestroyObject(3.0f));
                float distance = Vector3.Distance(this.transform.position, Target.transform.position);
                if(distance < 10f && Dying == false) {
                    StartCoroutine(DestroyObject(0.0f));
                }
            }
        }
        /*if(flaredOut == false) {
            flaredOut = true;
            Target = GameObject.FindGameObjectWithTag("Flare").transform.position;
        }*/
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && Dying == false) {
            other.GetComponent<PlayerController>().Hit(35);
            StartCoroutine(DestroyObject(0.0f));
        }
    }

    public IEnumerator UpwardStart() {
        /*misslePool[missleCount].transform.position = missleSpawn.transform.position;
        misslePool[missleCount].transform.rotation = missleSpawn.transform.rotation;
        misslePool[missleCount].GetComponent<Rigidbody>().AddForce(missleSpawn.transform.forward * 50f, ForceMode.VelocityChange);
        misslePool[missleCount].SetActive(true);
        StartCoroutine(misslePool[missleCount].GetComponent<Missle>().DestroyMissle(10f, misslePool[missleCount].transform.position));
        */

        //this.transform.eulerAngles = new Vector3(90f,0,0);
        this.GetComponent<Rigidbody>().AddForce(transform.up * 40f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(1.0f);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        openingMovement = false;
    }

    public IEnumerator DestroyObject(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Dying = true;
        //PC.missileTargeted = true;
        Explosion.SetActive(true);
        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        AS.PlayOneShot(AS.clip);
        yield return new WaitForSeconds(2.0f);
        Explosion.SetActive(false);
        GameObject.FindGameObjectWithTag("Flare emitter").GetComponent<FlareEmitter_Test>().activeMissiles.Remove(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void Flared(GameObject newtarget) {
        flaredOut = true;
        Target = newtarget.transform;
    }
}
