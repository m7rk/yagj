using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// don't use this.
/**
public class Antag : MonoBehaviour
{
    public Sprite redAlt;

    public char team;

    public bool findRock;

    public float waitTime;
    
    public Tuple<int,int> dest;

    public void changeToRed()
    {
        transform.Find("starm").GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f);
        transform.Find("r").GetComponent<SpriteRenderer>().sprite = redAlt;
    }

    private int[] findCoordNearby(int start_x, int start_y)
    {
        var opts = new List<int[]>();
        for (int dx = -4; dx != 5; dx++)
        {
            for (int dy = -4; dy != 5; dy++)
            {
                if(GameState.gs[start_x+dx,start_y+dy] == GameState.TerrainType.GRASS)
                {
                    opts.Add(new int[] { start_x + dx, start_y + dy });
                }
            }
        }

        if (opts.Count > 0)
        {
            var random = new System.Random().Next(opts.Count);
            return opts[random];
        }
        return null;
    }

    private void OnEnable()
    {
        // cheat like a fuckin bastard
        FindObjectOfType<GameState>().spawnBeaver(this.gameObject, team);
        FindObjectOfType<GameState>().spawnCaterpillar(this.gameObject, team);
        FindObjectOfType<GameState>().spawnGolem(this.gameObject, team);
        var v = findCoordNearby((int)(this.transform.position.x / 0.6f), (int)(this.transform.position.y / 0.6));
        if(v != null)
        {
            FindObjectOfType<StructureManager>().putShed(v[0], v[1], team);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            GetComponent<Animator>().SetBool("Walking", false);
            return;
        }

        dest = CrapBFS.find((int)(this.transform.position.x / 0.6f), (int)(this.transform.position.y / 0.6), findRock ? 2 : 1, GameState.getTerrainAdapter(team));

        if (dest.Item1 == 0 && dest.Item2 == 0)
        {
            waitTime = UnityEngine.Random.Range(8f, 16f);
            findRock = !findRock;
            // 5 spawnables, pick one, try to build if we have the res.
            int rand = ((int)UnityEngine.Random.Range(0,10));

            // continue to cheat like a fuckin bastard
            var v = findCoordNearby((int)(this.transform.position.x / 0.6f), (int)(this.transform.position.y / 0.6));


            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(v[0] > 0f ? 0.2f : -0.2f, 0.2f, 0.2f);
            switch (rand)
            {
                case 0:
                case 6:
                    FindObjectOfType<GameState>().spawnBeaver(this.gameObject, team);
                    break;
                case 1:
                case 7:
                    FindObjectOfType<GameState>().spawnGolem(this.gameObject, team);
                    break;
                case 2:
                    FindObjectOfType<GameState>().spawnCaterpillar(this.gameObject, team);
                    break;
                case 3:
                    if (v != null)
                    {
                        FindObjectOfType<StructureManager>().putShed(v[0], v[1], team);
                    }break;
                case 4:
                    if (v != null)
                    {
                        FindObjectOfType<StructureManager>().putPFact(v[0], v[1], team);
                    }
                    break;
                case 5:
                    if (v != null)
                    {
                        FindObjectOfType<StructureManager>().putTurret(v[0], v[1], team);
                    }
                    break;
            }


            /**
            int[] numberOfPieces = new int[] { 2, 2, 0, 0, 0 };
            int[] numberOfBuilds = new int[] { 0, 0, 0, 0, 0, 1, 0, 0 };
            List<int> osg = (new CostAnalysis.TurrentCostAnalysis()).Ordering_ExpensiveGreedy(numberOfPieces, numberOfBuilds);
   
            
        }



        this.transform.position += new Vector3(dest.Item1, dest.Item2, 0).normalized * Time.deltaTime;

        GetComponent<Animator>().SetBool("Walking",true);

    }
}
*/