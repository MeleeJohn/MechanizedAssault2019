using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum BossPhase { PhaseOne, PhaseTwo };
public enum MissileSide { LeftSide, RightSide };

public class Rover_Enemy : MonoBehaviour {

    public GameManager GM;

    [Header("Health")]
    //public float Health = 100;
    //public float baseHealth;
    //public Slider HealthBar;

    [Header("Class References")]
    public PlayerController PC;
    public Enemy enemyClass;

    [EnumPaging]
    public LevelControllerType LCT;
    [ShowIf("LCT", LevelControllerType.TestLevel)]
    public TestController TC;
    [ShowIf("LCT", LevelControllerType.LevelOne)]
    public LevelOneController levelOneController;
    [ShowIf("LCT", LevelControllerType.LevelTwo)]
    public LevelTwoController levelTwoController;
    [ShowIf("LCT", LevelControllerType.LevelThree)]
    public LevelThreeController levelThreeController;

    [Header("Targeted and UI")]
    public bool Targeted;
    //public bool Scanned;
    //public GameObject UICanvas;

    [Header("States and Patrol points")]
    public EnemyAIStates state;
    public List<GameObject> patrolPoints = null;
    public GameObject patrollingInterestPoint;
    public GameObject stationaryPoint;
    public GameObject playerOfInterest;
    public bool isStationaryEnemy;
    public float distanceToPlayer;
    //public GameObject patrolPointOne;
    //public GameObject patrolPointTwo;

    [Header("Speeds")]
    public float walkingSpeed = 3.0f;
    public float chasingSpeed = 5.0f;
    public float attackingSpeed = 1.5f;

    [Header("Distances")]
    public float attackingDistance;
    public float attackStoppingDistance;
    public float shootingDistance;
    public float patrolDistance = 20.0f;

    [Header("Attcking Variables")]
    public bool canAttack = false;

    [Header("Current State")]
    public EnemyAILifeStates Cs = EnemyAILifeStates.Paused;
    public bool dying = false;
    [Header("NavMesh Agent")]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;

    [Header("Animations")]
    public Animator Anim;
    public Rigidbody RB;

    [Header("Explosions")]
    public GameObject explosion01;
    public GameObject explosion02;
    public GameObject explosion03;
    // Use this for initialization


    #region Boss Variables
    [Header("Boss Variables")]
    public bool isBoss;
    public BossPhase BP;
    public GameObject energyOrb;
    [Header("Upper Arms")]
    public GameObject leftUpperArm;
    public GameObject rightUpperArm;
    [Header("Lower Arms")]
    public GameObject leftLowerArm;
    public GameObject rightLowerArm;

    [Header("Missile Launchers")]
    public EnemyShoulderRockets leftShoulderRockets;
    public EnemyShoulderRockets rightShoulderRockets;
    public MissileSide MS;
    public bool missileFired;

    [Header("Targeted Enemy and UI")]
    public bool Targeting;
    public GameObject PossibleEnemy;
    public GameObject targetedRightEnemy;
    public GameObject targetedLeftEnemy;
    public GameObject aimingCollider;

    [Header("Boss Flight and Boost Variables")]
    public float thrusterUsage;
    public float thrusterAmountReference;
    public float thrusterAmount;
    public float thrusterRecharge;
    #endregion


    void Start() {
        if (patrolPoints == null && isStationaryEnemy == false) {
            print("FIND POINTS...");
            patrolPoints = new List<GameObject>();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("PatrolPoints")) {
                Debug.Log("Adding Enemy Patrol Point: " + go.transform.position);
                patrolPoints.Add(go);
            }
        }

        //Health = baseHealth;
        RB = GetComponent<Rigidbody>();
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        SwitchToPatrolling();

    }

    // Update is called once per frame
    void Update() {
        if (playerOfInterest == null) {
            playerOfInterest = GameObject.FindGameObjectWithTag("Player");
        } else {
            if (enemyClass.Health <= 0 && isBoss == false && Cs != EnemyAILifeStates.Dead && dying == false) {
                dying = true;
                StartCoroutine(Dead());
            } else if (enemyClass.Health <= 0 && isBoss == true && BP == BossPhase.PhaseOne) {
                enemyClass.Health = 2250;
                BP = BossPhase.PhaseTwo;
                StartCoroutine(PhaseChange());
            } else if (enemyClass.Health <= 0 && isBoss == true && BP == BossPhase.PhaseTwo && dying == false) {
                dying = true;
                StartCoroutine(Dead());
            }

            if (playerOfInterest != null) {
                distanceToPlayer = Vector3.Distance(this.transform.position, playerOfInterest.transform.position);
            }

            if (Cs == EnemyAILifeStates.Alive) {
                switch (state) {
                    case EnemyAIStates.Attacking:
                        OnAttackingUpdate();
                        break;
                    case EnemyAIStates.Chasing:
                        OnChasingUpdate();
                        break;
                    case EnemyAIStates.Patrolling:
                        OnPatrollingUpdate();
                        break;
                    case EnemyAIStates.Stationary:
                        OnStationaryUpdate();
                        break;
                }
            }

            if(canAttack ==  true && enemyClass.isAlive == false) {
                canAttack = false;
            }
            //Debug.Log(navMeshAgent.velocity.normalized);
            Anim.SetFloat("Z-Movement", navMeshAgent.velocity.normalized.z);
            Anim.SetFloat("X-Movement", navMeshAgent.velocity.normalized.x);
        }

        #region Flying Updates
        if (isBoss == true) {
            if (playerOfInterest != null && playerOfInterest.GetComponent<PlayerController>().onGround == false) {
                //this.transform.position = new Vector3 (this.transform.position.x,playerOfInterest.transform.position.y, this.transform.position.z);
            } else {
                RB.AddRelativeForce(0, -0.008f, 0, ForceMode.Impulse);
            }
        }


        #endregion
    }

    void LateUpdate() {
        if (targetedLeftEnemy != null) {
            leftUpperArm.transform.LookAt(targetedLeftEnemy.transform);
        }

        if (targetedRightEnemy != null) {
            rightUpperArm.transform.LookAt(targetedRightEnemy.transform);
        }

    }

    void OnTriggerEnter(Collider other) {

    }

    #region AI Code --------------------------------------------------------------------------------------------

    protected virtual void OnAttackingUpdate() {
        float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
        if (enemyClass.isAlive == true) {
            if (distance > attackStoppingDistance) {
                Debug.Log("Distance < attackStoppingDistance");
                navMeshAgent.SetDestination(playerOfInterest.transform.position);
            } else {
                navMeshAgent.SetDestination(this.transform.position);
                if (isBoss == false) {
                    this.transform.LookAt(playerOfInterest.transform, this.transform.up);
                }
            }

            if (isBoss != true) {
                if (distance < shootingDistance) {
                    canAttack = true;
                } else {
                    canAttack = false;
                }
            } else {
                if (distance < shootingDistance && targetedLeftEnemy != null) {
                    canAttack = true;
                } else {
                    canAttack = false;
                }
            }

            if(enemyClass.isAlive == false && canAttack == true) {
                canAttack = false;
            }
        }


        //Debug.Log ("Distance Attacking: " + distance);
        if (distance > attackingDistance) {
            SwitchToChasing(playerOfInterest);
        }
    }

    protected virtual void OnChasingUpdate() {
        navMeshAgent.SetDestination(playerOfInterest.transform.position);
        if (enemyClass.isAlive == true) {
            float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
            float distanceToPlayer = Vector3.Distance(transform.position, playerOfInterest.transform.position);


            //Debug.Log ("Distance Chasing: " + distance);

            if (distanceToPlayer <= attackingDistance) {
                SwitchToAttacking(playerOfInterest);
            }

            if (missileFired == false && isBoss == true) {
                missileFired = true;
                switch (MS) {
                    case MissileSide.LeftSide:
                        if (distanceToPlayer >= attackingDistance + attackingDistance / 2f && leftShoulderRockets.canFire == true) {
                            leftShoulderRockets.FireMissileLauncher(BP);
                        }
                        MS = MissileSide.RightSide;
                        break;

                    case MissileSide.RightSide:
                        if (distanceToPlayer >= attackingDistance + attackingDistance / 2f && rightShoulderRockets.canFire == true) {
                            rightShoulderRockets.FireMissileLauncher(BP);
                        }
                        MS = MissileSide.LeftSide;
                        break;
                }
                StartCoroutine(MissileFiringCoolDown());
            }

            if (distanceToPlayer > patrolDistance) {
                SwitchToPatrolling();
            }
        }
    }

    protected virtual void OnPatrollingUpdate() {
        navMeshAgent.SetDestination(patrollingInterestPoint.transform.position);

        float distance = Vector3.Distance(transform.position, patrollingInterestPoint.transform.position);
        //Debug.Log("Distance to selected patrol point is: " + distance);
        float distanceToPlayer = Vector3.Distance(transform.position, playerOfInterest.transform.position);

        //Debug.Log("Patrolling Distance: " + distanceToPlayer);
        if (enemyClass.isAlive == true) {
            if (distance <= navMeshAgent.stoppingDistance) {
                ChangePatrolPoint();
            }

            if (distanceToPlayer < patrolDistance && playerOfInterest.GetComponent<PlayerController>().canMove == true) {
                SwitchToChasing(playerOfInterest);
            }
        }
    }

    protected virtual void OnStationaryUpdate() {
        navMeshAgent.SetDestination(stationaryPoint.transform.position);

        //Debug.Log("Distance to selected patrol point is: " + distance);
        float distanceToPlayer = Vector3.Distance(transform.position, playerOfInterest.transform.position);

        //Debug.Log("Patrolling Distance: " + distance);
        if (enemyClass.isAlive == true) {
            if (distanceToPlayer < patrolDistance && playerOfInterest.GetComponent<PlayerController>().canMove == true) {
                SwitchToChasing(playerOfInterest);
            }
        }
    }


    protected void SwitchToPatrolling() {
        if (isStationaryEnemy == false) {
            state = EnemyAIStates.Patrolling;
            Debug.Log("Switch to patrol");
            ChangePatrolPoint();
        } else {
            state = EnemyAIStates.Stationary;
            Debug.Log("Switch to patrol");
        }
    }

    protected void SwitchToAttacking(GameObject target) {
        state = EnemyAIStates.Attacking;
        Debug.Log("Switch to attack");
    }

    protected void SwitchToChasing(GameObject target) {
        state = EnemyAIStates.Chasing;
        Debug.Log("Switch to chasing");
        playerOfInterest = target;
    }

    private IEnumerator MissileFiringCoolDown() {
        switch (BP) {
            case BossPhase.PhaseOne:
                yield return new WaitForSeconds(8.0f);
                break;

            case BossPhase.PhaseTwo:
                yield return new WaitForSeconds(4.0f);
                break;
        }
        missileFired = true;
    }

    protected virtual void ChangePatrolPoint() {
        int choice = Random.Range(0, patrolPoints.Count);
        patrollingInterestPoint = patrolPoints[choice];
        Debug.Log(this.gameObject.ToString() + " is changing patrolpoint");
    }
    #endregion

    public void TargetEnemy() {

        Targeting = true;
        targetedLeftEnemy = PossibleEnemy;
        targetedRightEnemy = PossibleEnemy;
        PossibleEnemy = null;
    }

    public void UnTargeted() {

        targetedLeftEnemy = null;
        targetedRightEnemy = null;
        Targeting = false;
    }


    public IEnumerator BossStartUp() {
        Cs = EnemyAILifeStates.Paused;
        yield return new WaitForSeconds(0.0f);
        leftUpperArm.transform.rotation = Quaternion.Euler(Vector3.zero);
        rightUpperArm.transform.rotation = Quaternion.Euler(Vector3.zero);
        leftLowerArm.transform.rotation = Quaternion.Euler(Vector3.zero);
        rightLowerArm.transform.rotation = Quaternion.Euler(Vector3.zero);
        yield return new WaitForSeconds(1.0f);
        Cs = EnemyAILifeStates.Alive;
    }

    public IEnumerator PhaseChange() {
        yield return new WaitForSeconds(0.0f);
        energyOrb.SetActive(true);
        navMeshAgent.speed += 200;
        navMeshAgent.angularSpeed += 225;
    }

    private IEnumerator Dead() {
        Cs = EnemyAILifeStates.Dead;
        enemyClass.isAlive = false;
        
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (enemyClass.Targeted == true) {
            PC.targetedLeftEnemy = null;
            PC.targetedRightEnemy = null;
            PC.Targeting = false;
        }

        if(isBoss == true && Targeting == true) {
            targetedLeftEnemy = null;
            targetedRightEnemy = null;
            Targeting = false;
        }

        switch (LCT) {
            case LevelControllerType.TestLevel:
                TC.enemyCount--;
                explosion01.SetActive(true);
                explosion02.SetActive(true);
                yield return new WaitForSeconds(1.2f);
                explosion03.SetActive(true);
                yield return new WaitForSeconds(1.2f);
                this.transform.parent.gameObject.SetActive(false);
                break;
            case LevelControllerType.LevelOne:
                levelOneController.enemiesCount--;
                explosion01.SetActive(true);
                explosion02.SetActive(true);
                yield return new WaitForSeconds(1.2f);
                explosion03.SetActive(true);
                yield return new WaitForSeconds(1.2f);
                this.transform.parent.gameObject.SetActive(false);
                break;
            case LevelControllerType.LevelTwo:
                levelTwoController.enemiesCount--;
                explosion01.SetActive(true);
                explosion02.SetActive(true);
                yield return new WaitForSeconds(1.2f);
                explosion03.SetActive(true);
                yield return new WaitForSeconds(1.2f);
                this.transform.parent.gameObject.SetActive(false);
                break;
            case LevelControllerType.LevelThree:
                energyOrb.SetActive(false);
                StartCoroutine(levelThreeController.EndingCinematic());
                break;
        }
    }
}
