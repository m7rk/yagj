using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public class GramCollection
    {
        public int LT = 5;
        public int MT = 5;
        public int ST = 5;
        public int S = 5;
        public int P = 5;

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

        public void decr(string gramType)
        {
            switch (gramType)
            {
                case "lt":
                    LT -= 1; break;
                case "mt":
                    MT -= 1; break;
                case "st":
                    ST -= 1; break;
                case "p":
                    P -= 1; break;
                case "s":
                    S -= 1; break;
            }
        }

        public void incr(string gramType)
        {
            switch (gramType)
            {
                case "lt":
                    incr(GramType.LT); break;
                case "mt":
                    incr(GramType.MT); break;
                case "st":
                    incr(GramType.ST); break;
                case "p":
                    incr(GramType.P); break;
                case "s":
                    incr(GramType.S); break;
            }
        }
    }

    public enum GramType { LT, MT, ST, S, P }
    public enum TerrainType { WATER, GRASS, ROCKS, TREES }

    // reference to player
    public GameObject player;

    public GameObject pickupParent;

    // instantiateables
    public GameObject beaver;
    public GameObject golem;
    public GameObject caterpillar;

    public GameObject turret;
    public GameObject shed;
    public GameObject pFactory;


    public GameObject pkLT;
    public GameObject pkMT;
    public GameObject pkST;
    public GameObject pkS;
    public GameObject pkP;


    // this is the rocks, water, trees, grass.
    public static TerrainType[,] naturalWorldState;

    // etc
    public static GridManager gm;
    public static StructureManager hm;

    public GameObject NPCParent;
    public GameObject BuildingParent;

    public GramCollection p1 = new GramCollection();
    public GramCollection p2 = new GramCollection();


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GridManager>();
        hm = FindObjectOfType<StructureManager>();
        var seed = Random.Range(0f, 100f);
        naturalWorldState = new TerrainType[200, 150];

        // first pass, generate the basic outline of the world.
        for (var x = 0; x != naturalWorldState.GetLength(0); ++x)
        {
            for (var y = 0; y != naturalWorldState.GetLength(1); ++y)
            {
                var basePerlin = Mathf.Max(0.2f, Mathf.PerlinNoise((seed + x) / 5f, (seed + y) / 5f));
                var yFalloff = Mathf.Abs(y - (naturalWorldState.GetLength(1) / 2)) / (0.5 * (float)naturalWorldState.GetLength(1));
                var xFalloff = Mathf.Abs(x - (naturalWorldState.GetLength(0) / 2)) / (0.5 * (float)naturalWorldState.GetLength(0));
                bool land = (basePerlin + -xFalloff + -yFalloff) > 0;

                if (land)
                {
                    naturalWorldState[x, y] = TerrainType.GRASS;

                    // high freq noise for rocks, trees.
                    var resPerlin = Mathf.PerlinNoise(((seed*2) + x) / 2f, ((seed*4) + y) / 2f);

                    if (resPerlin < 0.2f)
                    {
                        naturalWorldState[x, y] = TerrainType.TREES;
                    }

                    if (resPerlin > 0.8f)
                    {
                        naturalWorldState[x, y] = TerrainType.ROCKS;
                    }
                } else
                {

                    naturalWorldState[x, y] = TerrainType.WATER;
                }
            }
        }

        // secondly, we need to place the bases on opposide sides of map, so the player can choose. 
        int[] first = null;
        int[] second = null;
        for (var x = 0; x != naturalWorldState.GetLength(0); ++x)
        {
            for (var y = 0; y != naturalWorldState.GetLength(1); ++y)
            {
                if(first == null && naturalWorldState[x,y] != TerrainType.WATER)
                {
                    // candidate
                    if(naturalWorldState[x+1,y] != TerrainType.WATER && naturalWorldState[x+1,y+1] != TerrainType.WATER && naturalWorldState[x,y+1] == TerrainType.WATER)
                    {
                        first = new int[2] { x, y };
                    }
                }

                if (naturalWorldState[x, y] != TerrainType.WATER)
                {
                    // candidate
                    if (naturalWorldState[x + 1, y] != TerrainType.WATER && naturalWorldState[x + 1, y + 1] != TerrainType.WATER && naturalWorldState[x, y + 1] == TerrainType.WATER)
                    {
                        second = new int[2] { x, y };
                    }
                }
            }
        }

        naturalWorldState[first[0],first[1]] = TerrainType.GRASS;
        naturalWorldState[first[0]+1, first[1]] = TerrainType.GRASS;
        naturalWorldState[first[0], first[1]+1] = TerrainType.GRASS;
        naturalWorldState[first[0]+1, first[1]+1] = TerrainType.GRASS;

        naturalWorldState[second[0], second[1]] = TerrainType.GRASS;
        naturalWorldState[second[0] + 1, second[1]] = TerrainType.GRASS;
        naturalWorldState[second[0], second[1] + 1] = TerrainType.GRASS;
        naturalWorldState[second[0] + 1, second[1] + 1] = TerrainType.GRASS;

        gm.Create(naturalWorldState);

        // there's space (for a base)
        for (var x = 0; x != naturalWorldState.GetLength(0); ++x)
        {
            for (var y = 0; y != naturalWorldState.GetLength(1); ++y)
            {
                if (naturalWorldState[x, y] == TerrainType.TREES)
                {
                    hm.plantTree(x, y);
                }
                if (naturalWorldState[x, y] == TerrainType.ROCKS)
                {
                    hm.plantRocks(x, y);
                }
            }
        }

        FindObjectOfType<StructureManager>().putBase(first[0], first[1], 'R');
        FindObjectOfType<StructureManager>().putBase(second[0], second[1], 'B');
    }

    static public int[,] getTerrainAdapter()
    {
        var terrainAdapter = new int[200, 150];
        for (var x = 0; x != naturalWorldState.GetLength(0); ++x)
        {
            for (var y = 0; y != naturalWorldState.GetLength(1); ++y)
            {
                int val = 0;
                switch (naturalWorldState[x, y])
                {
                    case TerrainType.GRASS:
                        val = 0; break;
                    case TerrainType.TREES:
                        val = 1; break;
                    case TerrainType.ROCKS:
                        val = 2; break;
                    case TerrainType.WATER:
                        val = -1; break;
                }
                terrainAdapter[x, y] = val;
            }
        }
        return terrainAdapter;
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
        v.transform.position = player.transform.position + new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
    }

    public void spawnCaterpillar()
    {
        var v = Instantiate(caterpillar);
        v.transform.SetParent(NPCParent.transform);
        v.transform.position = player.transform.position + new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0);
    }

    public void spawnTurret()
    {
        player.GetComponent<PlayerController>().createConstPrefab(turret);
    }

    public void spawnShed()
    {
        player.GetComponent<PlayerController>().createConstPrefab(shed);
    }

    public void spawnPFactory()
    {
        player.GetComponent<PlayerController>().createConstPrefab(pFactory);
    }


    public void addPickupable(string gm, Vector2 pos)
    {
        GameObject go = null;
        switch(gm)
        {
            case "lt":
                go = pkLT; break;
            case "mt":
                go = pkMT; break;
            case "st":
                go = pkST; break;
            case "s":
                go = pkS; break;
            case "p":
                go = pkP; break;
        }

        var v = Instantiate(go);
        v.GetComponent<Pickupable>().gs = this;
        v.transform.SetParent(pickupParent.transform);
        v.transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }
}
