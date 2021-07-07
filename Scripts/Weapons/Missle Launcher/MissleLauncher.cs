using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum WeaponStatus{Reloading, ReadytoFire, Firing};

public class MissleLauncher : MonoBehaviour {

    public GameManager GM;
    public PlayerController Player;
	public GameObject missleLauncher_Y;
	public GameObject missleLauncher_X;
	public GameObject missleSpawn;
	public GameObject missleObject;
	public int Ammo = 12;
    private int ammoReference;
    public int extraAmmo;
    public int magazineCount;
    private float fireDelta = 0.25f;
    private float nextFire = 0.25f;
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
    public List<GameObject> misslePool = new List<GameObject>();
    [SerializeField]
    private int missleCount;

    public AudioSource audioSource;
    public bool Dropped = false;
    public GameObject jetesinedParticles;

    [Header("Explosive Pieces")]
    public GameObject explosionObject;
    public List<GameObject> explosionObjectArray = new List<GameObject>();
    public GameObject explosionSpawnParent;

    void Awake () {
		//AmmoText = GameObject.FindGameObjectWithTag("LeftShoulderText").GetComponent<Text>();
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		ReloadImage = GameObject.FindGameObjectWithTag("LeftShoulderReloadImage");
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
		//ReloadImage.SetActive(false);
        ammoReference = Ammo;
        extraAmmo = Ammo * magazineCount;
        misslePoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");
        explosionSpawnParent = GameObject.FindGameObjectWithTag("MissileExplosionParent");
        Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 0);
        Player.ActivateObject(Player.shieldEmitterObjects, 0);
        /*for (int i = 0; i < misslePool.Count; i++) {
            GameObject S_Missle = Instantiate(missleObject);
            misslePool[i] = S_Missle;
            misslePool[i].transform.parent = misslePoolParent.transform;
            misslePool[i].SetActive(false);
        }*/

        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject S_MISSILE = Instantiate(missleObject, misslePoolParent.transform);
            misslePool.Add(S_MISSILE);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            misslePool[i].transform.position = Vector3.zero;
            misslePool[i].SetActive(false);
        }

        /*for (int i = 0; i < misslePool.Count; i++) {
            GameObject S_Explosion = Instantiate(explosionObject);
            explosionObjectArray[i] = S_Explosion;
            explosionObjectArray[i].transform.parent = explosionSpawnParent.transform;
            explosionObjectArray[i].SetActive(false);
        }*/

        for (int i = 0; i < ammoReference + extraAmmo; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject S_EXPLOSION = Instantiate(explosionObject, explosionSpawnParent.transform);
            explosionObjectArray.Add(S_EXPLOSION);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            explosionObjectArray[i].transform.position = Vector3.zero;
            explosionObjectArray[i].SetActive(false);
        }
        Debug.Log("bulletPool count is " + misslePool.Count);
    }
	
	void Update ()
	{
        myTime = myTime + Time.deltaTime;
        if (AmmoText.gameObject.activeSelf == true) {
			AmmoText.text = Ammo.ToString() + " - " + extraAmmo;
		}
        if (Player.targetedLeftEnemy != null || Player.targetedRightEnemy != null) {
			Tracking ();
		} else {
			missleLauncher_Y.transform.localEulerAngles = new Vector3(0f,0f,0f);
		}
        if(Dropped == false){
		    if (Input.GetKey (KeyCode.F) && Ammo > 0 && myTime > nextFire && Player.canMove == true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                Shoot ();
                audioSource.PlayOneShot(audioSource.clip);
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }


            if (GM.prevState.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.LeftShoulder == XInputDotNetPure.ButtonState.Pressed && Ammo > 0 && myTime > nextFire && Player.canMove == true && Player.canMove == true) {
                nextFire = myTime + fireDelta;
                Shoot();
                audioSource.Play();
                nextFire = nextFire - myTime;
                myTime = 0.0f;
            }
        }

        if (Ammo <= ammoReference / 4 && Player.shoulderWeaponLowAmmoNotice.activeSelf == false && WS == WeaponStatus.ReadytoFire) {
            Player.ActivateObject(Player.shoulderWeaponLowAmmoNotice, 1);
            if (WS != WeaponStatus.Reloading) {
                Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "LOW AMMO";
                Player.shoulderWeaponLowAmmoNotice.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = yellow;
            }
        }

        if (Ammo <= 0 && WS != WeaponStatus.Reloading && extraAmmo != 0) {
			StartCoroutine(reload());
		}

        if (Ammo <= 0 && extraAmmo == 0 && Dropped != true) {
            StartCoroutine(BreakOff());
        }
    }

	void Tracking(){
        /*Vector3 targetDirection = PC.targetedLeftEnemy.transform.position - missleLauncher_Y.transform.position;
		float angle = Mathf.Atan2 (-targetDirection.x, -targetDirection.y) * Mathf.Rad2Deg;
		Vector3 q = Quaternion.AngleAxis (angle, -Vector3.forward).eulerAngles;
        missleLauncher_Y.transform.rotation = Quaternion.Euler(new Vector3(0f, q.y, 0f));*/

        //Debug.Log("Tracking Updated");
        //missleLauncher_X.transform.rotation = Quaternion.Euler(new Vector3(q.x, 0f, 0f));
	}

	void Shoot ()
	{
        #region Object Pool
        Ammo--;
		//Debug.Log("Shooting");
        if (Player.targetedLeftEnemy != null || Player.targetedRightEnemy != null) {
			//GameObject Missle_I = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            if(Player.targetedLeftEnemy != null){
                misslePool[missleCount].GetComponent<Missle>().target = Player.targetedLeftEnemy.transform;
                misslePool[missleCount].GetComponent<Missle>().lockedOn = true;
            } else if(Player.targetedRightEnemy != null) {
                misslePool[missleCount].GetComponent<Missle>().target = Player.targetedRightEnemy.transform;
                misslePool[missleCount].GetComponent<Missle>().lockedOn = true;
            }
            misslePool[missleCount].SetActive(true);

            //Destroy (Missle_I, 10.0f);
        } else {
			//GameObject Missle_II = (GameObject)Instantiate (missleObject, missleSpawn.transform.position, missleSpawn.transform.rotation);
            misslePool[missleCount].transform.position = missleSpawn.transform.position;
            misslePool[missleCount].transform.rotation = missleSpawn.transform.rotation;
            misslePool[missleCount].GetComponent<Rigidbody> ().AddForce (missleSpawn.transform.forward * 50f, ForceMode.VelocityChange);
            misslePool[missleCount].SetActive(true);
            StartCoroutine(misslePool[missleCount].GetComponent<Missle>().DestroyMissle(10f, misslePool[missleCount].transform.position));
            //Destroy (Missle_II, 10.0f);
        }

        misslePool[missleCount].GetComponent<Missle>().chosenExplosion = explosionObjectArray[missleCount];

        /*if (missleCount >= misslePool.Count - 1) {
            //Debug.Log("pool count reset");
            missleCount = 0;
        } else {
            //Debug.Log("pool count add 1");
            missleCount++;
        }*/

        missleCount++;
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