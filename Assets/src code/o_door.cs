using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_door : o_block  {

    public bool locked;
    public string location;
    public Vector3Int teleport_position;
    Animator anim;
    public bool is_ending = false;

    public int required_waterstones;

    public enum DOOR_MODE
    {
        NONE,
        WATERSTONE,
    }
    public DOOR_MODE DOOR_STATE = DOOR_MODE.NONE;

    private new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        isshadow = false;
    }

    public bool CanPass()
    {
        if (locked)
        {
            switch (DOOR_STATE)
            {

                case DOOR_MODE.WATERSTONE:

                    if (grid.player != null)
                    {
                        int o = grid.player.water_stones_cap;
                        if (o >= required_waterstones)
                        {
                            locked = false;
                        }
                    }
                    return false;
            }
        }
        return true;
    }
    public string PassReq()
    {
        if (locked)
        {
            switch (DOOR_STATE)
            {

                case DOOR_MODE.WATERSTONE:

                    if (grid.player != null)
                    {
                        int o = grid.player.water_stones_cap;
                        if (o >= required_waterstones)
                        {
                            locked = false;
                        }
                    }
                    return "Requires " + required_waterstones + " water stones";
            }
        }
        return "";
    }


    new private void Update()
    {
        if (locked)
        {
            anim.SetBool("Locked", true);
            switch (DOOR_STATE)
            {

                case DOOR_MODE.WATERSTONE:

                    if (grid.player != null)
                    {
                        int o = grid.player.water_stones_cap;
                        if (o >= required_waterstones)
                        {
                            locked = false;
                        }
                    }

                    break;
            }
        }
        else
            anim.SetBool("Locked", false);
        base.Update();
    }

}
