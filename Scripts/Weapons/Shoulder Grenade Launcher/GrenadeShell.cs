using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeShell : MonoBehaviour {

    public GameObject Explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hit(50);
            DestroyObject();
        }
        else if(other.tag != "Player" && other.tag != "Aiming Collider" && other.tag != "Bounding Box")
        {
            Debug.Log("Grenade Shell hit " + other.gameObject.name);
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    public IEnumerator GrenadeTimer()
    {
        yield return new WaitForSeconds(5.0f);
        GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        this.gameObject.SetActive(false);
    }
}
