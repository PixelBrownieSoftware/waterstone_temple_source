using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_camera : MonoBehaviour {

    public GameObject player;
    
	
	void Update () {
        if (player != null)
        {
            transform.position = player.transform.position;
        }	
	}
}
