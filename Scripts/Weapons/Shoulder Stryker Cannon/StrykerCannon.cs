 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CurrentBarrel{Barrel1, Barrel2 };

public class StrykerCannon : MonoBehaviour
{
    public GameManager GM;
    public PlayerController PC;
    public WeaponStatus WS;
    public CannonStatus CS;
    public CurrentBarrel CB;
    public TextMeshProUGUI AmmoText;
    public bool isTargeting;
    public Animator Anim;
    public GameObject strykerBullet;
    public int Ammo = 40;
    private int ammoReference = 40;
    [SerializeField]
    private int extraAmmo;
    private int magazineCount = 3;
    public GameObject ammoTextObject;

    public GameObject weaponSystem;
    [Header("Colors")]
    public Color green;
    public Color yellow;
    public Color red;

    [Header("Fire rate items")]
    private float fireDelta = 0.25f;
    private float nextFire = 0.25f;
    private float myTime = 0.0f;

    [Header("Spawn points")]
    public GameObject bulletSpawn01;
    public GameObject bulletSpawn02;

    [Header("Muzzle Effects")]
    public ParticleSystem MuzzleFlash_1;
    public ParticleSystem MuzzleFlash_2;
    public AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip reloadSound;

    public GameObject targetedEnemy;
    public float enemyDistance;

    public GameObject bulletPoolParent;
    public List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField]
    private int poolCount;

    public List<GameObject> bulletPool_2 = new List<GameObject>();
    [SerializeField]
    private int poolCount_2;

    public bool Dropped = false;
    public GameObject jetesinedParticles;
    void Start()
    {
        //AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        bulletPoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");
        WS = WeaponStatus.ReadytoFire;
        PC.ActivateObject(PC.shoulderWeaponLowAmmoNotice, 0);
        PC.ActivateObject(PC.shieldEmitterObjects, 0);

        for (int i = 0; i < ammoReference; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject stryker_Bullet_1 = Instantiate(strykerBullet, bulletPoolParent.transform);
            bulletPool.Add(stryker_Bullet_1);
            bulletPool[i].transform.position = Vector3.zero;
            bulletPool[i].SetActive(false);
        }


        for (int i = 0; i < ammoReference; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject stryker_Bullet_2 = Instantiate(strykerBullet, bulletPoolParent.transform);
            bulletPool_2.Add(stryker_Bullet_2);
            bulletPool_2[i].transform.position = Vector3.zero;
            bulletPool_2[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        myTime = myTime + Time.deltaTime;
        if (AmmoText.gameObject.activeSelf == true) {
            AmmoText.text = "0" + Ammo.ToString() + " - " + extraAmmo;
        }
        if (PC.Targeting == true && CS == CannonStatus.Deployed) {
            if(PC.targetedLeftEnemy != null){
                enemyDistance = Vector3.Distance(this.transform.position,PC.targetedLeftEnemy.transform.position);
            } else if (PC.targetedRightEnemy != null) {
                enemyDistance = Vector3.Distance(this.transform.position, PC.targetedRightEnemy.transform.position);
            }
            if (enemyDistance < 240f && enemyDistance > 40f) {
                isTargeting = true;
                TargetEnemy();
            } else {
                isTargeting = false;
            }
        } else {
            isTargeting = false;
        }

        if(isTargeting == true && CS == CannonStatus.Deployed) {
            Debug.Log("SAC Running isTargeting");
            //&& WS == WeaponStatus.ReadytoFire && CS == CannonStatus.Deployed && dropped != true && PC.canMove == true
            if (myTime > nextFire && WS == WeaponStatus.ReadytoFire && CS == CannonStatus.Deployed && Dropped != true && PC.canMove == true && Ammo > 0) {
                Debug.Log("SAC Running Shoot");
                nextFire = myTime + fireDelta;
                Shoot();
                Ammo--;
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }
        }

        if(Ammo <= 0 && WS != WeaponStatus.Reloading && extraAmmo != 0) {
            StartCoroutine(Reload());
        } else if (Ammo<=0 && extraAmmo <=0) {
            StartCoroutine(BreakOff());
        }

        if (Input.GetKeyDown(KeyCode.E) && CS == CannonStatus.Retracted && PC.canMove == true) {
            StartCoroutine(WeaponUp());
        } else if (Input.GetKeyDown(KeyCode.E) && CS == CannonStatus.Deployed && PC.canMove == true) {
            CS = CannonStatus.Retracted;
            Anim.SetBool("SAC_up", false);
            Anim.SetBool("SAC_down", true);
        }

        if (GM.prevState.Buttons.Y == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.Y == XInputDotNetPure.ButtonState.Pressed && CS == CannonStatus.Retracted && PC.canMove == true) {
            StartCoroutine(WeaponUp());
        } else if (GM.prevState.Buttons.Y == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.Y == XInputDotNetPure.ButtonState.Pressed && CS == CannonStatus.Deployed && PC.canMove == true) {
            CS = CannonStatus.Retracted;
            Anim.SetBool("SAC_up", false);
            Anim.SetBool("SAC_down", true);

        }

        if (Ammo <= ammoReference / 4 && PC.shoulderWeaponLowAmmoNotice.activeSelf == false && WS == WeaponStatus.ReadytoFire) {
            PC.ActivateObject(PC.shoulderWeaponLowAmmoNotice, 1);
            if (WS != WeaponStatus.Reloading) {
                PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }
    }

    void TargetEnemy() {
        Debug.Log("SAC Enemy Targeting");
        weaponSystem.transform.LookAt(PC.targetedLeftEnemy.transform,Vector3.up);
    }
    
    void Shoot() {
        switch (CB) {
            case CurrentBarrel.Barrel1:
                #region Object Pool
                Debug.Log("SAC Shoot shooting barrel 1");
                if (bulletPool[poolCount] == null) {
                    GameObject LMG_Bullet = Instantiate(strykerBullet);
                    bulletPool[poolCount] = LMG_Bullet;
                    bulletPool[poolCount].transform.parent = bulletPoolParent.transform;
                    bulletPool[poolCount].SetActive(false);
                }
                MuzzleFlash_1.Play();
                audioSource.PlayOneShot(shotSound);
                bulletPool[poolCount].SetActive(true);
                bulletPool[poolCount].GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                bulletPool[poolCount].GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = false;
                bulletPool[poolCount].transform.position = bulletSpawn01.transform.position;
                bulletPool[poolCount].GetComponent<TrailRenderer>().enabled = true;
                bulletPool[poolCount].transform.rotation = bulletSpawn01.transform.rotation;
                bulletPool[poolCount].GetComponent<Rigidbody>().AddForce(bulletSpawn01.transform.forward * 1450f, ForceMode.VelocityChange);
                StartCoroutine(bulletPool[poolCount].GetComponent<Bullet>().WaitInActive(0.8f));

                /*if (poolCount >= bulletPool.Count - 1) {
                    poolCount = 0;
                } else {
                    poolCount++;
                }*/

                poolCount++;
                #endregion
                CB = CurrentBarrel.Barrel2;
                break;

            case CurrentBarrel.Barrel2:
                #region Object Pool
                Debug.Log("SAC Shoot shooting barrel 2");
                if (bulletPool_2[poolCount_2] == null) {
                    GameObject LMG_Bullet_2 = Instantiate(strykerBullet);
                    bulletPool_2[poolCount_2] = LMG_Bullet_2;
                    bulletPool_2[poolCount_2].transform.parent = bulletPoolParent.transform;
                    bulletPool_2[poolCount_2].SetActive(false);
                }
                MuzzleFlash_2.Play();
                audioSource.PlayOneShot(shotSound);
                bulletPool_2[poolCount_2].SetActive(true);
                bulletPool_2[poolCount_2].GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                bulletPool_2[poolCount_2].GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
                bulletPool_2[poolCount_2].GetComponent<TrailRenderer>().enabled = false;
                bulletPool_2[poolCount_2].transform.position = bulletSpawn02.transform.position;
                bulletPool_2[poolCount_2].GetComponent<TrailRenderer>().enabled = true;
                bulletPool_2[poolCount_2].transform.rotation = bulletSpawn02.transform.rotation;
                bulletPool_2[poolCount_2].GetComponent<Rigidbody>().AddForce(bulletSpawn02.transform.forward * 1450f, ForceMode.VelocityChange);
                StartCoroutine(bulletPool[poolCount_2].GetComponent<Bullet>().WaitInActive(0.8f));

                /*if (poolCount_2 >= bulletPool_2.Count - 1) {
                    poolCount_2 = 0;
                } else {
                    poolCount_2++;
                }*/
                poolCount_2++;
                CB = CurrentBarrel.Barrel1;

                #endregion
                break;
        }
    }

    IEnumerator WeaponUp() {
        yield return new WaitForSeconds(0.0f);
        CS = CannonStatus.Deployed;
        WS = WeaponStatus.ReadytoFire;
        Anim.SetBool("SAC_up", true);
        Anim.SetBool("SAC_down", false);
        Debug.Log("SAC Weapon up");
    }

    public IEnumerator Reload() {

        Debug.Log("Reloading");
        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;


        WS = WeaponStatus.Reloading;
        yield return new WaitForSeconds(2.0f);
        audioSource.PlayOneShot(reloadSound);
        Ammo = ammoReference;
        extraAmmo -= ammoReference;
        WS = WeaponStatus.ReadytoFire;

        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "CLEAR";
        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = green;
        yield return new WaitForSeconds(1.98f);
        PC.ActivateObject(PC.shoulderWeaponLowAmmoNotice, 0);
    }

    private IEnumerator BreakOff() {
        Dropped = true;
        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "DROPPED";
        PC.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        yield return new WaitForSeconds(0.50f);
        this.gameObject.transform.parent = null;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        jetesinedParticles.SetActive(true);
        this.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 0.2f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(10.0f);
        this.gameObject.SetActive(false);
    }
}
