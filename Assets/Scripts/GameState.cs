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
    public GameObject turret;

    public GameObject pkLT;
    public GameObject pkMT;
    public GameObject pkST;
    public GameObject pkS;
    public GameObject pkP;


    // this is the rocks, water, trees, grass.
    public static TerrainType[,] naturalWorldState;

    // etc
    public static GridManager gm;
    public static HarvestableManager hm;

    public GameObject NPCParent;
    public GameObject BuildingParent;

    public GramCollection p1 = new GramCollection();
    public GramCollection p2 = new GramCollection();


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GridManager>();
        hm = FindObjectOfType<HarvestableManager>();
        var seed = Random.Range(0f, 100f);
        naturalWorldState = new TerrainType[200, 150];

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

                    if ((x + (5 * y)) % 9 == 0)
                    {
                        naturalWorldState[x, y] = TerrainType.TREES;
                    }
                    if ((x + (5 * y)) % 15 == 0)
                    {
                        naturalWorldState[x, y] = TerrainType.ROCKS;
                    }
                } else
                {

                    naturalWorldState[x, y] = TerrainType.WATER;
                }
            }
        }

        gm.Create(naturalWorldState);
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

    public void spawnTurret()
    {
        FindObjectOfType<PlayerController>().createConstPrefab(turret);
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
