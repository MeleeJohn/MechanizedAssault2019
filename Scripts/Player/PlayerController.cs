using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using TMPro;

public enum ShieldStatus { Up, Down };

public class PlayerController : MonoBehaviour {
    public GameManager GM;

    [Header("Object References")]
    public GameObject middleReference;
    public GameObject headReference;

    [Header("Camera")]
    public GameObject Camera;
    public GameObject VirtualCamera;
    public Transform lookPoint;
    public Transform followPoint;
    float cameraRotationLimitX;

    [Header("Rigidbody and Movement")]
    public bool canMove;
    [Required]
    public Rigidbody RB;
    [SerializeField]
    private float jumpInput;
    public float moveX;
    public float moveZ;
    public float moveY;


    [Header("Variables")]
    [SerializeField]
    public float immediateGroundMoveForce = 1f;
    private float groundSpeed = 8;
    private float jumpForce = 1;
    private float jumpForceInitial = 1;
    //public float jumpSpeed = 20;
    //private float boostForce;
    [SerializeField]
    private float magnitudeReference;
    [SerializeField]
    private float groundForce;
    [SerializeField]
    private float boostForce;
    private float flightRayDistance;

    [Header("TESTER VARIABLE SELECTIONS")]
    public Dropdown frameType;

    [Header("Animations")]
    public Animator Anim;
    public float animX;
    public float animZ;

    [Header("Upper Arms")]
    public GameObject RightArm;
    public GameObject LeftArm;

    [Header("Upper Body")]
    public GameObject upperBody;

    [Header("Targeted Enemy and UI")]
    public bool Targeting;
    public GameObject PossibleEnemy;
    public GameObject targetedRightEnemy;
    public GameObject targetedLeftEnemy;
    public GameObject possibleTargetReticle;
    public GameObject targetedReticle;
    public GameObject aimingCollider;
    public GameObject incomingMissileWarning;
    public GameObject radarPanel;
    public GameObject indicatorPanel;
    public SickscoreGames.HUDNavigationSystem.HUDNavigationSystem HUDsystem;

    [Header("Scanner UI")]
    public float scannerValue;
    public Image scannerBarImage;

    [Header("Boolean Conditions")]
    public bool onGround;
    public bool Flying;
    public bool enginesOff;
    public bool isScanning;
    public bool scannerEmpty;
    public bool Boosting;
    public bool speedDoubled;
    public bool regeningThrusters;
    public bool missileTargeted;

    [Header("Shield and Health")]
    public ShieldStatus ShieldStatus = ShieldStatus.Up;
    public GameObject Shield;
    public Image shieldBarImage;
    [SerializeField]
    public float Health;
    private float healthReference;
    [SerializeField]
    public float Shields;
    public float overShields;
    private float shieldsReference;
    public TextMeshProUGUI healthText;

    [Header("Flying and Boosting Variables")]
    //public Slider flightTestUiBar;
    public Image thrusterBarImage;
    public float thrusterUsage;
    public float thrusterAmountReference;
    public float thrusterAmount;
    public float thrusterRecharge;
    //private float fallingWeight;

    [Header("Scanning Items")]
    public GameObject scanOverlay;
    public GameObject scannerText;
    private float scannerTime;
    private float scannerTimeReference;

    [Header("Weapon Spawns")]
    public GameObject leftWeaponSpawn;
    public GameObject rightWeaponSpawn;

    [Header("Stationary Pistols")]
    public bool leftPistolSwapped;
    public GameObject leftStationaryPistol;
    public bool rightPistolSwapped;
    public GameObject rightstationaryPistol;

    [Header("Pistol Prefabs")]
    public GameObject rightPistol;
    public GameObject leftPistol;

    [Header("Materials")]
    public Material talkingMaterial;

    #region Shoulder Weapon Variables
    [TabGroup("Shoulder Weapon Variables")]
    public GameObject shoulderWeaponLowAmmoNotice;
    [TabGroup("Shoulder Weapon Variables")]
    public Text shoulderWeaponNotice;
    [TabGroup("Shoulder Weapon Variables")]
    public GameObject[] shoulderWeapons;
    #endregion

    #region Left Weapon Variables
    [TabGroup("Left Weapons")]
    public GameObject leftWeaponTextOBJ;
    [TabGroup("Left Weapons")]
    public int leftWeaponAmmoCount;
    [TabGroup("Left Weapons")]
    public GameObject[] leftWeaponMagazines;
    [TabGroup("Left Weapons")]
    public GameObject leftShotgun;
    [TabGroup("Left Weapons")]
    public GameObject leftAssaultRifle;
    [TabGroup("Left Weapons")]
    public GameObject leftSubMachinegun;
    [TabGroup("Left Weapons")]
    public GameObject leftSniperRifle;
    [TabGroup("Left Weapons")]
    public GameObject leftMinigun;
    [TabGroup("Left Weapons")]
    public GameObject leftMarksmanRifle;
    [TabGroup("Left Weapons")]
    public bool leftMeleeWeapon = false;
    [TabGroup("Left Weapons")]
    public GameObject leftShortSword;
    [TabGroup("Left Weapons")]
    public GameObject leftWeaponLowAmmoNotice;
    #endregion

    #region Right Weapon Variables
    [TabGroup("Right Weapons")]
    public GameObject rightWeaponTextOBJ;
    [TabGroup("Right Weapons")]
    public int rightWeaponAmmoCount;
    [TabGroup("Right Weapons")]
    public GameObject[] rightWeaponMagazines;
    [TabGroup("Right Weapons")]
    public GameObject rightShotgun;
    [TabGroup("Right Weapons")]
    public GameObject rightAssaultRifle;
    [TabGroup("Right Weapons")]
    public GameObject rightSubMachinegun;
    [TabGroup("Right Weapons")]
    public GameObject rightSniperRifle;
    [TabGroup("Right Weapons")]
    public GameObject rightMinigun;
    [TabGroup("Right Weapons")]
    public GameObject rightMarksmanRifle;
    [TabGroup("Right Weapons")]
    public bool rightMeleeWeapon = false;
    [TabGroup("Right Weapons")]
    public GameObject rightShortSword;
    [TabGroup("Right Weapons")]
    public GameObject rightWeaponLowAmmoNotice;

    [TabGroup("Shoulder Weapons")]
    public GameObject shoulderWeaponTextOBJ;
    #endregion

    [Header("Different UI Pieces")]
    public GameObject frameStartupText;
    public String frameStartUpString;
    public Canvas playerCanvas;
    public GameObject combatHUD;
    public GameObject scannerHUD;
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyHitPoints;
    public TextMeshProUGUI enemyType;
    public GameObject AmmoHolder;
    public GameObject HealthHolder;
    public GameObject MiddleReticleHolder;
    public GameObject shieldEmitterObjects;
    public GameObject[] ActiveCanvasItems;
    public GameObject missionStartText;
    public GameObject unknownEnemyHelthBar;
    [Header("Particle Jets")]
    public GameObject chestFlightJets;
    public GameObject leftLegFrontJets;
    public GameObject leftLegBackJets;
    public GameObject rightLegFrontJets;
    public GameObject rightLegBackJets;
    public GameObject leftGroupParticles;
    public GameObject rightGroupParticles;

    [Header("Damage items")]
    public GameObject[] damageItemsArray;
    public GameObject[] deathExplosions;
    public GameObject systemFailureWarning;
    public GameObject controlLossWarning;
    public bool Dying;

    [Header("Text Running")]
    public String deathWarningString;
    private String currentText;
    public TextMeshProUGUI frameDeathText;
    public float textTypeDelay;

    [Header("Audio Items")]
    public AudioSource WarningAudioSource;
    public AudioClip MissileWarning;
    public AudioSource frameAudioSource;
    public AudioClip frameStartUpSound;
    public AudioClip frameRunningSound;
    public AudioSource flightSoundsAudioSource;
    public AudioClip frameFlightSound;
    public AudioSource boostSoundAudioSource;
    public AudioSource frameBulletHitSource;
    public AudioClip frameBulletHit;

    void Awake() {
        GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        GM.PC = this.GetComponent<PlayerController>();
        GM.playerCanvasOBJ = playerCanvas.gameObject;
        GM.playerCanvas = playerCanvas;
    }

    void Start() {
        shoulderWeaponNotice = shoulderWeaponLowAmmoNotice.transform.transform.GetChild(0).GetComponent<Text>();
        Camera = GameObject.FindGameObjectWithTag("Player Camera");
        VirtualCamera = GameObject.FindGameObjectWithTag("PlayerVirtualCamera");
        aimingCollider = GameObject.FindGameObjectWithTag("Aiming Collider");
        //playerCanvas = GameObject.FindGameObjectWithTag("Player Canvas").GetComponent<Canvas>();

        #region Frame stats selection
        switch (PlayerPrefs.GetInt("FrameChoice")) {
            case 1:
                Health = 1750;
                healthReference = Health;
                magnitudeReference = 65;
                groundForce = 67;
                boostForce = 79f;
                Shields = 125;
                shieldsReference = Shields;
                //fallingWeight = 4f;
                thrusterUsage = 15f;
                thrusterAmount = 115f;
                thrusterAmountReference = thrusterAmount;
                jumpForceInitial = 1;
                //jumpSpeed = 4;
                flightRayDistance = 20f;
                scannerTime = 250f;
                scannerTimeReference = scannerTime;
                break;

            case 2:
                Health = 2000;
                healthReference = Health;
                magnitudeReference = 68;
                groundForce = 63;
                boostForce = 65f;
                Shields = 150;
                shieldsReference = Shields;
                //fallingWeight = 5f;
                thrusterUsage = 17f;
                thrusterAmount = 110f;
                thrusterAmountReference = thrusterAmount;
                jumpForceInitial = 1;
                //jumpSpeed = 5;
                flightRayDistance = 20f;
                scannerTime = 250f;
                scannerTimeReference = scannerTime;
                break;

            case 3:
                Health = 2500;
                healthReference = Health;
                magnitudeReference = 70;
                groundForce = 57;
                boostForce = 59f;
                Shields = 200;
                shieldsReference = Shields;
                //fallingWeight = 6f;
                thrusterUsage = 20f;
                thrusterAmount = 105f;
                thrusterAmountReference = thrusterAmount;
                jumpForceInitial = 1;
                //jumpSpeed = 5;
                flightRayDistance = 23f;
                scannerTime = 250f;
                scannerTimeReference = scannerTime;
                break;

        }

        if (PlayerPrefs.GetInt("FrameExtraArmorVariable") == 1) {
            Health += 475;
            groundForce -= 6;
            jumpForceInitial = 0.75f;
        }

        if (PlayerPrefs.GetInt("FrameExtraFuelVariable") == 1) {
            groundForce -= 2;
            jumpForceInitial = 0.90f;
            thrusterAmount += 30;
            thrusterAmountReference = thrusterAmount;
        }

        if (PlayerPrefs.GetInt("FrameIncreasedShieldCapactiy") == 1) {
            groundForce -= 1.5f;
            Shields += 200;
            shieldsReference = Shields;
        }

        if (PlayerPrefs.GetInt("FrameIncreasedScannerTime") == 1) {
            scannerTime += 150;
            scannerTimeReference = scannerTime;
        }

        #endregion

        #region Weapon Select
        switch (PlayerPrefs.GetInt("RightWeaponChoice")) {

            case 0:
                rightSubMachinegun.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                break;

            case 1:
                rightAssaultRifle.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                break;

            case 2:
                rightShotgun.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                break;

            case 3:
                rightMarksmanRifle.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                break;

            case 4:
                rightSniperRifle.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                break;

            case 5:
                rightMinigun.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                break;

            case 6:
                rightShortSword.SetActive(true);
                rightWeaponMagazines[PlayerPrefs.GetInt("RightWeaponChoice")].SetActive(true);
                rightMeleeWeapon = true;
                break;
        }

        switch (PlayerPrefs.GetInt("LeftWeaponChoice")) {
            case 0:
                leftSubMachinegun.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                break;

            case 1:
                leftAssaultRifle.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                break;

            case 2:
                leftShotgun.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                break;

            case 3:
                leftMarksmanRifle.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                break;

            case 4:
                leftSniperRifle.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                break;

            case 5:
                leftMinigun.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                break;

            case 6:
                leftShortSword.SetActive(true);
                leftWeaponMagazines[PlayerPrefs.GetInt("LeftWeaponChoice")].SetActive(true);
                leftMeleeWeapon = true;
                break;
        }
        Debug.Log(PlayerPrefs.GetInt("ShoulderWeaponChoice"));
        switch (PlayerPrefs.GetInt("ShoulderWeaponChoice")) {
            case 1:
                shoulderWeapons[0].SetActive(true);
                break;

            case 2:
                shoulderWeapons[1].SetActive(true);
                break;

            case 3:
                shoulderWeapons[2].SetActive(true);
                break;
        }
        #endregion
        switch (PlayerPrefs.GetInt("AutoAimChoice")) {
            case 0:
                //Debug.Log("Auto Aim False");
                aimingCollider.GetComponent<AimingCollider>().autoAim = false;
                break;

            case 1:
                //Debug.Log("Auto Aim True");
                aimingCollider.GetComponent<AimingCollider>().autoAim = true;
                break;
        }
        Camera = GameObject.FindGameObjectWithTag("Player Camera");
        //AmmoHolder.SetActive(false);
        MiddleReticleHolder.SetActive(false);
        HealthHolder.SetActive(false);
        //StartCoroutine(FrameStartUp());
    }

    void FixedUpdate() {
        #region Keyboard Movement
        if (canMove == true) {
            if (GM.prevState.IsConnected == false) {
                if (Boosting == false) {
                    moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
                    moveZ = Input.GetAxis("Vertical") * Time.deltaTime;
                    if (Input.GetButton("Jump") == false && Flying == false) {
                        jumpForce = -0.008f;
                        moveY = jumpForce;
                    } else {
                        if (Input.GetButton("Jump") == true) {
                            jumpForce = jumpForceInitial;
                            moveY = jumpForce * Time.deltaTime;
                        } else if (Input.GetButton("Jump") == false) {
                            jumpForce = 0;
                            moveY = jumpForce;
                        }
                    }
                    if (Input.GetKey(KeyCode.Z)) {
                        //this will shut off the engines and drop the player using the out of fuel drop modifier
                        enginesOff = true;
                        jumpForce = -0.008f;
                        moveY = jumpForce;
                    }

                    animX = Input.GetAxis("Horizontal");
                    animZ = Input.GetAxis("Vertical");
                    Anim.SetFloat("X_Axis", animX);
                    Anim.SetFloat("Z_Axis", animZ);

                    if (Flying == false) {

                        Movement(moveZ * immediateGroundMoveForce, moveY, moveX * immediateGroundMoveForce);
                        if (thrusterAmount < thrusterAmountReference) {
                            regeningThrusters = true;
                        } else {
                            regeningThrusters = false;
                        }
                    } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") != false) {
                        Movement(moveZ * 1.65f, moveY, moveX * 1.65f);
                        thrusterAmount = thrusterAmount - 0.30f;
                        regeningThrusters = false;
                    } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") == false) {
                        if (RB.velocity.y > 10f && RB.velocity.y < 30f) {
                            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 2.25f, RB.velocity.z);
                        } else if (RB.velocity.y > 29f) {
                            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 4f, RB.velocity.z);
                        }
                        Movement(moveZ * 2, moveY, moveX * 2);
                        thrusterAmount = thrusterAmount - 0.27f;
                        if (thrusterAmount < thrusterAmountReference) {
                            regeningThrusters = true;
                        } else {
                            regeningThrusters = false;
                        }
                    } else if (Flying == true && thrusterAmount <= 0 || Input.GetButton("Jump") == false || Input.GetButton("Jump") == true) {
                        jumpForce = -0.027f;
                        moveY = jumpForce;
                        if (thrusterAmount < thrusterAmountReference) {
                            regeningThrusters = true;
                        } else {
                            regeningThrusters = false;
                        }
                        Movement(moveZ, moveY, moveX);
                    }

                    if (Input.GetButtonDown("Boost") && thrusterAmount > thrusterUsage) {
                        thrusterAmount -= thrusterUsage;
                        Boosting = true;
                        StartCoroutine(Dash(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"), 0.050f));
                    }
                }

                this.transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X") * 5, 0f));
                cameraRotationLimitX -= Input.GetAxis("Mouse Y") * 2.5f;
                cameraRotationLimitX = Mathf.Clamp(cameraRotationLimitX, -35, 30);
                Camera.transform.localEulerAngles = new Vector3(cameraRotationLimitX, 0.0f, 0.0f);

            }
        }
        #endregion

        #region Controller Movement
        if (canMove == true) {
            if (GM.prevState.IsConnected) {

                moveX = GM.state.ThumbSticks.Left.X * Time.deltaTime;
                moveZ = GM.state.ThumbSticks.Left.Y * Time.deltaTime;

                if (Input.GetButton("Jump") == false && Flying == false) {
                    jumpForce = -0.008f;
                    moveY = jumpForce;
                } else {
                    if (Input.GetButton("Jump") == true) {
                        jumpForce = jumpForceInitial;
                        moveY = jumpForce * Time.deltaTime;
                    } else if (Input.GetButton("Jump") == false) {
                        enginesOff = true;
                        jumpForce = 0;
                        moveY = 0;
                    }
                }
                if (GM.prevState.DPad.Down == XInputDotNetPure.ButtonState.Pressed && GM.state.DPad.Down != XInputDotNetPure.ButtonState.Released && Flying == true) {
                    //this will shut off the engines and drop the player using the out of fuel drop modifier
                    enginesOff = true;
                    jumpForce = -0.008f;
                    moveY = jumpForce;
                }

                animX = GM.state.ThumbSticks.Left.X;
                animZ = GM.state.ThumbSticks.Left.Y;
                Anim.SetFloat("X_Axis", animX);
                Anim.SetFloat("Z_Axis", animZ);

                if (Flying == false) {

                    Movement(moveZ * immediateGroundMoveForce, moveY, moveX * immediateGroundMoveForce);
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") == true) {
                    Movement(moveZ * 1.65f, moveY, moveX * 1.65f);
                    thrusterAmount = thrusterAmount - 0.50f;
                    regeningThrusters = false;
                } else if (thrusterAmount > 0 && Flying == true && Input.GetButton("Jump") == false) {
                    if (RB.velocity.y > 10f && RB.velocity.y < 30f) {
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 2.25f, RB.velocity.z);
                    } else if (RB.velocity.y > 29f) {
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y - 4f, RB.velocity.z);
                    }
                    Movement(moveZ * 2f, moveY, moveX * 2f);
                    thrusterAmount = thrusterAmount - 0.27f;
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                } else if (Flying == true && thrusterAmount <= 0 || Input.GetButton("Jump") == false || Input.GetButton("Jump") == true) {
                    jumpForce = -0.027f;
                    moveY = jumpForce;
                    if (thrusterAmount < thrusterAmountReference) {
                        regeningThrusters = true;
                    } else {
                        regeningThrusters = false;
                    }
                    Movement(moveZ, moveY, moveX);
                }

                if (Input.GetButtonDown("Boost") && thrusterAmount > thrusterUsage) {
                    thrusterAmount -= thrusterUsage;
                    Boosting = true;
                    StartCoroutine(Dash(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"), 0.050f));
                }

                this.transform.Rotate(new Vector3(0f, GM.state.ThumbSticks.Right.X * 2, 0f));

                cameraRotationLimitX -= GM.state.ThumbSticks.Right.Y * 2.5f;
                cameraRotationLimitX = Mathf.Clamp(cameraRotationLimitX, -35, 30);

                Camera.transform.localEulerAngles = new Vector3(cameraRotationLimitX, 0.0f, 0.0f);

            }
        }
        #endregion

        #region Flying Check and recharge
        RaycastHit hit;
        //Debug.DrawRay(transform.position, -Vector3.up, Color.red, 1, false);
        if (Physics.Raycast(transform.position, -transform.up, out hit)) {
            if (hit.distance < 20f && hit.collider.tag == "Ground") {
                //Debug.Log("Flying == false");
                Flying = false;
            } else {
                //Debug.Log("Flying == true");
                Flying = true;
            }
        }

        if (Flying == true) {
            chestFlightJets.SetActive(true);
            leftGroupParticles.SetActive(false);
            rightGroupParticles.SetActive(false);
            if (flightSoundsAudioSource.isPlaying == false) {
                flightSoundsAudioSource.clip = frameFlightSound;
                flightSoundsAudioSource.volume = 0.1f;
                flightSoundsAudioSource.Play();
                flightSoundsAudioSource.loop = true;
            }
        } else if (Flying == false) {
            chestFlightJets.SetActive(false);
            leftGroupParticles.SetActive(true);
            rightGroupParticles.SetActive(true);
            if (flightSoundsAudioSource.isPlaying == true) {
                flightSoundsAudioSource.clip = null;
                flightSoundsAudioSource.Stop();
                flightSoundsAudioSource.loop = false;
            }
        }

        if (regeningThrusters == true) {
            thrusterAmount += 0.25f;
        }
        #endregion
    }

    void Update() {
        DamageEffects();
        ScannerBar();
        ThrusterBar();
        ShieldBar();

        if (canMove == false) {
            controlLossWarning.SetActive(true);
        } else {
            controlLossWarning.SetActive(false);
        }

        if (PossibleEnemy == true) {
            possibleTargetReticle.SetActive(true);
        } else {
            possibleTargetReticle.SetActive(false);
        }

        if (thrusterAmount >= thrusterAmountReference / 1.5f) {
            thrusterBarImage.color = new Color(0f, 1f, 0f, 0.45f);
        } else if (thrusterAmount <= thrusterAmountReference / 1.5f && thrusterAmount >= thrusterAmountReference / 2.5f) {
            thrusterBarImage.color = new Color(1f, 1f, 0f, 0.45f);
        } else if (thrusterAmount <= thrusterAmountReference / 2.5f) {
            thrusterBarImage.color = new Color(1f, 0f, 0f, 0.45f);
        }

        if (Health <= healthReference / 6 && Health > 0) {
            systemFailureWarning.SetActive(true);
        }

        if (Health <= 0 && Dying == false) {
            Dying = true;
            healthText.text = "" + 0;
            Debug.Log("Death Method about to run");
            StartCoroutine(DeathMethod());

        } else {
            healthText.text = Health.ToString();
        }

        if (missileTargeted == true) {
            incomingMissileWarning.SetActive(true);
            if (WarningAudioSource.isPlaying == false) {
                WarningAudioSource.clip = MissileWarning;
                WarningAudioSource.volume = 0.1f;
                WarningAudioSource.Play();
            }
        } else {
            if (WarningAudioSource.isPlaying == true) {
                WarningAudioSource.Stop();
            }
            incomingMissileWarning.SetActive(false);
        }

        #region Keyboard Actions
        if (GM.prevState.IsConnected == false) {
            if (Input.GetKeyDown(KeyCode.L) && rightPistolSwapped == false && canMove == true) {
                StartCoroutine(RightPistolSwap());
            }

            if (Input.GetKeyDown(KeyCode.K) && leftPistolSwapped == false && canMove == true) {
                StartCoroutine(LeftPistolSwap());
            }
            if (PlayerPrefs.GetInt("AutoAimChoice") == 0) {
                if (Input.GetMouseButtonDown(2) && Targeting == true) {
                    UnTargeted();
                }

                if (Input.GetMouseButtonDown(2) && PossibleEnemy != null && Targeting == false && PossibleEnemy.GetComponent<Enemy>().isAlive == true) {
                    TargetEnemy();
                }
            } else if (PlayerPrefs.GetInt("AutoAimChoice") == 1) {
                if (PossibleEnemy != null && Targeting == false && PossibleEnemy.GetComponent<Enemy>().isAlive == true) {
                    TargetEnemy();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && isScanning == true) {
                StartCoroutine(ScannerDown());
                StartCoroutine(ScannerRecharge());
                //StartCoroutine(ScannerTimer());
                isScanning = false;
            }

            if (Input.GetKeyUp(KeyCode.Alpha1) && isScanning == false && scannerEmpty == false && canMove == true) {
                StartCoroutine(ScannerTimer());
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                Time.timeScale = 1.0f;
            }
        }
        #endregion

        #region Controller Actions
        if (GM.prevState.IsConnected) {
            if (GM.prevState.DPad.Up == XInputDotNetPure.ButtonState.Released && GM.state.DPad.Up == XInputDotNetPure.ButtonState.Pressed && isScanning == true) {
                StartCoroutine(ScannerDown());
                StartCoroutine(ScannerRecharge());
            }

            if (GM.prevState.DPad.Up == XInputDotNetPure.ButtonState.Released && GM.state.DPad.Up == XInputDotNetPure.ButtonState.Pressed && isScanning == false && scannerEmpty == false && canMove == true) {
                StartCoroutine(ScannerTimer());

            }

            if (PlayerPrefs.GetInt("AutoAimChoice") == 0) {
                if (GM.prevState.Buttons.RightStick == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.RightStick == XInputDotNetPure.ButtonState.Pressed && Targeting == true) {
                    UnTargeted();
                }

                if (GM.prevState.Buttons.RightStick == XInputDotNetPure.ButtonState.Released && GM.state.Buttons.RightStick == XInputDotNetPure.ButtonState.Pressed && PossibleEnemy != null && Targeting == false) {
                    TargetEnemy();
                }
            } else if (PlayerPrefs.GetInt("AutoAimChoice") == 1) {
                if (PossibleEnemy != null && Targeting == false) {
                    TargetEnemy();
                }
            }

            if (GM.prevState.DPad.Down == XInputDotNetPure.ButtonState.Released && GM.state.DPad.Down == XInputDotNetPure.ButtonState.Pressed) {
                Flying = false;
                moveY = -0.04f;
            }
        }

        /*if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Time.timeScale = 1.0f;
        }*/
        #endregion

        #region Flying Raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
            if (hit.distance < 11.0f && hit.collider.tag == "Ground") {
                onGround = true;
                Flying = false;
                Anim.SetBool("Flying", Flying);
                Anim.SetBool("OnGround", onGround);
            } else {
                onGround = false;
                Flying = true;
                Anim.SetBool("Flying", Flying);
                Anim.SetBool("OnGround", onGround);
            }
        }
        #endregion

        #region Targeted Health Bar
        if (targetedLeftEnemy != null || targetedRightEnemy != null) {
            targetedReticle.SetActive(true);
            if (targetedLeftEnemy.GetComponent<Rover_Enemy>() != null) {
                if (targetedLeftEnemy != null && targetedLeftEnemy.GetComponent<Enemy>().gameObject.GetComponent<Rover_Enemy>().isBoss == false) {
                    EnemyHealthBar(targetedLeftEnemy.GetComponent<Enemy>().Health, targetedLeftEnemy.GetComponent<Enemy>().baseHealth);
                } else if (targetedRightEnemy != null && targetedRightEnemy.GetComponent<Enemy>().gameObject.GetComponent<Rover_Enemy>().isBoss == false) {
                    EnemyHealthBar(targetedRightEnemy.GetComponent<Enemy>().Health, targetedRightEnemy.GetComponent<Enemy>().baseHealth);
                } else {
                    unknownEnemyHelthBar.SetActive(true);
                }
            } else if(targetedLeftEnemy.GetComponent<Rover_Enemy>() == null) {
                if (targetedLeftEnemy != null) {
                    EnemyHealthBar(targetedLeftEnemy.GetComponent<Enemy>().Health, targetedLeftEnemy.GetComponent<Enemy>().baseHealth);
                } else if (targetedRightEnemy != null) {
                    EnemyHealthBar(targetedRightEnemy.GetComponent<Enemy>().Health, targetedRightEnemy.GetComponent<Enemy>().baseHealth);
                }
            }
        } else {
            targetedReticle.SetActive(false);
        }
        #endregion
    }

    void LateUpdate() {
        upperBody.transform.localEulerAngles = new Vector3(cameraRotationLimitX, upperBody.transform.rotation.y, upperBody.transform.rotation.z);

        if (targetedLeftEnemy != null) {
            LeftArm.transform.LookAt(targetedLeftEnemy.transform);
        }

        if (targetedRightEnemy != null) {
            RightArm.transform.LookAt(targetedRightEnemy.transform);
        }

    }

    #region Move Methods
    void Movement(float moveZ, float moveY, float moveX) {
        Vector3 movement = new Vector3(moveX, moveY, moveZ);
        RB.AddRelativeForce(movement * groundForce * groundSpeed, ForceMode.Impulse);
        if (RB.velocity.magnitude > magnitudeReference && Boosting == false) {
            RB.velocity = groundForce * (RB.velocity.normalized);
        }
    }

    private IEnumerator ThrusterRegen(float waitTime) {
        while (thrusterAmount < thrusterAmountReference) {
            thrusterAmount += 0.35f;
            yield return new WaitForSeconds(waitTime);
        }
    }
    #endregion

    #region Boost Method
    public IEnumerator Dash(float moveX, float moveY, float moveZ, float dashAddition) {
        Boosting = true;
        /*if (GM.prevState.IsConnected == false) {
            if (Input.GetKey(KeyCode.W) == true) {
                leftLegBackJets.SetActive(true);
                rightLegBackJets.SetActive(true);
            } else if (Input.GetKey(KeyCode.S) == true) {
                leftLegFrontJets.SetActive(true);
                rightLegFrontJets.SetActive(true);
            }
        } else {
            
        }*/
        boostSoundAudioSource.PlayOneShot(boostSoundAudioSource.clip);
        if (moveZ > 0.0000001) {
            leftLegBackJets.SetActive(true);
            rightLegBackJets.SetActive(true);
        } else if (moveZ < -0.000000001) {
            leftLegFrontJets.SetActive(true);
            rightLegFrontJets.SetActive(true);
        }
        Vector3 boostDir = new Vector3(moveX, 0.0f, moveZ);
        float boostReferenceNumber = 0;
        while (boostReferenceNumber < 0.1) {
            RB.AddRelativeForce(boostDir * 70f, ForceMode.Impulse);
            boostReferenceNumber += dashAddition;
        }
        yield return new WaitForSeconds(0.35f);
        leftLegBackJets.SetActive(false);
        rightLegBackJets.SetActive(false);
        leftLegFrontJets.SetActive(false);
        rightLegFrontJets.SetActive(false);
        Boosting = false;
    }
    #endregion

    #region Right Reload
    public IEnumerator Right_Reload() {
        GameObject rightEnemyHold = null;
        if (targetedRightEnemy != null) {
            rightEnemyHold = targetedRightEnemy;
            targetedRightEnemy = null;
        }
        Anim.SetBool("RightArm_Reload", true);
        yield return new WaitForSeconds(0.98f);
        Anim.SetBool("RightArm_Reload", false);
        if (rightEnemyHold != null) {
            targetedRightEnemy = rightEnemyHold;
            rightEnemyHold = null;
        }
    }
    #endregion

    #region Left Reload
    public IEnumerator Left_Reload() {
        GameObject enemyHold = null;
        if (targetedLeftEnemy != null) {
            enemyHold = targetedLeftEnemy;
            targetedLeftEnemy = null;
        }
        Anim.SetBool("LeftArm_Reload", true);
        yield return new WaitForSeconds(0.98f);
        Anim.SetBool("LeftArm_Reload", false);
        if (enemyHold != null) {
            targetedLeftEnemy = enemyHold;
            enemyHold = null;
        }
    }
    #endregion

    #region Enemy Targeting
    public void TargetEnemy() {
        PossibleEnemy.GetComponent<Enemy>().Targeted = true;
        if (leftMeleeWeapon == false) {
            targetedLeftEnemy = PossibleEnemy;
        }

        if (rightMeleeWeapon == false) {
            targetedRightEnemy = PossibleEnemy;
        }
        Targeting = true;
        PossibleEnemy = null;
    }

    public void UnTargeted() {
        if (targetedLeftEnemy == null || targetedRightEnemy == null) {

        } else {
            targetedLeftEnemy.GetComponent<Enemy>().Targeted = false;
        }
        targetedLeftEnemy = null;
        targetedRightEnemy = null;
        Targeting = false;
    }
    #endregion

    private void DamageEffects() {
        if (Health < healthReference - 291.6 && damageItemsArray[0].activeSelf == false) {
            damageItemsArray[0].SetActive(true);
        } else if (Health < healthReference - 291.6 * 2 && damageItemsArray[1].activeSelf == false) {
            damageItemsArray[1].SetActive(true);
        } else if (Health < healthReference - 291.6 * 3) {
            damageItemsArray[2].SetActive(true);
        } else if (Health < healthReference - 291.6 * 4) {
            damageItemsArray[3].SetActive(true);
        } else if (Health < healthReference - 291.6 * 5) {
            damageItemsArray[4].SetActive(true);
        } else if (Health < healthReference - 291.6 * 6) {
            damageItemsArray[5].SetActive(true);
        }

    }

    public IEnumerator FrameStartUp() {
        canMove = false;
        frameStartupText.SetActive(true);
        StartCoroutine(TypeText(frameStartUpString, frameStartupText.GetComponent<TextMeshProUGUI>()));
        frameAudioSource.volume = 0.25f;
        frameAudioSource.PlayOneShot(frameStartUpSound);
        yield return new WaitForSeconds(1.5f);
        AmmoHolder.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        HealthHolder.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        MiddleReticleHolder.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        frameAudioSource.clip = frameRunningSound;
        frameAudioSource.Play();
        frameAudioSource.volume = 0.01f;
        frameAudioSource.loop = true;
        frameStartupText.SetActive(false);
        missionStartText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        missionStartText.SetActive(false);
        canMove = true;
        GM.canPause = true;
    }

    #region Pistol Swap
    public IEnumerator RightPistolSwap() {
        GameObject enemyHold = null;
        if (targetedRightEnemy != null) {
            Debug.Log("Removing Right Enemy");
            enemyHold = targetedRightEnemy;
            targetedRightEnemy = null;
        }
        Anim.SetBool("RightPistolSwap", true);
        yield return new WaitForSeconds(2.4f);
        rightstationaryPistol.SetActive(false);
        rightPistol.SetActive(true);
        Anim.SetBool("RightPistolSwap", false);
        yield return new WaitForSeconds(0.65f);
        if (enemyHold != null) {
            targetedRightEnemy = enemyHold;
            enemyHold = null;
        }
        rightPistolSwapped = true;
    }

    public IEnumerator LeftPistolSwap() {
        GameObject enemyHold = null;
        if (targetedLeftEnemy != null) {
            Debug.Log("Removing Left Enemy");
            enemyHold = targetedLeftEnemy;
            targetedLeftEnemy = null;
        }
        Anim.SetBool("LeftPistolSwap", true);
        yield return new WaitForSeconds(2.4f);
        leftStationaryPistol.SetActive(false);
        leftPistol.SetActive(true);
        Anim.SetBool("LeftPistolSwap", false);
        yield return new WaitForSeconds(0.65f);
        if (enemyHold != null) {
            targetedLeftEnemy = enemyHold;
            enemyHold = null;
        }
        leftPistolSwapped = true;
    }
    #endregion

    public void Hit(float Damage) {
        if (overShields > 0) {
            if (overShields - Damage < 0) {
                frameBulletHitSource.PlayOneShot(frameBulletHit);
                float carryOverDamage = Damage - overShields;
                overShields = 0;
                overShields -= Damage;
                Shields -= carryOverDamage;
            } else {
                frameBulletHitSource.PlayOneShot(frameBulletHit);
                overShields -= Damage;
            }
        } else if (overShields <= 0) {
            if (Shields > 0) {
                if (Shields - Damage < 0) {
                    frameBulletHitSource.PlayOneShot(frameBulletHit);
                    float carryOverDamage = Damage - Shields;
                    Shields -= Damage;
                    Health -= carryOverDamage;
                } else {
                    frameBulletHitSource.PlayOneShot(frameBulletHit);
                    Shields -= Damage;
                }

            } else if (Shields <= 0) {
                frameBulletHitSource.PlayOneShot(frameBulletHit);
                Health -= Damage;
            }
        }
    }

    #region Scanner Code
    public IEnumerator ScannerTimer() {
        if (isScanning == false) {
            //Debug.Log("isScanning == false");
            isScanning = true;
            scanOverlay.SetActive(true);
            scannerHUD.SetActive(true);
            combatHUD.SetActive(false);
            scannerText.SetActive(true);
            talkingMaterial.color = new Color(0f, 255f, 255f);
        } else if (isScanning == true) {
            //Debug.Log("isScanning == true");
            isScanning = false;
        }

        while (isScanning == true && scannerValue > 0) {
            //Debug.Log("Minus the value in while");
            scannerValue -= 1f;
            yield return new WaitForSeconds(0.02f);
        }

        if (scannerValue <= 0f) {
            StartCoroutine(ScannerDown());
            scannerEmpty = true;
        } else {
            isScanning = false;
        }
        StartCoroutine(ScannerRecharge());
    }

    public IEnumerator ScannerRecharge() {
        if (scannerEmpty == true) {
            isScanning = false;
        }
        while (scannerValue < 250) {
            scannerValue += 3.5f;
            yield return new WaitForSeconds(0.05f);
        }
        scannerEmpty = false;
    }

    private IEnumerator ScannerDown() {
        scanOverlay.GetComponent<Animator>().SetBool("Deactivated", true);
        yield return new WaitForSeconds(1.00f);
        isScanning = false;
        scanOverlay.SetActive(false);
        scannerText.SetActive(false);
        scannerHUD.SetActive(false);
        combatHUD.SetActive(true);
        talkingMaterial.color = new Color(0f, 0f, 255f);
    }
    #endregion

    #region UI Elements

    public void ShieldBar() {
        shieldBarImage.fillAmount = ShieldBarMap(Shields, 0, shieldsReference, 0, 1);
    }

    private float ShieldBarMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void ThrusterBar() {
        thrusterBarImage.fillAmount = ThrusterBarMap(thrusterAmount, 0, thrusterAmountReference, 0, 1);
    }

    private float ThrusterBarMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void ScannerBar() {
        if (scannerValue < scannerTimeReference / 1.25f && scannerValue > scannerTimeReference / 2) {
            scannerBarImage.color = new Color(1, 1, 0, 0.45f);
        } else if (scannerValue < scannerTimeReference / 2) {
            scannerBarImage.color = new Color(1, 0, 0, 0.45f);
        } else {
            scannerBarImage.color = new Color(0, 1, 0, 0.45f);
        }
        scannerBarImage.fillAmount = ScannerBarMap(scannerValue, 0, scannerTimeReference, 0, 1);
    }

    private float ScannerBarMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void EnemyHealthBar(float enemyHealth, float enemyHealthBase) {
        enemyHealthBar.fillAmount = EnemyHealthBarMap(enemyHealth, 0, enemyHealthBase, 0, 1);
    }

    private float EnemyHealthBarMap(float Value, float inMin, float inMax, float outMin, float outMax) {
        return (Value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    #endregion

    public void ActivateObject(GameObject selectedObject, int activationState) {
        //0 is inactive, 1 is active
        if (activationState == 0) {
            selectedObject.SetActive(false);
            return;
        } else if (activationState == 1) {
            selectedObject.SetActive(true);
        }
    }

    public IEnumerator TypeText(String stringInput, TextMeshProUGUI textMeshText) {
        textMeshText.gameObject.SetActive(true);
        for (int i = 0; i < stringInput.Length; i++) {
            currentText = stringInput.Substring(0, i);
            textMeshText.text = currentText;
            yield return new WaitForSeconds(textTypeDelay);
        }
    }

    public IEnumerator DeathMethod() {
        //yield return new WaitForSeconds(10f);
        Debug.Log("Death Method running");
        canMove = false;
        RB.velocity = Vector3.zero;
        Anim.SetBool("Dead", true);
        GameObject levelController = GameObject.FindGameObjectWithTag("Level Controller");

        if (levelController.GetComponent<TestController>() != null) {
            PlayerPrefs.SetInt("FromMission", 1);
            StartCoroutine(levelController.GetComponent<TestController>().MissionFailed());
        }

        if (levelController.GetComponent<LevelOneController>() != null) {
            PlayerPrefs.SetInt("FromMission", 1);
            StartCoroutine(levelController.GetComponent<LevelOneController>().MissionFailed());
        }

        if (levelController.GetComponent<LevelTwoController>() != null) {
            PlayerPrefs.SetInt("FromMission", 1);
            StartCoroutine(levelController.GetComponent<LevelTwoController>().MissionFailed());
        }

        if (levelController.GetComponent<LevelThreeController>() != null) {
            PlayerPrefs.SetInt("FromMission", 1);
            StartCoroutine(levelController.GetComponent<LevelThreeController>().MissionFailed());
        }

        if (Anim.GetBool("leftMeleeWeapon") == false) {
            Anim.SetBool("Arms down", true);
        }

        if (Anim.GetBool("rightMeleeWeapon") == false) {
            Anim.SetBool("Arms down", true);
        }
        for (int i = 0; i < ActiveCanvasItems.Length; i++) {
            ActiveCanvasItems[i].SetActive(false);
        }
        for (int i = 0; i < deathExplosions.Length; i++) {
            deathExplosions[i].SetActive(true);
            yield return new WaitForSeconds(0.35f);
        }
        yield return new WaitForSeconds(0.5f);
        systemFailureWarning.SetActive(false);
        StartCoroutine(TypeText(deathWarningString, frameDeathText));

    }
}