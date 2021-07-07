using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShoulderAML : MonoBehaviour
{
    public GameManager GM;
    public PlayerController Player;
    public GameObject missileObject;
    public int Ammo = 15;
    private int ammoReference;
    public int extraAmmo;
    public int magazineCount;
    private float fireDelta = 0.35f;
    private float nextFire = 0.35f;
    private float myTime = 0.0f;
    public GameObject ammoTextObject;
    public TextMeshProUGUI AmmoText;
    public GameObject ReloadImage;
    public WeaponStatus WS = WeaponStatus.ReadytoFire;

    [Header("Colors")]
    public Color green;
    public Color yellow;
    public Color red;

    public GameObject misslePoolParent;
    public List<GameObject> missileSpawn = new List<GameObject>();
    public List<GameObject> missilePool = new List<GameObject>();
    [SerializeField]
    private int missleCount;
    private int missleSpawnCount;

    public AudioSource audioSource;
    public bool Dropped = false;
    public GameObject jetesinedParticles;

    [Header("Explosive Pieces")]
    public GameObject explosionObject;
    public List<GameObject> explosionObjectArray = new List<GameObject>();
    public GameObject explosionSpawnParent;

    void Awake() {
        //AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        misslePoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");
        explosionSpawnParent = GameObject.FindGameObjectWithTag("MissileExplosionParent");
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        Player.ActivateObject(Player.shieldEmitterObjects, 0);
        /*for (int i = 0; i < missilePool.Count; i++) {
            GameObject AML_Missle = Instantiate(missileObject);
            missilePool[i] = AML_Missle;
            missilePool[i].transform.parent = missileSpawn[i].transform;
            missilePool[i].GetComponent<AMLMissile>().Parent = missileSpawn[i];
            missilePool[i].SetActive(false);
        }
        for (int i = 0; i < missilePool.Count; i++) {
            GameObject S_Explosion = Instantiate(explosionObject);
            explosionObjectArray[i] = S_Explosion;
            explosionObjectArray[i].transform.parent = explosionSpawnParent.transform;
            explosionObjectArray[i].SetActive(false);
        }*/

        for (int i = 0; i < ammoReference * extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject S_MISSILE = Instantiate(missileObject, misslePoolParent.transform);
            missilePool.Add(S_MISSILE);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            missilePool[i].transform.position = Vector3.zero;
            missilePool[i].SetActive(false);
        }


        for (int i = 0; i < ammoReference * extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject S_EXPLOSION = Instantiate(explosionObject, misslePoolParent.transform);
            explosionObjectArray.Add(S_EXPLOSION);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            explosionObjectArray[i].transform.position = Vector3.zero;
            explosionObjectArray[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + missilePool.Count);
    }

    void Update() {
        myTime = myTime + Time.deltaTime;
        if (AmmoText.gameObject.activeSelf == true) {
            AmmoText.text = Ammo.ToString() + " - " + extraAmmo;
        }

        if (Dropped == false) {
            if (Input.GetKey(KeyCode.F) && Ammo > 0 && myTime > nextFire && Player.canMove == true && WS == WeaponStatus.ReadytoFire) {
                nextFire = myTime + fireDelta;
                WS = WeaponStatus.Firing;
                StartCoroutine(Shoot());
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }


            if (GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && Ammo > 0 && myTime > nextFire && Player.canMove == true && WS == WeaponStatus.ReadytoFire) {
                nextFire = myTime + fireDelta;
                WS = WeaponStatus.Firing;
                StartCoroutine(Shoot());
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }
        }

        if (Ammo <= ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == false && WS == WeaponStatus.Firing) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 1);
            if (WS != WeaponStatus.Reloading) {
                Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }

        if (Ammo <= 0 && WS != WeaponStatus.Reloading && extraAmmo != 0) {
            StartCoroutine(reload());
        }

        if (Ammo <= 0 && extraAmmo <= 0 && Dropped != true) {
            StartCoroutine(BreakOff());
        }
    }

    IEnumerator Shoot() {
        #region Object Pool

        missleSpawnCount = 0;
        WS = WeaponStatus.Firing;
        //Debug.Log("BF Missile Count " + missleCount);
        //Debug.Log("Missile Count Equation" + missleCount/15);
        //for (int i = 0; i< missilePool.Count; i++) {
        while (Ammo > 0){
            Debug.Log("AF Missile Count Equation" + missleCount / 15);
            Debug.Log("Missile Count " + missleCount);
            missilePool[missleCount].SetActive(true);
            audioSource.PlayOneShot(audioSource.clip);
            missilePool[missleCount].transform.position = missileSpawn[missleSpawnCount].transform.position;
            //missilePool[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            //missilePool[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //missilePool[missleCount].transform.parent = null;
            missilePool[missleCount].GetComponent<Rigidbody>().isKinematic = false;
            missilePool[missleCount].GetComponent<Rigidbody>().velocity = Vector3.zero;
            missilePool[missleCount].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            missilePool[missleCount].GetComponent<Rigidbody>().AddForce(missileSpawn[missleSpawnCount].transform.forward * 70f, ForceMode.VelocityChange);
            StartCoroutine(missilePool[missleCount].GetComponent<AMLMissile>().GetTarget());
            explosionObjectArray[missleCount].SetActive(true);
            missilePool[missleCount].GetComponent<AMLMissile>().chosenExplosion = explosionObjectArray[missleCount];
            /*if(missilePool[missleCount].GetComponent<AMLMissile>().parentSpawn == null) {
                missilePool[missleCount].GetComponent<AMLMissile>().parentSpawn = misslePoolParent;
            }*/
            Ammo--;
            missleCount++;
            missleSpawnCount++;
            yield return new WaitForSeconds(0.15f);
            //missilePool[i].GetComponent<Missle>().lockedOn = true;
        }

        /*//Debug.Log("Shooting");
        if (Player.targetedLeftEnemy != null || Player.targetedRightEnemy != null) {
            //GameObject Missle_I = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            misslePool[missleCount].GetComponent<Missle>().target = Player.targetedLeftEnemy.transform;
            misslePool[missleCount].GetComponent<Missle>().lockedOn = true;
            misslePool[missleCount].SetActive(true);

            //Destroy (Missle_I, 10.0f);
        } else {
            //GameObject Missle_II = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            misslePool[missleCount].transform.rotation = missleSpawn.transform.rotation;
            misslePool[missleCount].GetComponent<Rigidbody>().AddForce(missleSpawn.transform.forward * 50f, ForceMode.VelocityChange);
            misslePool[missleCount].SetActive(true);
            StartCoroutine(misslePool[missleCount].GetComponent<Missle>().DestroyMissle(10f, misslePool[missleCount].transform.position));
            //Destroy (Missle_II, 10.0f);
        }


        if (missleCount >= missilePool.Count - 1) {
            //Debug.Log("pool count reset");
            missleCount = 0;
        } else {
            //Debug.Log("pool count add 1");
            missleCount++;
        }*/
        #endregion
    }

    private IEnumerator reload() {
        Debug.Log("Reloading");
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "RELOADING";
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;


        WS = WeaponStatus.Reloading;
        yield return new WaitForSeconds(2.0f);
        Ammo = ammoReference;
        extraAmmo -= ammoReference;
        WS = WeaponStatus.ReadytoFire;

        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "CLEAR";
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = green;
        yield return new WaitForSeconds(1.98f);
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);

    }

    private IEnumerator BreakOff() {
        Dropped = true;
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "DROPPED";
        Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = red;
        yield return new WaitForSeconds(0.50f);
        this.gameObject.transform.parent = null;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Player.immediateGroundMoveForce += 0.25f;
        jetesinedParticles.SetActive(true);
        this.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 3.0f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(10.0f);
        this.gameObject.SetActive(false);
    }
}
