using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_grid : MonoBehaviour {

    public o_block[,,] blocks;
    public GameObject block;
    const int nodeLength = 20;
    public o_player player;
    public GameObject[,,] blockobjs;
    UnityEngine.UI.Text txt;
    public GameObject blockobj;
    public Vector3Int worldsize;
    public Dictionary<string, Queue<o_block>> ObjectPool = new Dictionary<string, Queue<o_block>>();
    public List<o_block> BlockTypes = new List<o_block>();
    public GameObject LO;  //I do this for loading reasons.

    enum check
    {
        x,y,z
    }
    check chekcing;

    public int GetNodeLength
    {
        get
        {
            return nodeLength;
        }
    }

    void Awake ()
    {
        if (txt == null)
        {
            if(GameObject.Find("Position") != null)
                txt = GameObject.Find("Position").GetComponent<UnityEngine.UI.Text>();
        }

        Initialize();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            chekcing++;

        if (Input.GetKeyDown(KeyCode.Q))
            chekcing--;

        chekcing = (check)Mathf.Clamp((int)chekcing, 0, 3);
    }

    public void Initialize()
    {
        //blockobjs = new GameObject[worldsize.x, worldsize.y, worldsize.z];
        blocks = new o_block[worldsize.x, worldsize.y, worldsize.z];
        if (LO == null || ObjectPool == null)
        {
            LO = Instantiate(new GameObject(), transform.position, Quaternion.identity);
            LO.name = "Level Objects";

            int num = 1700;
            ObjectPool = new Dictionary<string, Queue<o_block>>();
            foreach (o_block b in BlockTypes)
            {
                Queue<o_block> que = new Queue<o_block>();
                for (int i = 0; i < num; i++)
                {
                    CreateBlock(b, ref que);
                }
                ObjectPool.Add(b.name, que);
            }
        }
        blocks = new o_block[worldsize.x, worldsize.y, worldsize.z];
    }

    public void CreateBlock(o_block b, ref Queue<o_block> que)
    {
        o_block blok = Instantiate(b, transform.position, Quaternion.identity);
        blok.name = b.name;
        print("OK");
        blok.transform.SetParent(LO.transform);
        blok.gameObject.SetActive(false);
        que.Enqueue(blok);
    }
    public void CreateBlock(string b, ref Queue<o_block> que)
    {
        o_block bl = BlockTypes.Find(x => x.name == b);
        o_block blok = Instantiate(bl, transform.position, Quaternion.identity);
        blok.name = b;
        print("OK");
        blok.transform.SetParent(LO.transform);
        blok.gameObject.SetActive(false);
        que.Enqueue(blok);
    }


    public void Initialize(int zdepth)
    {
        //blockobjs = new GameObject[worldsize.x, worldsize.y, worldsize.z];
        blocks = new o_block[worldsize.x, worldsize.y, worldsize.z];

        if (ObjectPool == null)
        {
            ObjectPool = new Dictionary<string, Queue<o_block>>();

            int num = 45 * 45 * worldsize.z;

            foreach (o_block b in BlockTypes)
            {
                Queue<o_block> que = new Queue<o_block>();
                for (int i = 0; i < num; i++)
                {
                    o_block blok = Instantiate(b, transform.position, Quaternion.identity);
                    blok.name = b.name;
                    blok.gameObject.SetActive(false);
                    que.Enqueue(blok);
                }
                ObjectPool.Add(b.name, que);
            }
        }
        for (int x = 0; x < worldsize.x - 1; x++)
        {
            for (int y = 0; y < worldsize.y - 1; y++)
            {
                for (int z = 0; z < worldsize.z - 1; z++)
                {
                    if (z <= zdepth)
                        SpawnObject("Block", new Vector3Int(x, y, z));
                }
            }
        }

    }
    /*
    public void DrawLevel(int i)
    {

        for (int x = 0; x != worldsize.x; x++)
        {
            for (int y = 0; y != worldsize.y; y++)
            {
                for (int z = 0; z != worldsize.z; z++)
                {
                    blockobjs[x, y, z].GetComponent<SpriteRenderer>().color = Color.clear;

                    blockobjs[x, y, z].transform.position = new Vector2(20 * x, 20 * y);

                    if (blocks[x, y, i] != null)
                    {
                        blockobjs[x, y, i].GetComponent<SpriteRenderer>().color = Color.blue + new Color(0, 0, 0, -0.5f);
                    }
                }

            }
        }
    }

    public void DrawLevel()
    {
        for (int x = 0; x != worldsize.x; x++)
        {
            for (int y = 0; y != worldsize.y; y++)
            {
                for (int z = 0; z != worldsize.z; z++)
                {
                    blockobjs[x, y, z].GetComponent<SpriteRenderer>().color = Color.clear;

                    if (blocks[x, y, z] != null)
                    {
                        blockobjs[x, y, z].GetComponent<SpriteRenderer>().color = Color.blue + new Color(0, 0, 0, -0.5f);

                        blockobjs[x, y, 0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    }

                    blockobjs[x, worldsize.y - 1, z].GetComponent<SpriteRenderer>().color = Color.black + new Color(0, 0, 0, -0.5f);

                    blockobjs[x, 0, z].GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0.1f, 0.2f);
                    blockobjs[0, y, z].GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0.1f, 0.2f);
                    if (player != null)
                    {
                        if (new Vector3Int(x, y, z) == GetObjectPositionToGrid(player.worldpos))
                        {
                            txt.text = (GetObjectPositionToGrid(player.worldpos)).ToString();
                            blockobjs[x, y, 0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                            blockobjs[x, y, z].GetComponent<SpriteRenderer>().color = Color.red;
                        }
                    }
                    blockobjs[x, y, z].transform.position = new Vector2(nodeLength * x, nodeLength * y + nodeLength * z);
                }

            }
        }
    }

    */
    public void ClearTiles()
    {
        s_object[,,] objec = blocks;
        for (int x = 0; x < worldsize.x - 1; x++)
        {
            for (int y = 0; y < worldsize.y - 1; y++)
            {
                for (int z = 0; z < worldsize.z - 1; z++)
                {
                    if (objec[x, y, z] != null)
                        DespawnObject(new Vector3Int(x, y, z));
                }
            }
        }

        
    }
    public void DespawnObject(s_object obj)
    {
        Vector3Int posInGrid = GetObjectPositionToGrid(obj.worldpos);

        bool check = blocks[posInGrid.x, posInGrid.y, posInGrid.z] == null;

        if (check)
            return;

        if (posInGrid.x > worldsize.x || posInGrid.x < 0 ||
            posInGrid.y > worldsize.y || posInGrid.y < 0 ||
            posInGrid.z > worldsize.z || posInGrid.z < 0)
            return;

        blocks[posInGrid.x, posInGrid.y, posInGrid.z].gameObject.SetActive(false);
        blocks[posInGrid.x, posInGrid.y, posInGrid.z] = null;
    }
    public void DespawnObject(Vector3 position)
    {
        Vector3Int posInGrid = GetObjectPositionToGrid(position);

        bool check = blocks[posInGrid.x, posInGrid.y, posInGrid.z] == null;

        if (check)
            return;

        if (posInGrid.x > worldsize.x || posInGrid.x < 0 ||
            posInGrid.y > worldsize.y || posInGrid.y < 0 ||
            posInGrid.z > worldsize.z || posInGrid.z < 0)
            return;

        blocks[posInGrid.x, posInGrid.y, posInGrid.z].gameObject.SetActive(false);
        blocks[posInGrid.x, posInGrid.y, posInGrid.z] = null;
    }
    public void DespawnObject(Vector3Int position)
    {
        Vector3Int posInGrid = position;

        if (posInGrid.x > worldsize.x || posInGrid.x < 0 ||
            posInGrid.y > worldsize.y || posInGrid.y < 0 ||
            posInGrid.z > worldsize.z || posInGrid.z < 0)
            return;


        blocks[posInGrid.x, posInGrid.y, posInGrid.z].gameObject.SetActive(false);
        blocks[posInGrid.x, posInGrid.y, posInGrid.z] = null;
    }

    public o_block SpawnObject(string nameofobj, Vector3Int position)
    {
        Vector3Int posInGrid = position;

        if (posInGrid.x > worldsize.x - 2 || posInGrid.x < 0 ||
            posInGrid.y > worldsize.y - 2 || posInGrid.y < 0 ||
            posInGrid.z > worldsize.z - 2 || posInGrid.z < 0)
            return null;

        if (blocks[posInGrid.x, posInGrid.y, posInGrid.z] != null)
            return null;

        print(ObjectPool[nameofobj].Peek().gameObject.activeSelf);

        if (ObjectPool[nameofobj].Peek().gameObject.activeSelf)
            return null;

        o_block bl = ObjectPool[nameofobj].Dequeue();
        ObjectPool[nameofobj].Enqueue(bl);

        bl.worldpos = new Vector3(posInGrid.x * nodeLength, posInGrid.y * nodeLength, posInGrid.z * nodeLength);

        bl.gameObject.SetActive(true);
        blocks[posInGrid.x, posInGrid.y, posInGrid.z] = bl;
        return bl;
    }
    public o_block SpawnObject(string nameofobj, Vector3 position)
    {
        Vector3Int posInGrid = GetObjectPositionToGrid(position);

        if (posInGrid.x > worldsize.x - 2 || posInGrid.x < 0 ||
            posInGrid.y > worldsize.y - 2 || posInGrid.y < 0 ||
            posInGrid.z > worldsize.z - 2 || posInGrid.z < 0)
            return null;

        if (blocks[posInGrid.x, posInGrid.y, posInGrid.z] != null)
            return null;

        if (ObjectPool[nameofobj].Peek().gameObject.activeSelf)
            return null;
        o_block bl = ObjectPool[nameofobj].Dequeue();
        ObjectPool[nameofobj].Enqueue(bl);

        bl.worldpos = new Vector3(posInGrid.x * nodeLength, posInGrid.y * nodeLength, posInGrid.z * nodeLength);

        bl.gameObject.SetActive(true);
        blocks[posInGrid.x, posInGrid.y, posInGrid.z] = bl;
        return bl;
    }

    public Vector3Int GetObjectPositionToGrid(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / nodeLength);
        int y = Mathf.RoundToInt(position.y / nodeLength);
        int z = Mathf.RoundToInt(position.z / nodeLength);

        return new Vector3Int(x, y, z);
    }
    public Vector3Int GetObjectPositionToGrid(s_object obj)
    {
        Vector3 position = obj.worldpos;
        int x = Mathf.RoundToInt(position.x / nodeLength);
        int y = Mathf.RoundToInt(position.y / nodeLength);
        int z = Mathf.RoundToInt(position.z / nodeLength);

        return new Vector3Int(x, y, z);
    }

    public s_object GetObjectFromGrid(Vector3 position  //TODO: PUT IN LAYERS
        )
    {
        Vector3Int pos = GetObjectPositionToGrid(position);

        if (pos.x > worldsize.x - 1 || pos.x < 0 ||
            pos.y > worldsize.y - 1 || pos.y < 0 ||
            pos.z > worldsize.z - 1 || pos.z < 0)
            return null;

        return blocks[pos.x, pos.y, pos.z];
    }
	
}
