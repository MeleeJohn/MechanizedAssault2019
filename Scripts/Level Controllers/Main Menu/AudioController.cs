using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public float songIntroLength;
    public AudioClip songIntro;
    public AudioClip songMainChorus;
    public bool playAtStart;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(playAtStart == true) {
            StartCoroutine(PlayMusic());
        }
    }

    public IEnumerator PlayMusic() {
        audioSource.clip = songIntro;
        audioSource.Play();
        yield return new WaitForSecondsRealtime(songIntroLength);
        audioSource.clip = songMainChorus;
        audioSource.loop = true;
        audioSource.Play();
    }
}
