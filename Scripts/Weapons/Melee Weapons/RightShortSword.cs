using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightShortSword : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
    private bool canAttack = true;
    public Animator Anim;
    public Text AmmoCount;
    public AudioSource bladeAudioSource;
    public AudioClip bladeAudio;

    void Start() {

        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Anim = GetComponent<Animator>();
        //AmmoCount = GameObject.FindGameObjectWithTag("LeftWeaponAmmo").GetComponent<Text>();
        Player.Anim.SetBool("rightMeleeWeapon", true);
        Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
    }

    // Update is called once per frame
    void Update() {
        if (canAttack == true && Input.GetMouseButtonDown(1) && Player.canMove == true) {
            canAttack = false;
            Player.Anim.SetBool("rightMeleeSwingOne", true);
            StartCoroutine(Attack());
        }

        if (canAttack == true && GM.prevState.Triggers.Right > 0.45f && Player.canMove == true) {
            canAttack = false;
            Player.Anim.SetBool("rightMeleeSwingOne", true);
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() {
        yield return new WaitForSeconds(0.25f);
        bladeAudioSource.PlayOneShot(bladeAudio);
        StartCoroutine(Player.Dash(Player.moveX, 0.0f, Player.moveZ, 0.040f));
        yield return new WaitForSeconds(0.75f);
        Player.Anim.SetBool("rightMeleeSwingOne", false);
        yield return new WaitForSeconds(0.35f);
        canAttack = true;
    }
}
