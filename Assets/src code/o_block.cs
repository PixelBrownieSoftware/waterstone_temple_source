using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_block : s_object {

    public SpriteRenderer shadow = null;
    public Sprite up_connect;
    public Sprite left_connect;
    public Sprite right_connect;
    public Sprite down_connect;

    public Sprite horz_connect;
    public Sprite vert_connect;

    public Sprite horz_up_connect;
    public Sprite horz_down_connect;
    public Sprite vert_left_connect;
    public Sprite vert_right_connect;

    public Sprite up_left_connect;
    public Sprite up_right_connect;
    public Sprite down_left_connect;
    public Sprite down_right_connect;
    public Sprite all_connect;

    protected bool isshadow = true;

    bool
        up_con,
        down_con,
        left_con,
        right_con;

    new void Start ()
    {
        //shadow = transform.Find("BlockShade").gameObject.GetComponent<SpriteRenderer>();
        base.Start();
    }
    new void Update()
    {
        base.Update();
        if (grid != null)
        {
            if (grid.player != null)
            {
                Vector3Int pos = grid.GetObjectPositionToGrid(worldpos);
                Vector3Int playervec = grid.GetObjectPositionToGrid(grid.player.worldpos);
                if (pos.x == playervec.x && pos.y == playervec.y)
                    rend.color = new Color(0.2f, 0.1f, 0.8f, 0.5f);
                else
                    rend.color = Color.white;
            }
        }
    }

    bool CheckShadows
    {
        get
        {
            Vector3Int vec = grid.GetObjectPositionToGrid(worldpos);

            for (int i = vec.z; i < 15; i++)
            {
                s_object obj = grid.GetObjectFromGrid(new Vector3(worldpos.x, worldpos.y, i * grid.GetNodeLength));

                if (obj != null && i == vec.z + 1)
                    return false;

                if (obj != null)
                    return true;
            }
            return false;
        }
    }

    void CheckSides()
    {
        //I'll probably come up with something better later
        //This checks all sides of the sprite

        //Left
        if (left_con)
        {
            rend.sprite = left_connect;
            if (right_con)
                rend.sprite = horz_connect;
        }

        //Right
        if (right_con)
        {
            rend.sprite = right_connect;
            if (left_con)
                rend.sprite = horz_connect;
        }

        //Up
        if (up_con)
        {
            rend.sprite = up_connect;
            if (left_con)
            {
                rend.sprite = up_left_connect;
                if (right_con)
                    rend.sprite = horz_up_connect;
            }
            if (right_con)
            {
                rend.sprite = up_right_connect;
                if (left_con)
                    rend.sprite = horz_up_connect;
            }
        }

        //Down
        if (down_con)
        {
            rend.sprite = down_connect;
            if (left_con)
            {
                rend.sprite = down_left_connect;
                if (right_con)
                    rend.sprite = horz_down_connect;
            }
            if (right_con)
            {
                rend.sprite = down_right_connect;
                if (left_con)
                    rend.sprite = horz_down_connect;
            }
        }
    }

}
