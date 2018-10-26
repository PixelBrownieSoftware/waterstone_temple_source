using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_cutscenefunc : MonoBehaviour {

    public void SwitchScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }
    public void SetScreen()
    {
        Screen.SetResolution(512, 288, Screen.fullScreen);
    }

}
