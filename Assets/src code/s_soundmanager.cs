using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_soundmanager : MonoBehaviour {

    public AudioSource audiosrc;
    AudioSource music;
    static s_soundmanager sman;

    void Start() {
        sman = this;
        audiosrc = GetComponent<AudioSource>();
        music = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip play)
    {
        sman.audiosrc.PlayOneShot(play);
    }

	void Update () {
		
	}
}
