using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public GameObject tree;
    public GameObject rocks;


    // map coord to actual GO (use name to keep track of what is.
    public Dictionary<Tuple<int, int>, GameObject> res = new Dictionary<Tuple<int, int>, GameObject>();


    public GameObject turret;
    public GameObject shed;
    public GameObject pFactory;
    public GameObject rbase;

    private bool gameEndFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void paint(GameObject obj, int teamID)
    {
        Color col = Color.white;
        switch(teamID)
        {
            case 1: col = Color.red; break;
            case 2: col = Color.blue; break;
            case 3: col = Color.green; break;
            case 4: col = Color.yellow; break;
        }

        foreach (Transform child in obj.transform)
        {
            if (child.GetComponent<SpriteRenderer>())
            {
                if (child.name[child.name.Length - 1] == 'C')
                {
                    child.GetComponent<SpriteRenderer>().color = col;
                }
            }

            foreach (Transform child2 in child)
            {
                // color if name ends with cap C
                if (child2.name[child2.name.Length - 1] == 'C')
                {
                    child2.GetComponent<SpriteRenderer>().color = col;
                }
            }
        }
    }

    public void plantTree(int x, int y)
    {
        var t = Instantiate(tree);
        put(t, x, y);
        t.name = "tree";
    }

    public void plantRocks(int x, int y)
    {
        var t = Instantiate(rocks);
        put(t, x, y);
        t.name = "rock";
    }

    private void put(GameObject t, int x, int y)
    {
        t.transform.SetParent(this.transform);
        t.transform.localPosition = new Vector3(0.6f * x, 0.6f * y, 0);
        res[new Tuple<int, int>(x, y)] = t;
    }
    // puts.
    public void putBase(int x, int y, int TID)
    {
        var t = Instantiate(rbase);
        put(t, x, y);
        paint(t, TID);
        // base is only big turret (this is okay!!)
        res[new Tuple<int, int>(x+1, y)] = t;
        res[new Tuple<int, int>(x, y+1)] = t;
        res[new Tuple<int, int>(x+1, y+1)] = t;

    }
    public void putShed(int x, int y, int TID)
    {
        var t = Instantiate(shed);
        put(t, x, y);
        paint(t, TID);
        GameState.gs.worldState[x, y] = GameState.TerrainType.STRUCTURE;
    }

    public void putTurret(int x, int y, int TID)
    {
        var t = Instantiate(turret);
        put(t, x, y);
        paint(t, TID);
        GameState.gs.worldState[x, y] = GameState.TerrainType.STRUCTURE;
    }
    public void putPFact(int x, int y, int TID)
    {
        var t = Instantiate(pFactory);
        put(t, x, y);
        paint(t, TID);
        GameState.gs.worldState[x, y] = GameState.TerrainType.STRUCTURE;
    }

    public string objAt(int x, int y)
    {
        if (!res.ContainsKey(new Tuple<int, int>(x, y)))
        {
            return "";
        }
        return res[new Tuple<int, int>(x, y)].name;
    }

    public void attack(int x, int y)
    {
        if(gameEndFlag)
        {
            return;
        }

        // someone else cut this already.
        if(!res.ContainsKey(new Tuple<int, int>(x, y)))
        {
            return;
        }

        // harvest this! it is a tree!
        if (res[new Tuple<int, int>(x, y)].name == "tree")
        {
            if (Vector2.Distance(res[new Tuple<int, int>(x, y)].transform.position, FindObjectOfType<Player>().transform.position) < 10)
            {
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("wood"), res[new Tuple<int, int>(x, y)].transform.position);
            }
        }

        if (res[new Tuple<int, int>(x, y)].name == "rock")
        {
            if (Vector2.Distance(res[new Tuple<int, int>(x, y)].transform.position, FindObjectOfType<Player>().transform.position) < 10)
            {
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("mine"), res[new Tuple<int, int>(x, y)].transform.position);
            }
        }

        // for each child
        for (int i = 0; i != res[new Tuple<int, int>(x, y)].transform.childCount; ++i)
        {
            var harvested = res[new Tuple<int, int>(x, y)].transform.GetChild(i);
            var sname = harvested.GetComponent<SpriteRenderer>().sprite.name;

            // if it's at max damage.
            if (sname.Contains("3"))
            {
                // harvest this! it is a tree!
                if (res[new Tuple<int, int>(x, y)].name == "tree" || res[new Tuple<int, int>(x, y)].name == "rock")
                {

                    FindObjectOfType<GameState>().addPickupable(harvested.name.Replace("C", ""), new Vector2(x * 0.6f + UnityEngine.Random.Range(0.2f, 0.4f), y * 0.6f + UnityEngine.Random.Range(0.2f, 0.4f)));
                    Destroy(harvested.gameObject);
                    return;
                } else
                {
                    // it's injured, just continue;
                    continue;
                }
            } 
            
            else
            {
                // injure it.
                if(sname.Contains("2"))
                {
                    harvested.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Broke/" + sname.Replace("2", "3"));
                } else if (sname.Contains("1"))
                {
                    harvested.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Broke/" + sname.Replace("1", "2"));
                } else
                {
                    harvested.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Broke/" + sname + "1");
                }
                return;
            }
        }

        // ledas mod, drop resources, no time for this now.

        // item exhaused 
        Destroy(res[new Tuple<int, int>(x, y)].gameObject);


        if(res[new Tuple<int, int>(x, y)].name == "Bbase")
        {
            // red victory
            // end game in 10 s.
            gameEndFlag = true;
            FindObjectOfType<UIManager>().redVictory();
        }

        if (res[new Tuple<int, int>(x, y)].name == "Rbase")
        {
            // red victory
            // end game in 10 s.
            FindObjectOfType<UIManager>().blueVictory();
            gameEndFlag = true;
        }

        res.Remove(new Tuple<int, int>(x, y));
        GameState.worldState[x, y] = GameState.TerrainType.GRASS;

        

    }
}
