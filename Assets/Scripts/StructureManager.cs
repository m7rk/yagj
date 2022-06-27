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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void putBase(int x, int y, char TID)
    {
        var t = Instantiate(rbase);
        put(t, x, y);
        t.name = TID + "base";
    }
    public void putShed(int x, int y, char TID)
    {
        var t = Instantiate(shed);
        put(t, x, y);
        t.name = TID + "shed";
    }

    public void putTurret(int x, int y, char TID)
    {
        var t = Instantiate(turret);
        put(t, x, y);
        t.name = TID + "tur";
    }
    public void putPFact(int x, int y, char TID)
    {
        var t = Instantiate(pFactory);
        put(t, x, y);
        t.name = TID + "pfct";
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
        // someone else cut this already.
        if(!res.ContainsKey(new Tuple<int, int>(x, y)))
        {
            return;
        }

        var harvested = res[new Tuple<int, int>(x, y)].transform.GetChild(0);

        // fuck
        FindObjectOfType<GameState>().addPickupable(harvested.name, new Vector2(x * 0.6f + UnityEngine.Random.Range(0.2f, 0.4f), y * 0.6f + UnityEngine.Random.Range(0.2f,0.4f)));
   
        if(res[new Tuple<int, int>(x, y)].transform.childCount == 1)
        {
            // res exhaused 
            Destroy(res[new Tuple<int, int>(x, y)].gameObject);
            res.Remove(new Tuple<int, int>(x, y));
            GameState.naturalWorldState[x, y] = GameState.TerrainType.GRASS;

            if(name == "Bbase")
            {
                // red victory
                // end game in 10 s.
            }

            if (name == "Rbase")
            {
                // red victory
                // end game in 10 s.
            }

        }
        

        Destroy(harvested.gameObject);
    }
}
