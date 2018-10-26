using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_item : o_block {

    public string item;

	new void Start ()
    {
        isshadow = false;
        solid = false;
        base.Start();
	}
	
	new void Update () {
        base.Update();
	}
}
