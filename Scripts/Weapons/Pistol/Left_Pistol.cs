using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Left_Pistol : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.45f;
	private float nextFire = 0.45f;
	private float myTime = 0.0f;
	public GameObject pistolBullet;
	public int Ammo = 15;
    private int ammoReference;
    private int extraAmmo;
    public int magazineCount;
    public TextMeshProUGUI AmmoCount;
	public bool Reloading;
    public ParticleSystem shellCasingParticleSystem;
    [Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash;
	public AudioSource audioSource;

    [Header("Colors")]
    public Color green;
    public Color yellow;
    public Color red;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        bulletPoolParent = GameObject.FindGameObjectWithTag("LeftBulletParent");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = this.GetComponent<AudioSource>();
        ammoReference = Ammo ;
        Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 0);

        for (int i = 0; i < ammoReference * magazineCount; i++) {
            Debug.Log("Spawning Bullet Object");
            GameObject P_Bullet = Instantiate(pistolBullet);
            bulletPool.Add(P_Bullet);
            //bulletPool[i] = P_Bullet;
            bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].SetActive(false);
        }
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
        AmmoCount.text = Ammo + "\n" + "----";

        if (!GM.prevState.IsConnected) {

            if (Input.GetKeyDown(KeyCode.R) && Input.GetMouseButton(0) && Reloading == false && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Left_Reload());
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButton (0) && myTime > nextFire && Ammo > 0 && Reloading != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
			    MuzzleFlash.Play ();
                //shellCasingParticleSystem.Play();
                audioSource.PlayOneShot(audioSource.clip);
                Shoot ();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
		    }

            if (Input.GetMouseButtonUp(0)) {
                //shellCasingParticleSystem.Stop();
            }
        }

        if (GM.prevState.IsConnected) {

            if (GM.prevState.Triggers.Left > 0.45f && GM.prevState.Buttons.X == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.X == XInputDotNetPure.ButtonState.Pressed && Reloading == false && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Left_Reload());
                StartCoroutine(Reload());
            }

            if (GM.prevState.Triggers.Left > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                //shellCasingParticleSystem.Play();
                audioSource.PlayOneShot(audioSource.clip);
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            if (GM.prevState.Triggers.Left < 0.45f) {
                //shellCasingParticleSystem.Stop();
            }
        }

        if (Ammo <= ammoReference / 4 && Player.leftWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 1);
            if (Reloading != true) {
                Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }

        if (Ammo <= 0 && Reloading == false) {
            StartCoroutine(Player.Left_Reload());
            StartCoroutine(Reload());
        }
    }

	private void Shoot(){
		Debug.Log ("Shooting");
        /*GameObject pistolBullet_I = (GameObject)Instantiate (pistolBullet,ShotSpawn.transform.position, Quaternion.identity);
		pistolBullet_I.GetComponent<Rigidbody> ().AddForce (-transform.forward * 1000f, ForceMode.VelocityChange);
		Destroy (pistolBullet_I, 0.4f);
        
        RaycastHit shotHit;
        if (Physics.Raycast(ShotSpawn.transform.position, -ShotSpawn.transform.right, out shotHit, 300f)) {

            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                pistolBullet.GetComponent<Bullet>().MediumHit(6, shotHit);
            }
        }*/

        #region Object Pool

        if (bulletPool[poolCount] == null) {
            GameObject P_Bullet = Instantiate(pistolBullet);
            bulletPool[poolCount] = P_Bullet;
            bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
            bulletPool[poolCount].SetActive(false);
        }

        bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
        bulletPool[poolCount].SetActive(true);
        bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(transform.forward * 1700f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(1.0f));
        //Debug.Log("Just fired Left");

        if (poolCount >= bulletPool.Count - 1) {
            // Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            //Debug.Log("pool count add 1");
            poolCount++;
        }
        #endregion

    }

    private IEnumerator Reload(){
        if (Player.leftWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 1);
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        } else {
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        }

        Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = ammoReference;
        Reloading = false;
        Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "CLEAR";
        Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = green;
        yield return new WaitForSeconds(1.98f);
        Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 0);
    }
}
