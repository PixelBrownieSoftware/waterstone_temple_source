using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class s_gamemanager : MonoBehaviour {

    s_grid Grid;
    o_player Player;
    public UnityEngine.UI.Text txt;

    List<data_level> levels = new List<data_level>();
    int currentLevNum = 0;
    Vector3Int last_player_pos = new Vector3Int(2,2,2);

    s_levelstructure ing;

    void Start () {
        Grid = GetComponent<s_grid>();
        Grid.Initialize();
        Player = GameObject.Find("player").GetComponent<o_player>();

        GameObject lev = GameObject.Find("Game");
        s_levelstructure l = lev.GetComponent<s_levelstructure>();

        GameObject ingalev = GameObject.Find("InGame");
        ing = ingalev.GetComponent<s_levelstructure>();

        List<data_level> lstruct = new List<data_level>();
        lstruct = l.levels;

        if (GameObject.Find("Position") != null)
            txt = GameObject.Find("Position").GetComponent<UnityEngine.UI.Text>();

        //Copy all the data from the Editor's to the inGame's
        foreach (data_level da in lstruct)
        {
            ing.levels.Add(da);
        }

        levels = ing.levels;
        //De-references the original levels object so it dosen't get changed.
        l = null;
        LoadLevel();
    }

    void Save()
    {
        s_object[,,] objec = Grid.blocks;

        data_level da = new data_level();
        da.blocks = new List<data_level.data_block>();
        da.doors = new List<data_level.data_door>();

        da.size_x = Grid.worldsize.x;
        da.size_y = Grid.worldsize.y;
        da.size_z = Grid.worldsize.z;
        da.name = levels[currentLevNum].name;
        for (int x = 0; x < Grid.worldsize.x - 1; x++)
        {
            for (int y = 0; y < Grid.worldsize.y - 1; y++)
            {
                for (int z = 0; z < Grid.worldsize.z - 1; z++)
                {
                    if (objec[x, y, z])
                    {
                        //This is a pretty crude approach, trying to save everything into seperate things

                        if (objec[x, y, z].GetType() == typeof(o_door))
                        {
                            o_door door = (o_door)objec[x, y, z];
                            if (door.location != "")
                                da.doors.Add(new data_level.data_door(x, y, z, door.locked,
                                        door.name,
                                       door.location,
                                       new Vector3Int(
                                           door.teleport_position.x,
                                           door.teleport_position.y,
                                           door.teleport_position.z
                                           ), door.DOOR_STATE,
                                       door.required_waterstones));
                            else
                                da.doors.Add(new data_level.data_door(x, y, z, door.name, door.locked, new Vector3Int(
                                           door.teleport_position.x,
                                           door.teleport_position.y,
                                           door.teleport_position.z
                                           ), door.DOOR_STATE,
                                       door.required_waterstones));
                        }
                        else if (objec[x, y, z].GetType() == typeof(o_block))
                        {
                            da.blocks.Add(new data_level.data_block(x, y, z, objec[x, y, z].name));
                        }
                        else if (objec[x, y, z].GetType() == typeof(o_item))
                        {
                            o_item item = (o_item)objec[x, y, z];
                            da.items.Add(new data_level.data_item(x, y, z, item.name, item.item));
                        }
                    }
                }
            }

        }
        levels[currentLevNum] = da;
        ing.levels[currentLevNum] = da;
    }

    public void LoadRoom(o_door door)
    {
        //TODO: SAVE DATA INTO 'INGAME'

        Save();

        last_player_pos = door.teleport_position;

        for (int i = 0; i < levels.Count; i++)
        {
            if (levels[i].name == door.location)
            {
                currentLevNum = i;
                break;
            }
        }
        LoadLevel();
    }

    public void ReloadRoom()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        Player.worldpos = new Vector3Int(
            last_player_pos.x * Grid.GetNodeLength,
            last_player_pos.y * Grid.GetNodeLength,
            last_player_pos.z * Grid.GetNodeLength);
        data_level dat = levels[currentLevNum];
        Grid.ClearTiles();
        Grid.worldsize = new Vector3Int(dat.size_x, dat.size_y, 45);
        Grid.Initialize();

        for (int x = 0; x < Grid.worldsize.x - 1; x++)
        {
            for (int y = 0; y < Grid.worldsize.y - 1; y++)
            {
                for (int z = 0; z < Grid.worldsize.z - 1; z++)
                {
                    data_level.data_block blok = dat.blocks.Find(bl => bl.x == x && bl.y == y && bl.z == z);
                    data_level.data_door dr = dat.doors.Find(bl => bl.x == x && bl.y == y && bl.z == z);
                    data_level.data_item it = dat.items.Find(bl => bl.x == x && bl.y == y && bl.z == z);

                    if (blok != null)
                    {
                        o_block block = Grid.SpawnObject(blok.name, new Vector3Int(x, y, z));

                    }

                    if (dr != null)
                    {
                        o_block block = Grid.SpawnObject(dr.name, new Vector3Int(x, y, z));
                        
                        o_door door = block.GetComponent<o_door>();
                        door.location = dr.lev;
                        door.teleport_position = new Vector3Int(dr.tele_x, dr.tele_y, dr.tele_z);
                        door.locked = dr.locked;
                        door.required_waterstones = dr.require;
                        door.DOOR_STATE = (o_door.DOOR_MODE)dr.doortype;
                    }
                    if (it != null)
                    {
                        o_block block = Grid.SpawnObject(it.name, new Vector3Int(x, y, z));
                        o_item item = block.GetComponent<o_item>();
                        item.item = it.item;
                    }
                }
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Screen.fullScreen)
                Screen.fullScreen = false;
            else
                Screen.fullScreen = true;
        }
    }
}
