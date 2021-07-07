using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCamera : MonoBehaviour {

    private PlayerController PC;

	void Start () {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = PC.followPoint;
        this.gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = PC.lookPoint;
    }
}
