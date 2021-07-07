using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Left_AssaultRifle : MonoBehaviour {

    public GameManager GM;
    public GameObject topObject;
	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.25f;
	private float nextFire = 0.25f;
	private float myTime = 0.0f;
	public GameObject assaultRifleBullet;
	public int Ammo = 50;
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
    public List<GameObject> bulletPool = new List<GameObject> ();
    [SerializeField]
    private int poolCount;

    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        bulletPoolParent = GameObject.FindGameObjectWithTag("LeftBulletParent");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = this.GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("LeftExtraAmmoVariable") == 1) {
            magazineCount += 3;
        }
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 0);
        Player.ActivateObject(Player.AmmoHolder, 0);
        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject LAR_Bullet = Instantiate(assaultRifleBullet, bulletPoolParent.transform);
            bulletPool.Add(LAR_Bullet);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }
        //Debug.Log("bulletPool count is " + bulletPool.Count);
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
        AmmoCount.text = Ammo + "\n" + extraAmmo;
        Debug.DrawRay (ShotSpawn.transform.position, -ShotSpawn.transform.right, Color.red);

        if (!GM.prevState.IsConnected) {

            if (Input.GetKeyDown(KeyCode.R) && Input.GetMouseButton(0) && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Left_Reload());
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButton (0) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
			    MuzzleFlash.Play ();
                shellCasingParticleSystem.Play();
                audioSource.PlayOneShot(shotSound);
                Shoot();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
		    }

            if (Input.GetMouseButtonUp(0)) {
                shellCasingParticleSystem.Stop();
            }
        }

        if (GM.prevState.IsConnected) {

            if (GM.prevState.Triggers.Left > 0.45f && GM.prevState.Buttons.X == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.X == XInputDotNetPure.ButtonState.Pressed && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Left_Reload());
                StartCoroutine(Reload());
            }

            if (GM.prevState.Triggers.Left > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                shellCasingParticleSystem.Play();
                audioSource.PlayOneShot(shotSound);
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            if (GM.prevState.Triggers.Left < 0.45f) {
                shellCasingParticleSystem.Stop();
            }
        }

        if (Ammo <= ammoReference / 4 && Player.leftWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 1);
            if (Reloading != true) {
                Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }

        if (Ammo <= 0 && Reloading == false && extraAmmo != 0) {
            StartCoroutine(Player.Left_Reload());
            StartCoroutine(Reload());
        }

        if (Ammo <= 0 && extraAmmo == 0 && dropped != true) {
            StartCoroutine(Player.LeftPistolSwap());
            StartCoroutine(PistolSwap());
        }

        if (Input.GetKeyDown (KeyCode.K) && Player.canMove == true) {
            StartCoroutine(Player.LeftPistolSwap());
            StartCoroutine( PistolSwap());
		}
	}

	private void Shoot(){
		//Debug.Log ("Shooting");
        /*RaycastHit shotHit;
		if(Physics.Raycast(ShotSpawn.transform.position, -ShotSpawn.transform.right,out shotHit,400f)){

			Debug.Log ("Hit object: " + shotHit.transform.gameObject);
			if (shotHit.collider.tag == "Enemy") {
                assaultRifleBullet.GetComponent<Bullet>().MediumHit(3,shotHit);
			}
		}*/
        #region Object Pool

        if (bulletPool[poolCount] == null) {
            GameObject LAR_Bullet = Instantiate(assaultRifleBullet);
            bulletPool[poolCount] = LAR_Bullet;
            bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
            bulletPool[poolCount].SetActive(false);
        }

        bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
        bulletPool[poolCount].SetActive(true);
        bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody> ().AddForce(-transform.right * 1700f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(1.0f));
        //Debug.Log("Just fired Left");

        /*if (poolCount >= bulletPool.Count-1) {
           // Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            //Debug.Log("pool count add 1");
            poolCount++;
        }*/

        poolCount++;
        #endregion

    }

    private IEnumerator Reload(){
        if (Player.leftWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 1);
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        } else{
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        }

        Reloading = true;
		yield return new WaitForSeconds(0.98f);
        audioSource.PlayOneShot(reloadSound);
        extraAmmo -= ammoReference - Ammo;
        Ammo = ammoReference;
        Reloading = false;

        Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "CLEAR";
        Player.leftWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = green;
        yield return new WaitForSeconds(1.98f);
        Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 0);
    }

	public IEnumerator PistolSwap(){
		dropped = true;
		yield return new WaitForSeconds(1.75f);
        DestroyBullets();
        this.gameObject.transform.parent = null;
		this.GetComponent<Rigidbody>().useGravity=true;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<Rigidbody>().AddForce(new Vector3(-30, -0.5f, 0),ForceMode.VelocityChange);
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<MeshCollider>().enabled = true;
        yield return new WaitForSeconds(10.0f);
        topObject.SetActive(false);
        this.gameObject.SetActive(false);
	}

    void DestroyBullets() {
        for (int i = 0; i < bulletPool.Count; i++) {
            Destroy(bulletPool[i]);
        }
    }
}
