using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class s_mainmenu : MonoBehaviour {

    Text textthing;
    Animator anim;
    float timer = 0;

    void Start ()
    {
        anim = GetComponent<Animator>();
        textthing = GameObject.Find("Text").GetComponent<Text>();
        textthing.text = "Press X to start \nF for fullscreen";
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            textthing.text = "Loading...";
            anim.SetBool("Play", true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.SetResolution(512, 288, true);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadSceneAsync("InGame");
    }
}
