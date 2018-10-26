using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class s_levelstructure : MonoBehaviour
{
    public List<data_level> levels;
    data_level starting_level;
    data_level playerpos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

[Serializable]
public class data_level
{
    public int size_x, size_y, size_z;
    public string name;
    
    public List<data_door> doors = new List<data_door>();
    public List<data_block> blocks = new List<data_block>();
    public List<data_item> items = new List<data_item>();

    [System.Serializable]
    public class data_block
    {
        public string name;
        public data_block(int x, int y, int z, string name)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.name = name;
        }

        //This is to collect the extra data that isn't in this class
        public virtual Dictionary<string, object> GetUniqueData()
        {
            return null;
        }
        
        public int x, y, z;
    }

    [System.Serializable]
    public class data_item : data_block
    {
        public data_item(int x, int y, int z, string name, string item
            ) 
            : base(x, y, z, name)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.name = name;
            this.item = item;
        }
        public string item;
    }
    [System.Serializable]
    public class data_tank : data_block
    {
        public data_tank(int x, int y, int z, string name  //TODO: ADD REQUIREMENTS
            )
            : base(x, y, z, name)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.name = name;
        }
    }

    [System.Serializable]
    public class data_door : data_block
    {
        public bool locked;
        public int tele_x, tele_y, tele_z;
        public string lev;
        public int require;
        public int doortype;

        public data_door(int x, int y, int z, bool locked, string name, string lev, Vector3Int teleport, o_door.DOOR_MODE drtype, int req)
            : base(x, y, z, name)
        {
            require = req;
            doortype = (int)drtype;
            tele_x = teleport.x;
            tele_y = teleport.y;
            tele_z = teleport.z;
            this.lev = lev;
            this.locked = locked;
        }

        public override Dictionary<string, object> GetUniqueData()
        {
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("tel_x" ,tele_x);
            Debug.Log(obj["tel_x"]);
            obj.Add("tel_y", tele_y);
            obj.Add("tel_z", tele_z);
            obj.Add("lock", locked);
            obj.Add("level", lev);
            return obj;
        }

        ~data_door()
        {
        }

        public data_door(int x, int y, int z, string name, bool locked, Vector3Int teleport, o_door.DOOR_MODE drtype, int req)
            : base(x, y, z, name)
        {
            require = req;
            doortype = (int)drtype;
            lev = null;
            tele_x = teleport.x;
            tele_y = teleport.y;
            tele_z = teleport.z;
            this.locked = locked;
        }
    }


}
