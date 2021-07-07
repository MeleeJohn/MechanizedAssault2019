using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoulderRockets : MonoBehaviour
{
    public GameObject enemyRocket;
    public int enemyRocketAmmo;
    public int enemyMagazine;
    public GameObject misslePoolParent;
    public List<GameObject> missilePool = new List<GameObject>();
    public int missileCount;
    public GameObject missileSpawn;
    public bool canFire;
    [Header("Enemy References")]
    public Rover_Enemy RE;

    public AudioSource audioSource;
    // Use this for initialization
    void Start() {
        //misslePoolParent = GameObject.FindGameObjectWithTag("ShoulderBulletParent");
        for (int i = 0; i < enemyRocketAmmo * 12 * enemyMagazine; i++) {
            //GameObject RSMG_Bullet = Instantiate(subMachinegunBullet);
            GameObject ENEMY_MISSILE = Instantiate(enemyRocket, misslePoolParent.transform);
            missilePool.Add(ENEMY_MISSILE);
            //bulletPool[i] = RSMG_Bullet;
            //bulletPool[i].transform.parent = bulletPoolParent.transform;
            missilePool[i].transform.position = Vector3.zero;
            missilePool[i].SetActive(false);
        }

        //FireMissileLauncher(RE.BP);
    }

    void Update() {

    }

    public void FireMissileLauncher(BossPhase bossPhase) {
        switch (bossPhase) {
            case BossPhase.PhaseOne:
                StartCoroutine(FireMissiles(0.25f,3));
                break;

            case BossPhase.PhaseTwo:
                StartCoroutine(FireMissiles(0.45f,7));
                break;
        }
        canFire = false;
        //StartCoroutine(RechargeWait(bossPhase));
    }

    private IEnumerator FireMissiles(float waitTime, int MissileFireCount) {
        for(int i = 0; i< MissileFireCount; i++){
            yield return new WaitForSeconds(waitTime);
            audioSource.PlayOneShot(audioSource.clip);
            missilePool[missileCount].transform.position = missileSpawn.transform.position;
            missilePool[missileCount].transform.rotation = missileSpawn.transform.rotation;
            missilePool[missileCount].SetActive(true);
            missileCount++;
        }
    }

    /*public IEnumerator RechargeWait(BossPhase bossPhase) {
        
        switch (bossPhase) {
            case BossPhase.PhaseOne:
                yield return new WaitForSeconds(8.0f);
                break;

            case BossPhase.PhaseTwo:
                yield return new WaitForSeconds(4.0f);
                break;
        }
        canFire = true;
    }*/
}
