using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtrilleryCannon : MonoBehaviour
{
    public Transform cannonBarrel;
    public GameObject pivotOne;
    public GameObject pivotTwo;
    public GameObject Player;
    public PlayerController PC;
    public float distanceToPlayer;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PC = Player.GetComponent<PlayerController>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(this.transform.position, Player.transform.position);
        if(distanceToPlayer < 500f) {
            //PlayerTracking();
        }
    }

    private void PlayerTracking() {
        var pivotOneRotation = Quaternion.LookRotation(pivotOne.transform.position - Player.transform.position, Vector3.forward);
        pivotOneRotation.z = 0.0f;
        pivotOneRotation.x = 0.0f;
        pivotOne.transform.rotation = Quaternion.Slerp(pivotOne.transform.rotation, pivotOneRotation, Time.deltaTime * 8);


        var pivotTwoRotation = Quaternion.LookRotation(pivotTwo.transform.position - Player.transform.position, Vector3.forward);
        pivotTwoRotation.z = 0.0f;
        pivotTwoRotation.y = 0.0f;
        pivotTwo.transform.rotation = Quaternion.Slerp(pivotTwo.transform.rotation, pivotTwoRotation, Time.deltaTime * 8);
    }
}
