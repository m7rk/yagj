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
    }


    public GridManager gm;

    public GramCollection p1 = new GramCollection();
    public GramCollection p2 = new GramCollection();

    // Start is called before the first frame update
    void Start()
    {
        // Eventually we want to fetch this from a c# class
        // for now..
        var world = new bool[50, 15];
        for(var x = 0; x != world.GetLength(0); ++x)
        {
            for (var y = 0; y != world.GetLength(1); ++y)
            {
                world[x, y] = ((Mathf.PerlinNoise(x/10f, y/10f) - (Mathf.Abs(y-7)/8f)) > 0.1);
            }
        }

        gm.Create(world);
    }

    // Update is called once per frame
    void Update()
    {
        // construction
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tilemapPos = gm.terrWalkable.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log(tilemapPos);
        }
    }
}
