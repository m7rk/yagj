using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    GameState gs;

    public float PLAYER_PUSH_FORCE = 0.5f;
    public float WATER_PUSH_FORCE = 0.9f;

    public Vector3 last;


    enum TT { E, C };
    public void Start()
    {
        foreach (Transform child in transform)
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
        //Print2DArray(GameState.terrainAdapter);
        var astar = new AStarFunctions.AStar();
 
        // grid size is 0.6u
        var p = this.transform.localPosition;

        // grid size is 0.6u
        var pl = FindObjectOfType<PlayerController>().transform.localPosition;

        if((int)(p.x / 0.6f) == (int)(pl.x / 0.6) && (int)(p.y / 0.6) ==  (int)(pl.y / 0.6))
        {
            return;
        }

        int start_x = (int)(p.x / 0.6);
        int start_y = (int)(p.y / 0.6);

        int end_x = (int)(pl.x / 0.6);
        int end_y = (int)(pl.y / 0.6);



        //this.transform.position += new Vector3(path[1][0] - path[0][0], path[1][1] - path[0][1]).normalized * Time.deltaTime;

        if (GameState.naturalWorldState[start_x,start_y] == GameState.TerrainType.TREES)
        {
            // tree cut down
            GameState.hm.cutDown(start_x, start_y);
        }
        else
        {
            var to_tile = TileSearch.TileSearcher<GameState.TerrainType>.findTile(start_x, start_y, GameState.TerrainType.TREES, GameState.naturalWorldState);

            var path = astar.pathTo(start_x, start_y, to_tile[0], to_tile[1], GameState.getTerrainAdapter());

            // later -> go to middle of square.
            this.transform.position += (new Vector3(path[1][0] - start_x, path[1][1] - start_y).normalized * Time.deltaTime);
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

    }
}
