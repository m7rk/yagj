using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public static int MISSION_SEL;
    public class GramCollection
    {
        public int LT = 0;
        public int MT = 0;
        public int ST = 2;
        public int S = 2;
        public int P = 0;

        public void incr(bool player,GramType gramType)
        {
            if (player)
            {
                FindObjectOfType<UIManager>().setIncrFlash(gramType);
            }

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

        public void incr(bool player,string gramType)
        {
            switch (gramType)
            {
                case "lt":
                    incr(player,GramType.LT); break;
                case "mt":
                    incr(player, GramType.MT); break;
                case "st":
                    incr(player, GramType.ST); break;
                case "p":
                    incr(player, GramType.P); break;
                case "s":
                    incr(player, GramType.S); break;
            }
        }
    }

    public enum GramType { LT, MT, ST, S, P }

    // This seems super terrible. "Structure" gives no data
    // for now though it's a quick "pass or impass" filter
    public enum TerrainType { WATER, GRASS, ROCKS, TREES, STRUCTURE }



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
    public static TerrainType[,] worldState;

    // etc
    public static GridManager gm;
    public static StructureManager hm;

    public GameObject NPCParent;
    public GameObject BuildingParent;

    public GramCollection p1 = new GramCollection();
    public GramCollection p2 = new GramCollection();

    private int[] redSpawn = null;
    private int[] blueSpawn = null;

    // World Generation
    void Start()
    {
        gm = FindObjectOfType<GridManager>();
        hm = FindObjectOfType<StructureManager>();

        int DIMX = 100;
        int DIMY = 75;

        var seed = UnityEngine.Random.Range(0f, 100f);
        worldState = new TerrainType[DIMX, DIMY];

        // first pass, generate the basic rough outline of the world.
        for (var x = 0; x != worldState.GetLength(0); ++x)
        {
            for (var y = 0; y != worldState.GetLength(1); ++y)
            {
                var basePerlin = Mathf.Max(0.4f, Mathf.PerlinNoise((seed + x) / 5f, (seed + y) / 5f));
                var yFalloff = 2f * (Mathf.Abs(y - (worldState.GetLength(1) / 2)) / (float)worldState.GetLength(1));
                var xFalloff = 2f * (Mathf.Abs(x - (worldState.GetLength(0) / 2)) / (float)worldState.GetLength(0));

                bool land = (basePerlin + -xFalloff + -yFalloff) > 0;

                if (land)
                {
                    worldState[x, y] = TerrainType.GRASS;

                    // high freq noise for rocks, trees.
                    var resPerlin = Mathf.PerlinNoise(((seed*2) + x) / 4f, ((seed*4) + y) / 4f);

                    if (resPerlin < 0.25f)
                    {
                        worldState[x, y] = TerrainType.TREES;
                    }

                    if (resPerlin > 0.75f)
                    {
                        worldState[x, y] = TerrainType.ROCKS;
                    }
                } else
                {

                    worldState[x, y] = TerrainType.WATER;
                }
            }
        }

        // find only mainland tiles (no random islands)
        HashSet<Tuple<int, int>> found = new HashSet<Tuple<int, int>>();
        Queue<Tuple<int, int>> mainLand = new Queue<Tuple<int, int>>();
        mainLand.Enqueue(new Tuple<int, int>(DIMX / 2, DIMY / 2));
        while(mainLand.Count > 0)
        {
            var v = mainLand.Dequeue();

            var adj = CrapBFS.fadj();
            foreach(var a in adj)
            {
                var cons = new Tuple<int, int>(a.Item1 + v.Item1, a.Item2 + v.Item2);

                if (!found.Contains(cons))
                {
                    if(worldState[cons.Item1,cons.Item2] != TerrainType.WATER)
                    {
                        found.Add(cons);
                        mainLand.Enqueue(cons);

                    }
                }
            }
        }

        // remove everything not on the mainland.
        for (var x = 0; x != worldState.GetLength(0); ++x)
        {
            for (var y = 0; y != worldState.GetLength(1); ++y)
            {
                if (!found.Contains(new Tuple<int, int>(x, y)))
                {
                    worldState[x, y] = TerrainType.WATER;
                }
                else
                {
                    if (worldState[x, y] == TerrainType.GRASS && UnityEngine.Random.Range(0.0f, 1f) < 0.05f)
                    {
                        // place a med tri if it is grass.
                        addPickupable("mt", new Vector2(x * 0.6f + UnityEngine.Random.Range(0.2f, 0.4f), y * 0.6f + UnityEngine.Random.Range(0.2f, 0.4f)));
                    }
                }
            }
        }

        // Now place bases on the map.

        for (var x = 0; x != worldState.GetLength(0); ++x)
        {
            for (var y = 0; y != worldState.GetLength(1); ++y)
            {
                if(redSpawn == null && worldState[x,y] != TerrainType.WATER)
                {
                    // candidate
                    if(worldState[x+1,y] != TerrainType.WATER && worldState[x+1,y+1] != TerrainType.WATER && worldState[x,y+1] == TerrainType.WATER)
                    {
                        redSpawn = new int[2] { x, y };
                    }
                }

                if (worldState[x, y] != TerrainType.WATER)
                {
                    // candidate
                    if (worldState[x + 1, y] != TerrainType.WATER && worldState[x + 1, y + 1] != TerrainType.WATER && worldState[x, y + 1] == TerrainType.WATER)
                    {
                        blueSpawn = new int[2] { x, y };
                    }
                }
            }
        }

        worldState[redSpawn[0],redSpawn[1]] = TerrainType.GRASS;
        worldState[redSpawn[0]+1, redSpawn[1]] = TerrainType.GRASS;
        worldState[redSpawn[0], redSpawn[1]+1] = TerrainType.GRASS;
        worldState[redSpawn[0]+1, redSpawn[1]+1] = TerrainType.GRASS;

        worldState[blueSpawn[0], blueSpawn[1]] = TerrainType.GRASS;
        worldState[blueSpawn[0] + 1, blueSpawn[1]] = TerrainType.GRASS;
        worldState[blueSpawn[0], blueSpawn[1] + 1] = TerrainType.GRASS;
        worldState[blueSpawn[0] + 1, blueSpawn[1] + 1] = TerrainType.GRASS;

        gm.Create(worldState);

        // there's space (for a base)
        for (var x = 0; x != worldState.GetLength(0); ++x)
        {
            for (var y = 0; y != worldState.GetLength(1); ++y)
            {
                if (worldState[x, y] == TerrainType.TREES)
                {
                    hm.plantTree(x, y);
                }
                if (worldState[x, y] == TerrainType.ROCKS)
                {
                    hm.plantRocks(x, y);
                }
            }
        }

        FindObjectOfType<StructureManager>().putBase(redSpawn[0], redSpawn[1], 'R');
        FindObjectOfType<StructureManager>().putBase(blueSpawn[0], blueSpawn[1], 'B');

    }

    public void putAtBase(GameObject p, bool red)
    {
        if (red)
        {
            p.transform.localPosition = new Vector3(0.3f + redSpawn[0] * 0.6f, 0.3f + redSpawn[1] * 0.6f, this.transform.localPosition.z);
        } else
        {
            p.transform.localPosition = new Vector3(0.3f + blueSpawn[0] * 0.6f, 0.3f + blueSpawn[1] * 0.6f, this.transform.localPosition.z);

        }
    }

    static public int[,] getCatPickUpAdapter(char team)
    {
        var terrainAdapter = new int[200, 150];
        for (var x = 0; x != worldState.GetLength(0); ++x)
        {
            for (var y = 0; y != worldState.GetLength(1); ++y)
            {
                int val = 0;
                switch (worldState[x, y])
                {
                    case TerrainType.GRASS:
                        val = 0;
                        break;
                    case TerrainType.TREES:
                    case TerrainType.ROCKS:
                    case TerrainType.STRUCTURE:
                        val = 1;
                        break;
                    case TerrainType.WATER:
                        val = -1; break;
                }
                terrainAdapter[x, y] = val;
            }
        }

        foreach(var v in FindObjectsOfType<Pickupable>())
        {
            terrainAdapter[(int)(v.transform.position.x / 0.6f), (int)(v.transform.position.y / 0.6f)] = 2;
        }

        return terrainAdapter;
    }

    static public int[,] getTerrainAdapter(char team)
    {
        var terrainAdapter = new int[200, 150];
        for (var x = 0; x != worldState.GetLength(0); ++x)
        {
            for (var y = 0; y != worldState.GetLength(1); ++y)
            {
                int val = 0;
                switch (worldState[x, y])
                {
                    case TerrainType.GRASS:
                        val = 0; break;
                    case TerrainType.TREES:
                        val = 1; break;
                    case TerrainType.ROCKS:
                        val = 2; break;
                    case TerrainType.STRUCTURE:
                        // 3 for friendly, 4 for shed, 5 for enemy.
                        if (hm.objAt(x, y)[0] == team)
                        {
                            if (hm.objAt(x, y) == (team + "shed"))
                            {
                                val = 3;
                            }
                            else
                            {
                                val = 4;
                            }
                        } else
                        {
                            val = 5;
                        }
                        break;
                    case TerrainType.WATER:
                        val = -1; break;
                }
                terrainAdapter[x, y] = val;
            }
        }
        return terrainAdapter;
    }

    public Vector3 randOffset()
    {
        return new Vector3(UnityEngine.Random.Range(-0.01f, 0.01f), UnityEngine.Random.Range(-0.01f, 0.01f),0);
    }

    public void spawnBeaver(GameObject player, char team)
    {
        var v = Instantiate(beaver);
        StructureManager.paint(v.GetComponentInChildren<Animator>().gameObject, (team == 'R' ? Color.red : Color.blue));
        v.transform.SetParent(NPCParent.transform);
        v.transform.position = player.transform.position + randOffset();
        v.name = (team.ToString() + "beaver");
    }

    public void spawnGolem(GameObject player, char team)
    {
        var v = Instantiate(golem);
        StructureManager.paint(v, (team == 'R' ? Color.red : Color.blue));
        v.transform.SetParent(NPCParent.transform);
        v.transform.position = player.transform.position + randOffset();
        v.name = (team.ToString() + "golem");
    }

    public void spawnCaterpillar(GameObject player, char team)
    {
        var v = Instantiate(caterpillar);
        StructureManager.paint(v, (team == 'R' ? Color.red : Color.blue));
        v.transform.SetParent(NPCParent.transform);
        v.transform.position = player.transform.position + randOffset();
        v.name = (team.ToString() + "caterpillar");
    }

    public void spawnTurret()
    {
        FindObjectOfType<PlayerController>().GetComponent<PlayerController>().createConstPrefab(turret);
    }

    public void spawnShed()
    {
        FindObjectOfType<PlayerController>().createConstPrefab(shed);
    }

    public void spawnPFactory()
    {
        FindObjectOfType<PlayerController>().createConstPrefab(pFactory);
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
