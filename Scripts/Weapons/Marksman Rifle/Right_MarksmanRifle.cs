using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Right_MarksmanRifle : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.45f;
	private float nextFire = 0.45f;
	private float myTime = 0.0f;
	public GameObject marksmanRifleBullet;
	public int Ammo = 35;
    private int ammoReference;
    private int extraAmmo;
    public int magazineCount;
    public TextMeshProUGUI AmmoCount;
	public bool Reloading;
	public bool dropped = false;
    public ParticleSystem shellCasingParticleSystem;
    [Header("Colors")]
    public Color green;
    public Color yellow;
    public Color red;

    [Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash;
    public AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip reloadSound;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        bulletPoolParent = GameObject.FindGameObjectWithTag("RightBulletParent");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("RightExtraAmmoVariable") == 1) {
            magazineCount += 3;
        }
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
        Player.ActivateObject(Player.AmmoHolder, 0);
        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject RMR_Bullet = Instantiate(marksmanRifleBullet, bulletPoolParent.transform);
            bulletPool.Add(RMR_Bullet);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + bulletPool.Count);
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
        AmmoCount.text = Ammo + "\n" + extraAmmo;

        if (!GM.prevState.IsConnected) {

            if (Input.GetKeyDown(KeyCode.R) && Input.GetMouseButton(1) && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Right_Reload());
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButtonDown (1) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
			    MuzzleFlash.Play ();
                //shellCasingParticleSystem.Play();
                Shoot ();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
		    }

            /*if (Input.GetMouseButtonUp(1)) {
                shellCasingParticleSystem.Stop();
            }*/
        }

        if (GM.prevState.IsConnected) {

            if (GM.prevState.Triggers.Right > 0.45f && GM.prevState.Buttons.X == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.X == XInputDotNetPure.ButtonState.Pressed && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Right_Reload());
                StartCoroutine(Reload());
            }

            if (GM.prevState.Triggers.Right > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                //shellCasingParticleSystem.Play();
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            /*if (GM.prevState.Triggers.Right < 0.45f) {
                shellCasingParticleSystem.Stop();
            }*/
        }

        if (Ammo <= ammoReference / 4 && Player.rightWeaponLowAmmoNotice.activeSelf == false && Reloading == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
            if (Reloading != true) {
                Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }

        if (Ammo <= 0 && Reloading == false && extraAmmo != 0) {
            StartCoroutine(Player.Right_Reload());
            StartCoroutine(Reload());
        }

        if (Ammo <= 0 && extraAmmo == 0 && dropped != true) {
            StartCoroutine(Player.RightPistolSwap());
            StartCoroutine(PistolSwap());
        }

        if (Input.GetKeyDown (KeyCode.L) && Player.canMove == true) {
            StartCoroutine(Player.RightPistolSwap());
            StartCoroutine( PistolSwap());
		}
	}

	private void Shoot(){
		Debug.Log ("Shooting");
        audioSource.PlayOneShot(shotSound);
        /*GameObject marksmanRilfebullet_I = (GameObject)Instantiate (marksmanRifleBullet,ShotSpawn.transform.position, Quaternion.identity);
		marksmanRilfebullet_I.GetComponent<Rigidbody> ().AddForce (-transform.right * 3000f, ForceMode.VelocityChange);
		Destroy (marksmanRilfebullet_I, 0.9f);
        //Vector3 forward = ShotSpawn.transform.TransformDirection (ShotSpawn.transform.forward);
        RaycastHit shotHit;
        if (Physics.Raycast(ShotSpawn.transform.position, -ShotSpawn.transform.right, out shotHit, 700f)) {
            //Debug.Log ("Hit soemthing at: " + shotHit.distance);
            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                marksmanRifleBullet.GetComponent<Bullet>().MediumHit(7, shotHit);
            }
        }*/

        #region Object Pool

        Debug.Log("About to fire Left");

        bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
        bulletPool[poolCount].SetActive(true);
        bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(-transform.right * 2500f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(1.0f));

        Debug.Log("Just fired Left");

        /*if (poolCount >= bulletPool.Count - 1) {
            Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount++;
        }*/
        poolCount++;
        shellCasingParticleSystem.Stop();
        #endregion

    }

    private IEnumerator Reload(){
        if (Player.rightWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        } else {
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        }

        Reloading = true;
        yield return new WaitForSeconds(0.98f);
        audioSource.PlayOneShot(reloadSound);
        extraAmmo -= ammoReference - Ammo;
        Ammo = ammoReference;
        Reloading = false;
        Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "CLEAR";
        Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = green;
        yield return new WaitForSeconds(1.98f);
        Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
    }

	public IEnumerator PistolSwap(){
		dropped = true;
		yield return new WaitForSeconds(1.75f);
        DestroyBullets();
        this.gameObject.transform.parent = null;
		this.GetComponent<Rigidbody>().useGravity=true;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<Rigidbody>().AddForce(new Vector3(30, -0.5f, 0),ForceMode.VelocityChange);
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<MeshCollider>().enabled = true;
		Destroy(this.gameObject, 10.0f);
		//Destroy(topObject, 10.0f);
	}

    void DestroyBullets() {
        for (int i = 0; i < bulletPool.Count; i++) {
            Destroy(bulletPool[i]);
        }
    }
}
