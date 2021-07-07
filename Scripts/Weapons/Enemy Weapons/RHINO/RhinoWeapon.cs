using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponSide{LeftArm, RightArm };

public class RhinoWeapon : MonoBehaviour {

    [Header("Rover Enemy Class")]
    public Rover_Enemy RE;
    public WeaponSide WS;
    public GameObject ShotSpawn;
    private float fireDelta = 0.35f;
    private float nextFire = 0.35f;
    private float myTime = 0.0f;
    public GameObject enemyBullet;
    public int Ammo = 30;
    private int ammoReference;
    private int extraAmmo;
    public int magazineCount;
    public bool Reloading;

    [Header("Muzzle Effects")]
    public ParticleSystem MuzzleFlash;
    public AudioSource audioSource;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    [Header("Left Arm Animation")]
    public Animator anim;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            GameObject enemy_AR_Bullet = Instantiate(enemyBullet, bulletPoolParent.transform);
            bulletPool.Add(enemy_AR_Bullet);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + bulletPool.Count);
    }
	
	void Update () {
        myTime = myTime + Time.deltaTime;

        if(RE.canAttack == true) {
            if (myTime > nextFire && Ammo > 0 && Reloading != true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                audioSource.PlayOneShot(audioSource.clip);
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            if(Ammo <= 0 && Reloading != true) {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot() {
        #region Object Pool

        if(RE.distanceToPlayer > 20f){
            if (bulletPool[poolCount] == null) {
                GameObject Bullet = Instantiate(enemyBullet);
                bulletPool[poolCount] = Bullet;
                bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
                bulletPool[poolCount].SetActive(false);
            }

            bulletPool[poolCount].transform.position = ShotSpawn.transform.position;
            bulletPool[poolCount].SetActive(true);
            //bulletPool[poolCount].transform.rotation = ShotSpawn.transform.rotation;
            bulletPool[poolCount].GetComponent<Rigidbody>().AddForce (ShotSpawn.transform.forward * 500f, ForceMode.VelocityChange);
            bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = true;
            StartCoroutine(bulletPool[poolCount].GetComponent<EnemyBullet>().WaitDestroy(1.0f));
            if (poolCount >= bulletPool.Count - 1) {
                Debug.Log("pool count reset");
                poolCount = 0;
            } else {
                Debug.Log("pool count add 1");
                poolCount++;
            }
        } else {
            RaycastHit shotHit;
            if (Physics.Raycast(ShotSpawn.transform.position, ShotSpawn.transform.forward, out shotHit, 25f)) {

                Debug.Log("Hit object: " + shotHit.transform.gameObject);
                if (shotHit.collider.tag == "Player") {
                    shotHit.collider.gameObject.GetComponent<PlayerController>().Hit(7);
                }
            }
        }
        #endregion
    }

    private IEnumerator Reload() {
        Reloading = true;
        if(WS == WeaponSide.LeftArm){
            Debug.Log("Reload Left");
            anim.SetBool("leftArmReload", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("leftArmReload", false);
        }

        if (WS == WeaponSide.RightArm) {
            Debug.Log("Reload Right");
            anim.SetBool("rightArmReload", true);
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("rightArmReload", false);
        }
        yield return new WaitForSeconds(0.7f);
        Ammo = ammoReference;
        yield return new WaitForSeconds(0.5f);
        Reloading = false;
    }
}
