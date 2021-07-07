using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoRouteController : MonoBehaviour
{
    public LevelTwoController LTC;

    public bool isFlanking;
    public bool isHighway;

    private void OnTriggerEnter(Collider other) {
        /*(if(other.tag == "Player" && LTC.chooseDirection == false) {
            if (isFlanking == true) {
                LTC.LD = LevelDirection.Flanking;
                LTC.chooseDirection = true;
                LTC.GoingFlankRoute();
            }

            if (isHighway == true) {
                LTC.LD = LevelDirection.Highway;
                LTC.chooseDirection = true;
                LTC.GoingHighwayRoute();
            }
        }*/
    }
}
