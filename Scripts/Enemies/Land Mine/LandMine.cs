using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    private bool hasTriggered;
    private bool hasBeenDiscovered;
    public GameObject Explosion;
    public GameObject hudNavigationElement;
    public GameObject Player;
    public int playerDamage;
    public AudioSource audioSource;
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Player == null) {
            Player = GameObject.FindGameObjectWithTag("Player");
        } else{
        if(Vector3.Distance(this.transform.position, Player.transform.position) < 50 && hasBeenDiscovered == false) {
            //hudNavigationElement.SetActive(true);
            hasBeenDiscovered = true;
        }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && hasTriggered == false){
            Explosion.SetActive(true);
            hasTriggered = true;
            StartCoroutine(HitPlayer(other.gameObject));
        }
    }

    private IEnumerator HitPlayer(GameObject Player) {
        Player.GetComponent<PlayerController>().Hit(playerDamage);
        audioSource.Play();
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }
}
