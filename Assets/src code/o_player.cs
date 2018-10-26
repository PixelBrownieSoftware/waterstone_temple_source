using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_player : s_object {

    float speed = 1.4f;
    s_gamemanager gman;
    Animator anim;
    public int water_stones_cap;

    string ite;
    string ite2;
    int saved_number_of_water = 0;
    string reqtext;
    bool isdying = false;

    new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();

        gman = GameObject.Find("Manager").GetComponent<s_gamemanager>();
        worldpos.z = 40;
        iskenematic = true;
        Collider.size = new Vector3(20,20,20);
    }

    IEnumerator DyingAnim()
    {
        isdying = true;
        iskenematic = false;
        Playsound("die_sound");

        yield return new WaitForSeconds(0.6f);
        gman.ReloadRoom();
        isdying = false;
        iskenematic = true;
    }

	new void Update ()
    {
        if (CheckCollisionInside(this) != null)
        {
            s_object collidedobj = CheckCollisionInside(this);
            if (collidedobj.GetType() == typeof(o_door))
            {
                o_door d = collidedobj.GetComponent<o_door>();
                if (d.CanPass())
                {
                    reqtext = "";
                    if (d.location != "Ending")
                    {
                        saved_number_of_water = water_stones_cap;
                        gman.LoadRoom(d);
                    }
                    else
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
                }
                else
                {
                    reqtext = d.PassReq();
                }
            }
            else
            if (collidedobj.GetType() == typeof(o_block))
            {
                if (collidedobj.name == "Spike")
                {
                    if (!isdying)
                    {
                        water_stones_cap = saved_number_of_water;
                        isgrounded = false;
                        StartCoroutine(DyingAnim());
                    }
                }
            }
            else
            if (collidedobj.GetType() == typeof(o_item))
            {
                o_item i = collidedobj.GetComponent<o_item>();
                water_stones_cap++;
                grid.DespawnObject(i);
            }
        }
        else
            reqtext = "";
        base.Update();

        ite = "Water stones: " + water_stones_cap;

        gman.txt.text = ite + "\n" + reqtext;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            anim.SetBool("Walk", true);
        }
        else
            anim.SetBool("Walk", false);

        if (!Input.GetButton("Vertical"))
        {
            anim.SetFloat("Y", 0);
            anim.SetFloat("X", (int)Input.GetAxisRaw("Horizontal"));
            if (Input.GetKey(KeyCode.RightArrow))
            {
                vecloicty.x = speed;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                vecloicty.x = -speed;
            }
        }
        if (!Input.GetButton("Horizontal"))
        {
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", (int)Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(KeyCode.UpArrow))
            {
                vecloicty.y = speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                vecloicty.y = -speed;
            }
        }
        
        if (isgrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vecloicty.z = 5;
            }
        }

    }
    
}
