using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Right_SubMachineGun : MonoBehaviour {

    public GameManager GM;
    public GameObject topObject;
	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.10f;
	private float nextFire = 0.10f;
	private float myTime = 0.0f;
	public GameObject subMachinegunBullet;
	public int Ammo = 100;
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
		//AmmoCount = GameObject.FindGameObjectWithTag("RightWeaponAmmo").GetComponent<Text>();
        bulletPoolParent = GameObject.FindGameObjectWithTag("RightBulletParent");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("RightExtraAmmoVariable") == 1) {
            magazineCount += 3;
        }
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
        //Player.ActivateObject(Player.AmmoHolder, 0);
        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject RSMG_Bullet = Instantiate(subMachinegunBullet, bulletPoolParent.transform);
            bulletPool.Add(RSMG_Bullet);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }
        //Debug.Log("bulletPool count is " + bulletPool.Count);
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo +"\n"+ extraAmmo;
		//Debug.DrawRay (ShotSpawn.transform.position, -ShotSpawn.transform.right, Color.red);

        if (!GM.prevState.IsConnected) {

            if (Input.GetKeyDown(KeyCode.R) && Input.GetMouseButton(1) && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Right_Reload());
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButton (1) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
			    MuzzleFlash.Play();
                shellCasingParticleSystem.Play();
                audioSource.PlayOneShot(shotSound);
			    Shoot ();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
		    }

            if (Input.GetMouseButtonUp(1)) {
                shellCasingParticleSystem.Stop();
            }
        }

        if (GM.prevState.IsConnected) {

            if (GM.prevState.Triggers.Right > 0.45f && GM.prevState.Buttons.X == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.X == XInputDotNetPure.ButtonState.Pressed && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Right_Reload());
                StartCoroutine(Reload());
            }

            if (GM.prevState.Triggers.Right > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                shellCasingParticleSystem.Play();
                audioSource.PlayOneShot(shotSound);
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            if (GM.prevState.Triggers.Right < 0.45f) {
                shellCasingParticleSystem.Stop();
            }
        }

        if(Ammo <= ammoReference / 4 && Player.rightWeaponLowAmmoNotice.activeSelf == false && Reloading == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
            if (Reloading != true) {
                Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        } /*else if(Ammo > ammoReference / 4 && Player.rightWeaponLowAmmoNotice.activeSelf == true && Reloading == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
        }*/

        if (Ammo <= 0 && Reloading == false && extraAmmo != 0) {
            StartCoroutine(Player.Right_Reload());
            audioSource.PlayOneShot(reloadSound);
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
        /*RaycastHit shotHit;
        if (Physics.Raycast(ShotSpawn.transform.position, ShotSpawn.transform.right, out shotHit, 300f)) {

            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                subMachinegunBullet.GetComponent<Bullet>().MediumHit(2, shotHit);
            }
        }*/
        #region Object Pool

        if (bulletPool[poolCount] == null) {
            GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            bulletPool[poolCount] = RSMG_Bullet;
            bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
            bulletPool[poolCount].SetActive(false);
        }

        bulletPool[poolCount].SetActive(true);
        bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = false;
        bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
        bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = true;
        bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(transform.right * 1700f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(0.7f));

        /*if (poolCount >= bulletPool.Count - 1) {
            Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount++;
        }*/

        poolCount++;
        #endregion


    }

    private IEnumerator Reload(){
        //Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<Text>().text = "RELOADING";
        if (Player.rightWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        } else {
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        }
        Reloading = true;
        yield return new WaitForSeconds(0.28f);
        /*for (int i = 0; i <= bulletPool.Count - 1; i++) {
            if (bulletPool[i] == null) {
                GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
                bulletPool[i] = RSMG_Bullet;
                bulletPool[i].transform.parent = bulletPoolParent.transform;
                bulletPool[i].SetActive(false);
            } else {
                bulletPool[i].SetActive(true);
                bulletPool[i].GetComponent<TrailRenderer>().enabled = false;
                bulletPool[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                bulletPool[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                bulletPool[i].transform.position = ShotSpawn.transform.position;
                yield return new WaitForSeconds(0.02f);
                bulletPool[i].GetComponent<TrailRenderer>().enabled = true;
                bulletPool[i].transform.rotation = ShotSpawn.transform.rotation;
            }
        }*/
        yield return new WaitForSeconds(0.60f);

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
		yield return new WaitForSeconds(1.25f);
        DestroyBullets();
        this.gameObject.transform.parent = null;
		this.GetComponent<Rigidbody>().useGravity=true;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<Rigidbody>().AddForce(new Vector3(30, -0.5f, 0),ForceMode.VelocityChange);
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		this.GetComponent<MeshCollider>().enabled = true;
		Destroy(this.gameObject, 10.0f);
	}

    void DestroyBullets() {
        for (int i = 0; i < bulletPool.Count; i++) {
            Destroy(bulletPool[i]);
        }
    }
}
