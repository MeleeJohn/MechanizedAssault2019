using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum BulletType{Basic, Intermideate, Advanced};

public class EnemyBullet : MonoBehaviour {

    [EnumToggleButtons]
    public BulletType BT;
    public bool isBossGun;
    public int bossDamage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(isBossGun == false){
                switch (BT)
                {
                    case BulletType.Basic:
                        other.GetComponent<PlayerController>().Hit(4);
                        StartCoroutine(WaitDestroy(0.0f));
                        break;

                    case BulletType.Intermideate:
                        other.GetComponent<PlayerController>().Hit(6);
                        StartCoroutine(WaitDestroy(0.0f));
                        break;

                    case BulletType.Advanced:
                        other.GetComponent<PlayerController>().Hit(8);
                        StartCoroutine(WaitDestroy(0.0f));
                        break;
                }
            } else {
                other.GetComponent<PlayerController>().Hit(bossDamage);
                StartCoroutine(WaitDestroy(0.0f));
            }
        }
    }

    public IEnumerator WaitDestroy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        if(this.gameObject != null){
        this.GetComponent<TrailRenderer>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
        this.gameObject.SetActive(false);
        }
    }
}
