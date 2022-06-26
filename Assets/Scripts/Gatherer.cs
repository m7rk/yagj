using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    GameState gs;

    public float GATHERER_PUSH_FORCE = 0.5f;
    public float PLAYER_PUSH_FORCE = 0.5f;
    public float WATER_PUSH_FORCE = 0.9f;

    public float animTimeout = 0f;
    public Vector3 last;

    public bool collectTrees;

    public void Start()
    {
        foreach (Transform child in transform.GetComponentInChildren<Animator>().transform)
        {
            if (child.GetComponent<SpriteRenderer>())
            {
                child.GetComponent<SpriteRenderer>().color = Color.blue;
            }

            foreach (Transform child2 in child)
            {
                // color if name ends with cap C
                if (child2.name[child2.name.Length - 1] == 'C')
                {
                    child2.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
        }


    }

    // ask gamestate where nearest resource is.
    public void Update()
    {

        // grid size is 0.6u
        var p = this.transform.localPosition;

        int start_x = (int)(p.x / 0.6);
        int start_y = (int)(p.y / 0.6);

        if (animTimeout > 0)
        {
            // check for a good harvest.
            if (animTimeout - Time.deltaTime <= 0)
            {
                GameState.hm.cutDown(start_x, start_y);
            }
            animTimeout -= Time.deltaTime;
            return;
        } 
        //Print2DArray(GameState.terrainAdapter);
        var astar = new AStarFunctions.AStar();


        // grid size is 0.6u
        var pl = FindObjectOfType<PlayerController>().transform.localPosition;

        if((int)(p.x / 0.6f) == (int)(pl.x / 0.6) && (int)(p.y / 0.6) ==  (int)(pl.y / 0.6))
        {
            return;
        }


        var tres = (collectTrees ? GameState.TerrainType.TREES : GameState.TerrainType.ROCKS);

        //this.transform.position += new Vector3(path[1][0] - path[0][0], path[1][1] - path[0][1]).normalized * Time.deltaTime;

        if (GameState.naturalWorldState[start_x,start_y] == tres)
        {
            // tree cut down
            GetComponentInChildren<Animator>().SetTrigger("Gather");
            animTimeout = 2.5f;
        }
        else
        {
            /**
             * Gale's approach is going to run faster, but i need a bandaid to test with
            var to_tile = TileSearch.TileSearcher<GameState.TerrainType>.findTile(start_x, start_y, tres, GameState.naturalWorldState);

            ta[to_tile[0],to_tile[1]] = 1;
            //var path = astar.pathTo(start_x, start_y, to_tile[0], to_tile[1], ta);
            */

            var ta = GameState.getTerrainAdapter();
            var move = CrapBFS.find(start_x, start_y, (collectTrees ? 1 : 2), ta);

            var posDelt = new Vector3(move.Item1,move.Item2,0).normalized;
            this.transform.position += posDelt * Time.deltaTime;
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(posDelt.x > 0 ? 1f : -1f,1f,1f);
        }

    }
    
    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            last = this.transform.position;
            transform.position -= (other.transform.position - this.transform.position).normalized * Time.deltaTime * PLAYER_PUSH_FORCE;
        }

        if (other.tag == "Water")
        {
            transform.position = last;
        }

        if (other.tag == "Gatherer")
        {
            transform.position -= (other.transform.position - this.transform.position).normalized * Time.deltaTime * GATHERER_PUSH_FORCE;
        }
    }
}
