using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechRotater : MonoBehaviour {
    public GameManager GM;
    public MainMenuController MMC;
    public float rotation;

    void Start() {
        if (GM == null) {
            GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        }
        rotation = 32f;
    }


    void Update() {
        if (GM != null) {

            if (!GM.prevState.IsConnected) {
                if (Input.GetKey("d") && MMC.canRotate == true) {
                    On_D_down();
                }

                if (Input.GetKey("a") && MMC.canRotate == true) {
                    On_A_down();
                }
            } else if (GM.prevState.IsConnected) {

                if (GM.prevState.ThumbSticks.Left.X < -0.2 && MMC.canRotate == true) {
                    On_A_down();
                }

                if (GM.prevState.ThumbSticks.Left.X > 0.2 && MMC.canRotate == true) {
                    On_D_down();
                }
            }
        } else {
            GM = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        }
    }

    private void On_D_down() {
        this.transform.Rotate(new Vector3(0, -rotation, 0) * Time.deltaTime * 5.0f);
    }

    private void On_A_down() {
        this.transform.Rotate(new Vector3(0, rotation, 0) * Time.deltaTime * 5.0f);
    }
}
