using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableManager : MonoBehaviour
{
    public GameObject tree;
    public GameObject rocks;

    public Dictionary<Tuple<int, int>, GameObject> res = new Dictionary<Tuple<int, int>, GameObject>(); 

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
        t.transform.SetParent(this.transform);
        t.transform.localPosition = new Vector3(0.6f * x, 0.6f * y, 0);
        res[new Tuple<int, int>(x, y)] = t;
    }

    public void plantRocks(int x, int y)
    {
        var t = Instantiate(rocks);
        t.transform.SetParent(this.transform);
        t.transform.localPosition = new Vector3(0.6f * x, 0.6f * y, 0);
        res[new Tuple<int, int>(x, y)] = t;
    }

    public void cutDown(int x, int y)
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
        }
        

        Destroy(harvested.gameObject);
    }
}
