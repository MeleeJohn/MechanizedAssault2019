using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftShortSword : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
    public bool canAttack;
    public Animator Anim;
    public Text AmmoCount;
    public AudioSource bladeAudioSource;
    public AudioClip bladeAudio;


    void Start () {
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Anim = GetComponent<Animator>();
        //AmmoCount = GameObject.FindGameObjectWithTag("LeftWeaponAmmo").GetComponent<Text>();
        Player.Anim.SetBool("leftMeleeWeapon",true);
        Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 0);
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetMouseButtonDown(0) && canAttack == true  && Player.canMove == true) {
            canAttack = false;
            Player.Anim.SetBool("leftMeleeSwingOne",true);
            StartCoroutine(Attack());
        }

        if (canAttack == true && GM.prevState.Triggers.Left > 0.45f && Player.canMove == true) {
            canAttack = false;
            Player.Anim.SetBool("leftMeleeSwingOne", true);
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        yield return new WaitForSeconds(0.25f);
        bladeAudioSource.PlayOneShot(bladeAudio);
        StartCoroutine(Player.Dash(Player.moveX, 0.0f, Player.moveZ, 0.040f));
        yield return new WaitForSeconds(0.75f);
        Player.Anim.SetBool("leftMeleeSwingOne", false);
        yield return new WaitForSeconds(0.35f);
        canAttack = true;
    }
}
