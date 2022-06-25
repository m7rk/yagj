using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public class GramCollection
    {
        public int LT = 0;
        public int MT = 0;
        public int ST = 0;
        public int S = 0;
        public int P = 0;

        public void incr(GramType gramType)
        {
            FindObjectOfType<UIManager>().setIncrFlash(gramType);
            switch (gramType)
            {
                case GameState.GramType.LT:
                    LT += 1; break;
                case GameState.GramType.MT:
                    MT += 1; break;
                case GameState.GramType.ST:
                    ST += 1; break;
                case GameState.GramType.P:
                    P += 1; break;
                case GameState.GramType.S:
                    S += 1; break;
            }
        }
    }

    public enum GramType { LT, MT, ST, S, P}


    public GameObject beaver;
    public GameObject player;
    GameObject golem;


    public GridManager gm;
    public TreeManager tm;
    public GameObject NPCParent;

    public GramCollection p1 = new GramCollection();
    public GramCollection p2 = new GramCollection();

    
    // Start is called before the first frame update
    void Start()
    {
        // Eventually we want to fetch this from a c# class
        // for now..
        var seed = Random.Range(0f, 100f);
        var world = new int[100, 50];
        for (var x = 0; x != world.GetLength(0); ++x)
        {
            for (var y = 0; y != world.GetLength(1); ++y)
            {
                var basePerlin = Mathf.Max(0.1f,Mathf.PerlinNoise((seed+x) / 5f, (seed+y) / 5f));
                var yFalloff = Mathf.Abs(y - (world.GetLength(1) / 2)) / (0.5*(float)world.GetLength(1));
                var xFalloff = Mathf.Abs(x - (world.GetLength(0) / 2)) / (0.5*(float)world.GetLength(0));
                world[x, y] = (basePerlin + -xFalloff + -yFalloff) > 0 ? 1 : 0;
                if (world[x, y] == 1)
                {
                    if ((x + (5*y)) % 9 == 0)
                    {
                        world[x, y] = 2;
                    }
                }
            }
        }

        gm.Create(world);
        for (var x = 0; x != world.GetLength(0); ++x)
        {
            for (var y = 0; y != world.GetLength(1); ++y)
            {
                if (world[x, y] == 2)
                {
                    tm.plantTree(x, y);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // construction
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tilemapPos = gm.terrWalkable.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
        }
    }

    public void spawnBeaver()
    {
        var v = Instantiate(beaver);
        v.transform.SetParent(NPCParent.transform);
        v.transform.position = player.transform.position + new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
    }

    public void spawnGolem()
    {
        var v = Instantiate(golem);
        v.transform.SetParent(NPCParent.transform);
        v.transform.position = this.transform.position;
    }
}
