using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_object : MonoBehaviour {

    public struct o_bounding_box
    {
        public Vector3 size;
        public Vector3 offset;
        public o_bounding_box(Vector3 offset, Vector3 size)
        {
            this.size = size;
            this.offset = offset;
        }
    }

    public float im_z;
    public Vector3 worldpos;
    public bool iskenematic = false;
    public const float gravity = 0.25f;
    internal o_bounding_box Collider;
    public Vector3 vecloicty;
    protected s_grid grid;
    public bool solid = true;
    protected bool isgrounded = false;
    protected SpriteRenderer rend = null;
    int nodeleng;

	protected void Start ()
    {
        rend = GetComponent<SpriteRenderer>();
        grid = GameObject.Find("Manager").GetComponent<s_grid>();
        nodeleng = grid.GetNodeLength;
        Collider = new o_bounding_box(
            new Vector3(0, 0, 0),
            new Vector3(nodeleng, nodeleng, nodeleng));
	}
    
    public void Playsound(string sound)
    {
        s_soundmanager.PlaySound(Resources.Load("sound/" + sound) as AudioClip);
    }

    public void CheckCollision()
    {
        //For checking the player's future 'XYZ' positions
        float x = worldpos.x + (Collider.size.x  * (int)Mathf.Sign(vecloicty.x));
        float y = worldpos.y + (Collider.size.y * (int)Mathf.Sign(vecloicty.y));
        float z = worldpos.z + (Collider.size.z * (int)Mathf.Sign(vecloicty.z));

        Vector3 futurepos = new Vector3(x, y, z);
        Vector3Int futrgridpos = grid.GetObjectPositionToGrid(futurepos);
        Vector3Int worlgridpos = grid.GetObjectPositionToGrid(worldpos);
        
        if (futrgridpos.x >= 0 && futrgridpos.x < grid.worldsize.x - 1)
        {
            if (grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z] != null &&
                grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z].solid)
            {
                vecloicty.x = 0;
            }
        }
        else
            vecloicty.x = 0;

        if (futrgridpos.y >= 0 && futrgridpos.y < grid.worldsize.y - 1)
        {
            if (grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z] != null &&
                grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z].solid)
            {
                vecloicty.y = 0;
            }
        }
        else
            vecloicty.y = 0;

        if (grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z] != null &&
            grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z].solid)
        {
            worldpos.z = (int)((worldpos.z / 10)) * 10; // This is so that the player lands perfectly on to the block.
            isgrounded = true;
            vecloicty.z = 0;
        }
        else
            isgrounded = false;
        
    }

    /// <summary>
    /// The XYZ cooridinates must be raw, in world co-ordinates as they will be calculated in this function.
    /// </summary>
    /// <returns></returns>
    public s_object GetTile(float x, float y, float z)
    {
        Vector3Int vec = grid.GetObjectPositionToGrid(new Vector3(x, y, z));
        

        if (grid.blocks[vec.x, vec.y, vec.z])
        {
            return grid.blocks[vec.x, vec.y, vec.z];
        }
        return null;
    }

    public s_object CheckCollisionInside(s_object obj)
    {
        //For checking the player's future 'X' position
        float x = worldpos.x;
        float y = worldpos.y;
        float z = worldpos.z;

        Vector3 futurepos = new Vector3(x, y, z);

        Vector3Int futrgridpos = grid.GetObjectPositionToGrid(futurepos);
        Vector3Int worlgridpos = grid.GetObjectPositionToGrid(worldpos);

        if (futrgridpos.y > 1 && futrgridpos.y < grid.worldsize.y - 2)
            if (grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z] != null)
                return grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z];

        if (futrgridpos.y > 1 && futrgridpos.y < grid.worldsize.y - 2)
            if (grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z] != null)
                return grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z];

        if (grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z] != null)
            return grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z];

        return null;
    }
    
    public s_object CheckCollision(s_object obj)
    {
        //For checking the player's future 'X' position
        float x = worldpos.x + (Collider.size.x);
        float y = worldpos.y + (Collider.size.y);
        float z = worldpos.z + (Collider.size.z);

        Vector3 futurepos = new Vector3(x, y, z);
        Vector3Int futrgridpos = grid.GetObjectPositionToGrid(futurepos);
        Vector3Int worlgridpos = grid.GetObjectPositionToGrid(worldpos);
        for (int i = -1; i != 1; i++)
        {
            if (i == 0)
                continue;

            if (futrgridpos.y + i > 1 && futrgridpos.y + i < grid.worldsize.y - 2)
                if (grid.blocks[futrgridpos.x + i, worlgridpos.y, worlgridpos.z] != null)
                    return grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z];
        }

        for (int i = -1; i != 1; i++)
        {
            if (i == 0)
                continue;

            if (futrgridpos.y + i > 1 && futrgridpos.y + i < grid.worldsize.y - 2)
                if (grid.blocks[worlgridpos.x, futrgridpos.y + i, worlgridpos.z] != null)
                    return grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z];
        }
        for (int i = -1; i != 1; i++)
        {
            if (i == 0)
                continue;

            if (grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z] != null)
                return grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z + i];
        }

        
        return null;
    }
    public s_object CheckCollision(s_object obj, Vector3 velocity)
    {
        //For checking the player's future 'X' position
        float x = worldpos.x + (Collider.size.x * (int)Mathf.Sign(vecloicty.x));
        float y = worldpos.y + (Collider.size.y * (int)Mathf.Sign(vecloicty.y));
        float z = worldpos.z + (Collider.size.z * (int)Mathf.Sign(vecloicty.z));

        Vector3 futurepos = new Vector3(x, y, z);
        Vector3Int futrgridpos = grid.GetObjectPositionToGrid(futurepos);
        Vector3Int worlgridpos = grid.GetObjectPositionToGrid(worldpos);

        if (futrgridpos.y > 1 && futrgridpos.y < grid.worldsize.y - 2)
            if (grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z] != null)
                return grid.blocks[futrgridpos.x, worlgridpos.y, worlgridpos.z];

        if (futrgridpos.y > 1 && futrgridpos.y < grid.worldsize.y - 2)
            if (grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z] != null)
                return grid.blocks[worlgridpos.x, futrgridpos.y, worlgridpos.z];

        if (grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z] != null)
            return grid.blocks[worlgridpos.x, worlgridpos.y, futrgridpos.z];

        return null;
    }

    protected void Update()
    {
        if (rend != null)
            rend.sortingOrder = (-(int)worldpos.z / nodeleng 
                + (int)worldpos.y / nodeleng) * -1;
        

        if (iskenematic)
        {
            if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                vecloicty.x = 0;

            if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
                vecloicty.y = 0;

            vecloicty.z -= gravity;
            CheckCollision();
            worldpos += vecloicty;
        }

        transform.position = new Vector2(worldpos.x, worldpos.y + worldpos.z);
	}
}
