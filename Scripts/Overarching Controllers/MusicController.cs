using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public AudioSource mainBeat;
    public AudioSource SecondaryBeat;
    public AudioSource Chorus;

    public void SecondaryBeatStart() {
        SecondaryBeat.PlayOneShot(SecondaryBeat.clip);
    }

    public void ChorusStart() {
        Chorus.PlayOneShot(Chorus.clip);
    }
}
