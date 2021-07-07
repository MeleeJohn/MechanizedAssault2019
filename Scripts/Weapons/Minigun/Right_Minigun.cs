using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Right_Minigun : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
	public GameObject ShotSpawn_1;
	public GameObject ShotSpawn_2;
	private float fireDelta = 0.12f;
	private float nextFire = 0.12f;
	private float myTime = 0.0f;
	public GameObject minigunBullet;
	public int Ammo = 250;
    private int ammoReference;
    public TextMeshProUGUI AmmoCount;
	public bool Reloading;
	public Animator Anim;
	public bool dropped = false;
    public ParticleSystem shellCasingParticleSystem;
    public ParticleSystem shellCasingParticleSystem_2;
    [Header("Muzzle Effects")]
	public ParticleSystem MuzzleFlash_1;
	public ParticleSystem MuzzleFlash_2;
	public AudioSource audioSource;
    public AudioClip shotSound;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    public List<GameObject> bulletPool_2 = new List<GameObject>();
    [SerializeField]
    private int poolCount_2;

    void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("RightExtraAmmoVariable") == 1) {
            Ammo += 100;
        }
        Anim = GetComponent<Animator>();
        ammoReference = Ammo;
        bulletPoolParent = GameObject.FindGameObjectWithTag("RightBulletParent");
        Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 0);
        Player.ActivateObject(Player.AmmoHolder, 0);
        for (int i = 0; i < ammoReference; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject RMG_Bullet_1 = Instantiate(minigunBullet, bulletPoolParent.transform);
            bulletPool.Add(RMG_Bullet_1);
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }

        /*for (int i = 0; i < bulletPool_2.Count; i++) {
            GameObject RMG_Bullet_2 = Instantiate(minigunBullet);
            bulletPool_2[i] = RMG_Bullet_2;
            bulletPool_2[i].transform.parent = bulletPoolParent.transform;
            bulletPool_2[i].SetActive(false);
        }*/

        for (int i = 0; i < ammoReference; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject RMG_Bullet_2 = Instantiate(minigunBullet, bulletPoolParent.transform);
            bulletPool_2.Add(RMG_Bullet_2);
            bulletPool_2[i].transform.position = Vector3.zero;
            bulletPool_2[i].SetActive(false);
        }
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
		AmmoCount.text = Ammo.ToString ();

        if (!GM.prevState.IsConnected) {
            if (Input.GetMouseButtonDown (1) && Player.canMove == true && Reloading != true && dropped != true) {
			    Anim.SetBool("Spin Down", false);
			    Anim.SetBool ("Firing", true);
		    }
        } else if (GM.prevState.IsConnected) {
            if (GM.prevState.Triggers.Right > 0.45f && Player.canMove == true && Reloading != true && dropped != true) {
                Anim.SetBool("Spin Down", false);
                Anim.SetBool("Firing", true);
            }
        }

        if (!GM.prevState.IsConnected) {
            if (Input.GetMouseButton (1) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
			    MuzzleFlash_1.Play ();
			    Shoot_1 ();
			    MuzzleFlash_2.Play ();
                shellCasingParticleSystem.Play();
                shellCasingParticleSystem_2.Play();
                audioSource.PlayOneShot(shotSound);
                Shoot_2();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
            }

            if (Input.GetMouseButtonUp(1)) {
                shellCasingParticleSystem.Stop();
                shellCasingParticleSystem_2.Stop();
            }
        }

        if (GM.prevState.IsConnected) {
            if (GM.prevState.Triggers.Right > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash_1.Play();
                Shoot_1();
                MuzzleFlash_2.Play();
                shellCasingParticleSystem.Play();
                shellCasingParticleSystem_2.Play();
                audioSource.PlayOneShot(shotSound);
                Shoot_2();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            if (GM.prevState.Triggers.Right < 0.45f) {
                shellCasingParticleSystem.Stop();
                shellCasingParticleSystem_2.Stop();
            }
        }

        if (Ammo <= ammoReference / 4 && Ammo > 0 && Player.rightWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.yellow;
        } else if (Ammo <= 0 && Player.rightWeaponLowAmmoNotice.activeSelf == false) {
            Player.ActivateObject(Player.rightWeaponLowAmmoNotice, 1);
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "NO AMMO";
            Player.rightWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
        }

        if (Input.GetMouseButtonUp (1) && Player.canMove == true) {
			Anim.SetBool ("Firing", false);
			Anim.SetBool("Spin Down", true);
		}

        if(Ammo<=0 && dropped != true) {
            StartCoroutine(Player.RightPistolSwap());
            StartCoroutine(PistolSwap());
        }

		if (Input.GetKeyDown (KeyCode.L) && Player.canMove == true) {
            StartCoroutine(Player.RightPistolSwap());
            StartCoroutine( PistolSwap());
		}
	}

	private void Shoot_1(){
        #region Object Pool

        if (bulletPool[poolCount] == null) {
            GameObject LMG_Bullet = Instantiate(minigunBullet);
            bulletPool[poolCount] = LMG_Bullet;
            bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
            bulletPool[poolCount].SetActive(false);
        }

        bulletPool[poolCount].SetActive(true);
        bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = false;
        bulletPool[poolCount].transform.position = ShotSpawn_1.transform.position;
        bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = true;
        bulletPool[poolCount].transform.rotation = ShotSpawn_1.transform.rotation;
        bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(-transform.right * 1450f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(0.8f));

        /*if (poolCount >= bulletPool.Count - 1) {
            Debug.Log("pool count reset");
            poolCount = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount++;
        }*/
        poolCount++;
        #endregion
        /*RaycastHit shotHit;
        if (Physics.Raycast(ShotSpawn_1.transform.position, -ShotSpawn_1.transform.right, out shotHit, 400f)) {

            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                minigunBullet.GetComponent<Bullet>().MediumHit(2, shotHit);
            }
        }*/
    }

    private void Shoot_2(){
        #region Object Pool

        if (bulletPool_2[poolCount_2] == null) {
            GameObject LMG_Bullet_2 = Instantiate(minigunBullet);
            bulletPool_2[poolCount_2] = LMG_Bullet_2;
            bulletPool_2[poolCount_2].transform.parent = bulletPoolParent.transform;
            bulletPool_2[poolCount_2].SetActive(false);
        }

        bulletPool_2[poolCount_2].SetActive(true);
        bulletPool_2[poolCount_2].GetComponent<TrailRenderer>().enabled = false;
        bulletPool_2[poolCount_2].transform.position = ShotSpawn_2.transform.position;
        bulletPool_2[poolCount_2].GetComponent<TrailRenderer>().enabled = true;
        bulletPool_2[poolCount_2].transform.rotation = ShotSpawn_2.transform.rotation;
        bulletPool_2[poolCount_2].GetComponent<Rigidbody>().AddForce(-transform.right * 1450f, ForceMode.VelocityChange);
        StartCoroutine(bulletPool[poolCount_2].GetComponent<Bullet>().WaitInActive(0.8f));

        /*if (poolCount_2 >= bulletPool_2.Count - 1) {
            Debug.Log("pool count reset");
            poolCount_2 = 0;
        } else {
            Debug.Log("pool count add 1");
            poolCount_2++;
        }*/
        poolCount_2++;
        #endregion
        /*
        RaycastHit shotHit;
        if (Physics.Raycast(ShotSpawn_2.transform.position, -ShotSpawn_2.transform.right, out shotHit, 400f)) {

            Debug.Log("Hit object: " + shotHit.transform.gameObject);
            if (shotHit.collider.tag == "Enemy") {
                minigunBullet.GetComponent<Bullet>().MediumHit(2, shotHit);
            }
        }*/
    }

    private IEnumerator Reload(){
		Reloading = true;
		yield return new WaitForSeconds(0.98f);
		Ammo = 150;
		Reloading = false;
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
            Destroy(bulletPool_2[i]);
        }
    }
}
