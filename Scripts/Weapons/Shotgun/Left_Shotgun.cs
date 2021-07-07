using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Left_Shotgun : MonoBehaviour {

    public GameManager GM;
    public GameObject topObject;
	public PlayerController Player;
	public GameObject ShotSpawn;
	private float fireDelta = 0.55f;
	private float nextFire = 0.55f;
	private float myTime = 0.0f;
	public GameObject shotgunBullet;
	public int Ammo = 12;
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

    [Header("Shotgun Spread")]
    public float shotSpreadAngle = 0.1f;

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
        bulletPoolParent = GameObject.FindGameObjectWithTag("LeftBulletParent");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = this.GetComponent<AudioSource>();
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        Player.ActivateObject(Player.leftWeaponLowAmmoNotice, 0);
        /*for (int i = 0; i < bulletPool.Count; i++) {
            GameObject LSG_Bullet = Instantiate(shotgunBullet);
            bulletPool[i] = LSG_Bullet;
            bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].SetActive(false);
        }*/

        for (int i = 0; i < 8*(ammoReference + extraAmmo); i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject LSG_Bullet = Instantiate(shotgunBullet, bulletPoolParent.transform);
            bulletPool.Add(LSG_Bullet);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }
    }

	void Update () {
		myTime = myTime + Time.deltaTime;
        AmmoCount.text = Ammo + "\n" + extraAmmo;

        if (!GM.prevState.IsConnected) {

            if (Input.GetKeyDown(KeyCode.R) && Input.GetMouseButton(0) && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Left_Reload());
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButton (0) && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
			    nextFire = myTime + fireDelta;
                shellCasingParticleSystem.Play();
                MuzzleFlash.Play ();
                Shoot();
			    Ammo--;
			    nextFire = nextFire - myTime;
			    myTime = 0.0f;
            }

            if (Input.GetMouseButtonUp(0)) {
                shellCasingParticleSystem.Stop();
            }
        } else if (GM.prevState.IsConnected) {

            if (GM.prevState.Triggers.Left > 0.45f && GM.prevState.Buttons.X == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.X == XInputDotNetPure.ButtonState.Pressed && Reloading == false && dropped != true && Player.canMove == true && Ammo < ammoReference) {
                StartCoroutine(Player.Left_Reload());
                StartCoroutine(Reload());
            }

            if (GM.prevState.Triggers.Left > 0.45f && myTime > nextFire && Ammo > 0 && Reloading != true && dropped != true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                shellCasingParticleSystem.Play();

                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }
            if (GM.prevState.Triggers.Left < 0.45f ) {
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
		Debug.Log ("Shooting");
        audioSource.PlayOneShot(shotSound);
        for (int i = 0; i<8;i++){

            if (bulletPool[poolCount] == null) {
                GameObject LSG_Bullet = Instantiate(shotgunBullet);
                bulletPool[poolCount] = LSG_Bullet;
                bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
                bulletPool[poolCount].SetActive(false);
            }

            bulletPool[poolCount].SetActive(true);
            bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = false;
            bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
            bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
            bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = true;
            float yAxis = Random.Range(-0.15f,0.15f);
            float xAxis = Random.Range(-0.15f,0.15f);
            Vector3 forceDirection = -transform.right;
            forceDirection = new Vector3(forceDirection.x + xAxis, forceDirection.y + yAxis, forceDirection.z);
            
            bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(forceDirection * 1000f, ForceMode.VelocityChange);
            StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(1.0f));

            /*if (poolCount >= bulletPool.Count - 1) {
                poolCount = 0;
            } else {
                poolCount++;
            }*/
            poolCount++;
        }

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
		Destroy(this.gameObject, 10.0f);
		Destroy(topObject, 10.0f);
	}

    void DestroyBullets() {
        for (int i = 0; i < bulletPool.Count; i++) {
            Destroy(bulletPool[i]);
        }
    }
}
