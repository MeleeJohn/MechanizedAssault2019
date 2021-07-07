using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : MonoBehaviour
{

    [Header("Rover Enemy Class")]
    public Rover_Enemy RE;
    public GameObject ShotSpawn;
    private float fireDelta = 0.75f;
    private float nextFire = 0.75f;
    private float myTime = 0.0f;
    public GameObject shotgunBullet;
    public int Ammo = 12;
    private int ammoReference;
    private int extraAmmo;
    public int magazineCount;
    public bool Reloading;

    [Header("Shotgun Spread")]
    public float shotSpreadAngle = 0.1f;

    [Header("Muzzle Effects")]
    public ParticleSystem MuzzleFlash;
    public AudioSource audioSource;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    [Header("Left Arm Animation")]
    public Animator anim;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        anim = RE.gameObject.GetComponent<Animator>();
        ammoReference = Ammo;
        extraAmmo = Ammo * 8 * magazineCount;
        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            GameObject enemySG_Bullet = Instantiate(shotgunBullet, bulletPoolParent.transform);
            bulletPool.Add(enemySG_Bullet);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + bulletPool.Count);
    }

    void Update() {
        myTime = myTime + Time.deltaTime;

        if (RE.canAttack == true) {
            if (myTime > nextFire && Ammo > 0 && Reloading != true) {
                nextFire = myTime + fireDelta;
                MuzzleFlash.Play();
                audioSource.PlayOneShot(audioSource.clip);
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }

            if (Ammo <= 0 && Reloading != true) {
                StartCoroutine(Reload());
            }
        }
    }

    private void Shoot() {
        Debug.Log("Shooting");
        audioSource.PlayOneShot(audioSource.clip);
        for (int i = 0; i < 8; i++) {

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
            float yAxis = Random.Range(-0.12f, 0.12f);
            float xAxis = Random.Range(-0.12f, 0.12f);
            Vector3 forceDirection = -transform.right;
            forceDirection = new Vector3(forceDirection.x + xAxis, forceDirection.y + yAxis, forceDirection.z);

            bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(forceDirection * 250f, ForceMode.VelocityChange);
            StartCoroutine(bulletPool[poolCount].GetComponent<EnemyBullet>().WaitDestroy(1.0f));

            /*if (poolCount >= bulletPool.Count - 1) {
                poolCount = 0;
            } else {
                poolCount++;
            }*/
            poolCount++;
        }

    }

    private IEnumerator Reload() {
        Reloading = true;
        Debug.Log("Reload Right");
        anim.SetBool("rightArmReload", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("rightArmReload", false);
        yield return new WaitForSeconds(0.7f);
        Ammo = ammoReference;
        yield return new WaitForSeconds(0.5f);
        Reloading = false;
    }
}
