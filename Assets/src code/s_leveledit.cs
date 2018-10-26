using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class s_leveledit : MonoBehaviour
{
    s_grid Grid; data_level dat;

    public List<data_level> levels;
    s_object currentblock;

    public s_object dataobj;

    string m_position;  //For where the doors will teleport the player

    Button
        left_bar,
        right_bar,
        button,
        left_bar_level,
        right_bar_level,
        load_level;

    InputField inpfeild;
    GameObject cursor;
    Text txt; Text text;

    Vector3Int wordsiz_temp;
    public int z = 0;
    int sel = 0;
    int z_depth = 0;
    int currentLevNum = 0;

    public enum MO
    {
        Z,
        NO_Z
    }
    public MO TI;

    public enum TOOLKT
    {
        BRUSH,
        EDIT
    }
    public TOOLKT TOOLKIT;

    private void Start()
    {
        if (GameObject.Find("Position") != null)
            txt = GameObject.Find("Position").GetComponent<UnityEngine.UI.Text>();

        cursor = GameObject.Find("Cursor");
        left_bar = GameObject.Find("<").GetComponent<Button>();
        right_bar = GameObject.Find(">").GetComponent<Button>();
        button = GameObject.Find("Brush").GetComponent<Button>();
        load_level = GameObject.Find("Load Level").GetComponent<Button>();
        inpfeild = GameObject.Find("Level Name").GetComponent<InputField>();

        text = button.gameObject.transform.GetChild(0).GetComponent<Text>();
        Grid = GetComponent<s_grid>();
        Grid.Initialize();
        currentblock = Grid.BlockTypes[0];

        if (Resources.Load("Game") != null)
        {
            LevelStructureLoad();
        }
    }
    private void OnGUI()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        switch (TOOLKIT)
        {
            case TOOLKT.BRUSH:

                break;

            case TOOLKT.EDIT:

                if (dataobj == null)
                {
                    s_object obj = Grid.GetObjectFromGrid(new Vector3(mousepos.x, mousepos.y, z * Grid.GetNodeLength));
                    if (obj == null)
                        break;
                    object otype = obj.GetType();

                    if (Input.GetMouseButtonDown(0))
                        dataobj = obj;

                }
                else
                    DisplayData(dataobj);

                break;
        }

        z_depth = (int)GUI.VerticalSlider(new Rect(45, 55, 150, 75), z_depth, 0, 10);
        wordsiz_temp.x = (int)GUI.HorizontalSlider(new Rect(32, 23, 150, 23), wordsiz_temp.x, 1, 45);
        wordsiz_temp.y = (int)GUI.VerticalSlider(new Rect(32, 45, 23, 150), wordsiz_temp.y, 1, 45);
        GUI.Label(new Rect(0,0, 90, 90),"(" + wordsiz_temp.x + ", " + wordsiz_temp.y + ")" + " Z-Fill: " + z_depth);
        
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            z++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            z--;
        }
        z = Mathf.Clamp(z, 0, Grid.worldsize.z);

        switch (TI)
        {
            case MO.Z:

                if (Input.GetKeyDown(KeyCode.B))
                    TI = MO.Z;

                break;

            case MO.NO_Z:

                if (Input.GetKeyDown(KeyCode.B))
                    TI = MO.NO_Z;

                break;
        }
        UpdateMouseInput();

    }

    public void LevelStructureLoad()
    {
        GameObject lev = (GameObject)Resources.Load("Game");
        s_levelstructure l = lev.GetComponent<s_levelstructure>();

        List<data_level> lstruct = l.levels;

        levels = lstruct;
    }

    public void UpdateMouseInput()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mousepogrid = Grid.GetObjectPositionToGrid(mousepos);

        cursor.transform.position = new Vector3(mousepogrid.x * Grid.GetNodeLength, mousepogrid.y * Grid.GetNodeLength + z * Grid.GetNodeLength);

        if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.Space))
            m_position += "\n" + "mpos: " + new Vector3Int(mousepogrid.x, mousepogrid.y, z) + " in area " + levels[currentLevNum].name;

        if (Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.Space))
            m_position = string.Empty;

        if (Grid.GetObjectFromGrid(new Vector3(mousepogrid.x * Grid.GetNodeLength, mousepogrid.y * Grid.GetNodeLength, z * Grid.GetNodeLength)))
            txt.text = "Space + A to memorise position \n" + "Space + B to delete position \n" +
                "Position: " + new Vector3Int(mousepogrid.x, mousepogrid.y, z) + " Focusing on: " +
                Grid.GetObjectFromGrid(new Vector3(mousepogrid.x * Grid.GetNodeLength, mousepogrid.y * Grid.GetNodeLength, z * Grid.GetNodeLength))
                + " " + m_position;
        else
            txt.text = "Space + A to memorise position \n" + "Space + B to delete position \n" +
                "Position: " + new Vector3Int(mousepogrid.x, mousepogrid.y, z)
                + " " + m_position;
        

        switch (TOOLKIT)
        {
            case TOOLKT.BRUSH:

                text.text = "Brush: " + currentblock.name;

                if (Input.GetMouseButton(0))
                    Grid.SpawnObject(currentblock.name, new Vector3(mousepos.x, mousepos.y, z * Grid.GetNodeLength));

                if (Input.GetMouseButton(1))
                    Grid.DespawnObject(new Vector3(mousepos.x, mousepos.y, z * Grid.GetNodeLength));
                break;
        }
    }

    public void SwitchTooklit()
    {
        if (TOOLKIT == TOOLKT.BRUSH)
        {
            TOOLKIT = TOOLKT.EDIT;
            left_bar.gameObject.SetActive(false);
            right_bar.gameObject.SetActive(false);
            text.text = "Edit Data";
        }
        else {
            TOOLKIT = TOOLKT.BRUSH;
            left_bar.gameObject.SetActive(true);
            right_bar.gameObject.SetActive(true);
            text.text = "Brush: " + currentblock.name;
        }
    }

    public void SwitchLevel(int i)
    {
        currentLevNum += i;
        currentLevNum = Mathf.Clamp(currentLevNum, 0, levels.Count - 1);
        load_level.transform.GetChild(0).GetComponent<Text>().text = "Load Level " + currentLevNum;
    }

    public void SwitchObject(int i)
    {
        sel += i;
        sel = Mathf.Clamp(sel, 0, Grid.BlockTypes.Count - 1);
        currentblock = Grid.BlockTypes[sel];
    }

    public void DisplayData(s_object otype)
    {
        string datatype = otype.GetType().ToString();

        switch (datatype)
        {
            case "o_door":

                o_door door = (o_door)otype;
                door.location = GUI.TextField(new Rect(50, 60, 80, 20), door.location);
                if (levels.Exists(d => d.name == door.location))
                {
                    GUI.Box(new Rect(50, 80, 160, 20), "Position: " + door.teleport_position);
                    data_level dat = levels.Find(d => d.name == door.location);
                    door.teleport_position.x = (int)GUI.HorizontalSlider(new Rect(50, 100, 90, 20), door.teleport_position.x, dat.size_x, 0);
                    door.teleport_position.y = (int)GUI.HorizontalSlider(new Rect(50, 120, 90, 20), door.teleport_position.y, dat.size_y, 0);
                    door.teleport_position.z = (int)GUI.HorizontalSlider(new Rect(50, 140, 90, 20), door.teleport_position.z, dat.size_z, 0);

                    GUI.Box(new Rect(50, 160, 270, 20), "Requirements: " + door.DOOR_STATE.ToString());

                    if (GUI.Button(new Rect(50, 180, 200, 40), "Change requirement +"))
                        door.DOOR_STATE++;
                    if (GUI.Button(new Rect(250, 180, 200, 40), "Change requirement -"))
                        door.DOOR_STATE--;

                    door.DOOR_STATE = (o_door.DOOR_MODE)Mathf.Clamp((float)door.DOOR_STATE, 0, 2);

                    switch (door.DOOR_STATE)
                    {
                        case o_door.DOOR_MODE.WATERSTONE:

                            door.locked = true;
                            GUI.Box(new Rect(50, 220, 270, 20), "Water stones: " + door.required_waterstones);
                            door.required_waterstones = (int)GUI.HorizontalSlider(new Rect(50, 240, 90, 20), door.required_waterstones, 0, 30);
                            break;

                        case o_door.DOOR_MODE.NONE:
                            door.locked = false;
                            break;
                    }
                }
                break;

            case "o_block":

                dataobj = null;
                break;
        }

        if (GUI.Button(new Rect(50, 260, 90, 20), "Exit"))
        {
            dataobj = null;
        }
    }

    public void ClearLevel()
    {
        Grid.ClearTiles();
        Grid.worldsize.x = wordsiz_temp.x;
        Grid.worldsize.y = wordsiz_temp.y;
        Grid.Initialize(z_depth);
    }

    public void LoadLevel()
    {
        Grid.ClearTiles();
        inpfeild.text = levels[currentLevNum].name;
        Vector3Int size = new Vector3Int(levels[currentLevNum].size_x, levels[currentLevNum].size_y, levels[currentLevNum].size_z);
        
        data_level dat = levels[currentLevNum];
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
                    data_level.data_item it = dat.items.Find(bl =>  bl.x == x && bl.y == y && bl.z == z);

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

    public void SaveLevel()
    {
        s_object[,,] objec = Grid.blocks;
        
        data_level da = new data_level();
        da.blocks = new List<data_level.data_block>();
        da.doors = new List<data_level.data_door>();

        da.size_x = Grid.worldsize.x;
        da.size_y = Grid.worldsize.y;
        da.size_z = Grid.worldsize.z;

        da.name = inpfeild.text;
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
                                da.doors.Add(new data_level.data_door(x, y, z, 
                                    door.locked,
                                    door.name,
                                    door.location, 
                                       new Vector3Int(
                                           door.teleport_position.x,
                                           door.teleport_position.y,
                                           door.teleport_position.z
                                           ),
                                       door.DOOR_STATE,
                                       door.required_waterstones));
                            else
                                da.doors.Add(new data_level.data_door(x, y, z,door.name ,
                                    door.locked, 
                                    new Vector3Int(
                                           door.teleport_position.x,
                                           door.teleport_position.y,
                                           door.teleport_position.z
                                           ),
                                       door.DOOR_STATE,
                                       door.required_waterstones
                                       ));
                        }
                        else if (objec[x, y, z].GetType() == typeof(o_block))
                        {
                            da.blocks.Add(new data_level.data_block(x, y, z, objec[x, y, z].name));
                        }
                        else if (objec[x, y, z].GetType() == typeof(o_item))
                        {
                            o_item item = (o_item)objec[x, y, z];
                            da.items.Add(new data_level.data_item(x, y, z,item.name, item.item));
                        }
                    }   
                }
            }
        }
        
        if (levels.Count == 0 || levels[currentLevNum].name != da.name)
        {
            levels.Add(da);
        }else if (levels[currentLevNum].name == da.name)
        {
            levels[currentLevNum] = da;
        }
    }

    public void SaveEntireLevelStructure()
    {
        GameObject lev = (GameObject)Resources.Load("Game");
        s_levelstructure l = lev.GetComponent<s_levelstructure>();

        List<data_level> lstruct = new List<data_level>();
        lstruct = levels;

        foreach (data_level le in lstruct)
        {
            l.levels.Add(le);
        }
    }

}

