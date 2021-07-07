using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public enum RailgunCharge{Low, Normal, OverCharged };

public class Railgun : MonoBehaviour
{
    
    [Header("Game Manager")]
    public GameManager GM;

    [Header("Player")]
    public PlayerController Player;

    [Header("Bullet Items")]
    public GameObject shotSpawn;
    public GameObject railGunShot;

    [Header("Ammo and Reloading")]
    public int Ammo = 5;
    private int ammoReference;
    [SerializeField]
    private int extraAmmo;
    public int magazineCount;
    public GameObject ammoTextObject;
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI RailgunChargePercentageText;
    //public GameObject ReloadImage;
    public float chargeLevel;
    public bool Charging;
    public bool isCharged;

    [Header("Aniamtions")]
    public Animator Anim;
    [EnumToggleButtons]
    public CannonStatus currentStatus;
    public RailgunCharge RC;

    [Header("Main Body")]
    public GameObject CannonBody;

    [Header("Cannon fire status")]
    [SerializeField]
    private bool canFire = false;
    private bool Reloading = false;

    [Header("Cannon Effects")]
    public ParticleSystem MuzzleFlash;
    public ParticleSystem railgunChargeParticles;
    public ParticleSystem railgunOverChargeParticles;
    public AudioSource chargingNoiseAS;
    public AudioSource firingNoiseAS;
    

    [Header("Colors")]
    public Color green;
    public Color yellow;
    public Color red;

    public GameObject cannonPoolParent;
    public List<GameObject> cannonPool = new List<GameObject>();
    [SerializeField]
    private int cannonCount;

    public bool Dropped = false;
    public GameObject jetesinedParticles;
    public GameObject CannonLasers;
    // Use this for initialization
    void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        //ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
        //ReloadImage.SetActive(false);
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        cannonPoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        Player.ActivateObject(Player.shieldEmitterObjects, 0);
        /*for (int i = 0; i < cannonPool.Count; i++) {
            GameObject S_Cannon = Instantiate(railGunShot);
            cannonPool[i] = S_Cannon;
            cannonPool[i].transform.parent = cannonPoolParent.transform;
            cannonPool[i].SetActive(false);
        }*/

        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject S_Cannon = Instantiate(railGunShot, cannonPoolParent.transform);
            cannonPool.Add(S_Cannon);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            cannonPool[i].transform.position = Vector3.zero;
            cannonPool[i].SetActive(false);
        }
    }

    private void Start() {
        RailgunChargePercentageText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        RailgunChargePercentageText.text = chargeLevel.ToString() + "%";
        Debug.DrawRay(shotSpawn.transform.position, shotSpawn.transform.up, Color.red);
        if (AmmoText.gameObject.activeSelf == true) {
            AmmoText.text = "0" + Ammo.ToString() + " - " + extraAmmo;
        }

        if (chargeLevel > 75f) {
            canFire = true;
        } else {
            canFire = false;
        }

        if (!GM.prevState.IsConnected) {
            if (Input.GetKeyDown(KeyCode.E) && currentStatus == CannonStatus.Retracted && Player.canMove == true) {
                StartCoroutine(WeaponUp());
            } else if (Input.GetKeyDown(KeyCode.E) && currentStatus == CannonStatus.Deployed && Player.canMove == true) {
                currentStatus = CannonStatus.Retracted;
                //CannonLasers.SetActive(false);
                Anim.SetBool("Bring Up", false);
                Anim.SetBool("Put Away", true);
            }

            if (Input.GetKeyUp(KeyCode.R) && Player.canMove == true && Charging == true) {
                Charging = false;
                chargingNoiseAS.Stop();
                Debug.Log("Railgun False");
            }else if (Input.GetKeyDown(KeyCode.R) && Player.canMove == true && chargeLevel < 149f && Charging == false) {
                railgunChargeParticles.gameObject.SetActive(true);
                Charging = true;
                if (chargingNoiseAS.isPlaying == false) {
                    chargingNoiseAS.Play();
                }
                Debug.Log("Going into Railgun Charge");
                StartCoroutine(RailgunRecharge());
            }        

            if (Input.GetKeyDown(KeyCode.F) && Ammo > 0 && Player.canMove == true && canFire == true && currentStatus == CannonStatus.Deployed) {
                MuzzleFlash.Play();
                Charging = false;
                chargeLevel = 0;
                Shoot();
            }
        }
        if (GM.prevState.IsConnected) {
            if (GM.prevState.Buttons.Y == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.Y == XInputDotNetPure.ButtonState.Pressed && currentStatus == CannonStatus.Retracted && Player.canMove == true) {
                StartCoroutine(WeaponUp());
            } else if (GM.prevState.Buttons.Y == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.Y == XInputDotNetPure.ButtonState.Pressed && currentStatus == CannonStatus.Deployed && Player.canMove == true) {
                currentStatus = CannonStatus.Retracted;
                //CannonLasers.SetActive(false);
                Anim.SetBool("Bring Up", false);
                Anim.SetBool("Put Away", true);
            }

            if (Player.canMove == true && Charging == true && GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released || Input.GetButtonUp("Reload")) {
                Charging = false;
                chargingNoiseAS.Stop();
                Debug.Log("Railgun False");
            } else if (GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && Input.GetButton("Reload") == true && Player.canMove == true && chargeLevel < 149f && Charging == false) {
                railgunChargeParticles.gameObject.SetActive(true);
                Charging = true;
                if (chargingNoiseAS.isPlaying == false) {
                    chargingNoiseAS.Play();
                }
                Debug.Log("Going into Railgun Charge");
                StartCoroutine(RailgunRecharge());
            }

            if (GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && Ammo > 0 && canFire == true && Player.canMove == true && currentStatus == CannonStatus.Deployed) {
                MuzzleFlash.Play();
                Shoot();
            }
        }
        //GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed

        if (Ammo <= ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == false && currentStatus == CannonStatus.Deployed) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 1);
            if (Reloading != true) {
                Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }

        if (Ammo <= 0 && extraAmmo != 0 && Reloading != true) {
            StartCoroutine(Reload());
        }

        if (Ammo <= 0 && extraAmmo == 0) {
            StartCoroutine(BreakOff());
        }
    }

    private void Shoot() {
        Debug.Log("Shooting");
        StartCoroutine(FireAnimation());

        Ammo--;

        if (cannonPool[cannonCount] == null) {
            GameObject LAR_Bullet = Instantiate(railGunShot);
            cannonPool[cannonCount] = LAR_Bullet;
            cannonPool[cannonCount].transform.parent = cannonPoolParent.transform;
            cannonPool[cannonCount].SetActive(false);
        }

        #region Object Pool

        Debug.Log("About to fire Left");

        cannonPool[cannonCount].transform.position = shotSpawn.transform.position;
        cannonPool[cannonCount].SetActive(true);
        StartCoroutine(cannonPool[cannonCount].GetComponent<RailgunShot>().WaitTillInActive(4.0f));
        cannonPool[cannonCount].transform.rotation = shotSpawn.transform.rotation;
        chargingNoiseAS.Stop();
        firingNoiseAS.PlayOneShot(firingNoiseAS.clip);
        switch (RC) {
            case RailgunCharge.Low:
                Debug.Log("Fired Low Power");
                cannonPool[cannonCount].GetComponent<Rigidbody>().AddForce(shotSpawn.transform.forward * 750f, ForceMode.VelocityChange);
                cannonPool[cannonCount].GetComponent<RailgunShot>().RC = RC;
                break;

            case RailgunCharge.Normal:
                Debug.Log("Fired normal Power");
                cannonPool[cannonCount].GetComponent<Rigidbody>().AddForce(shotSpawn.transform.forward * 800f, ForceMode.VelocityChange);
                cannonPool[cannonCount].GetComponent<RailgunShot>().RC = RC;
                break;

            case RailgunCharge.OverCharged:
                Debug.Log("Fired Overcharged Power");
                cannonPool[cannonCount].GetComponent<Rigidbody>().AddForce(shotSpawn.transform.forward * 850f, ForceMode.VelocityChange);
                cannonPool[cannonCount].GetComponent<RailgunShot>().RC = RC;
                break;
        }

        Debug.Log("Just fired Railgun");

        /*if (cannonCount >= cannonPool.Count - 1) {
            Debug.Log("pool count reset");
            cannonCount = 0;
        } else {
            Debug.Log("pool count add 1");
            cannonCount++;
        }*/
        cannonCount++;
        canFire = false;
        #endregion
        RailgunChargePercentageText.color = yellow;
        chargeLevel = 0;
        railgunOverChargeParticles.gameObject.SetActive(false);
        railgunChargeParticles.gameObject.SetActive(false);
    }

    public IEnumerator RailgunRecharge() {
        if(chargeLevel == 0f) {
            Charging = true;
        }
        Debug.Log("Going into while loop");
        while (chargeLevel < 150 && Charging == true) {
            if(chargeLevel < 75){
                RailgunChargePercentageText.color = yellow;
                chargeLevel += 1.5f;
            } else if(chargeLevel > 74 && chargeLevel < 100){
                RailgunChargePercentageText.color = yellow;
                RC = RailgunCharge.Low;
                chargeLevel += 1.5f;
            } else if (chargeLevel > 100 && chargeLevel < 120) {
                RailgunChargePercentageText.color = green;
                RC = RailgunCharge.Normal;
                chargeLevel += 1.2f;
            }else if (chargeLevel > 120 && chargeLevel < 149) {
                RailgunChargePercentageText.color = red;
                RC = RailgunCharge.OverCharged;
                railgunOverChargeParticles.gameObject.SetActive(true);
                chargeLevel += 1.2f;
            } else if (chargeLevel > 150) {
                chargeLevel = 150f;
            }
            yield return new WaitForSeconds(0.10f);
        }
        Charging = false;
    }

    private IEnumerator Reload() {
        Reloading = true;
        Debug.Log("Reloading");
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;

        yield return new WaitForSeconds(2.0f);
        Ammo = ammoReference;
        extraAmmo -= ammoReference;
        Reloading = false;
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "CLEAR";
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = green;
        yield return new WaitForSeconds(1.98f);
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
    }

    private IEnumerator WeaponUp() {
        Anim.SetBool("Put Away", false);
        Anim.SetBool("Bring Up", true);
        yield return new WaitForSeconds(3.00f);
        //CannonLasers.SetActive(true);
        currentStatus = CannonStatus.Deployed;

    }

    private IEnumerator FireAnimation() {
        Debug.Log("Firing");
        canFire = false;
        Anim.SetBool("Fired", true);
        yield return new WaitForSeconds(4.10f);
        Anim.SetBool("Fired", false);
        canFire = true;
    }

    private IEnumerator BreakOff() {
        Dropped = true;
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "DROPPED";
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        yield return new WaitForSeconds(0.50f);
        this.gameObject.transform.parent = null;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        jetesinedParticles.SetActive(true);
        this.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 0.2f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<CapsuleCollider>().enabled = true;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        yield return new WaitForSeconds(10.0f);
        this.gameObject.SetActive(false);
    }
}
