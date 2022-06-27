using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : MonoBehaviour
{
    GameState gs;

    public float GATHERER_PUSH_FORCE = 0.2f;
    public float PLAYER_PUSH_FORCE = 0.5f;
    public float WATER_PUSH_FORCE = 2f;

    public float animTimeout = 0f;
    public enum GTYPE { WOOD, MINE, RES }

    public GTYPE collectType;

    public void Start()
    {

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
                if (collectType == GTYPE.WOOD && GameState.hm.objAt(start_x, start_y) == "tree")
                {
                    GameState.hm.attack(start_x, start_y);
                }

                if (collectType == GTYPE.MINE && GameState.hm.objAt(start_x, start_y) == "rock")
                {
                    GameState.hm.attack(start_x, start_y);
                }
            }

            animTimeout -= Time.deltaTime;

            return;
        } 
        //Print2DArray(GameState.terrainAdapter);
        // var astar = new AStarFunctions.AStar();


        // grid size is 0.6u
        var pl = FindObjectOfType<PlayerController>().transform.localPosition;

        if((int)(p.x / 0.6f) == (int)(pl.x / 0.6) && (int)(p.y / 0.6) ==  (int)(pl.y / 0.6))
        {
            return;
        }


        var tres = (collectType == GTYPE.WOOD ? GameState.TerrainType.TREES : GameState.TerrainType.ROCKS);

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
            var move = CrapBFS.find(start_x, start_y, collectType == GTYPE.WOOD ? 1 : 2, ta);

            var posDelt = new Vector3(move.Item1,move.Item2,0).normalized;
            this.transform.position += posDelt * Time.deltaTime;
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(posDelt.x > 0 ? 1f : -1f,1f,1f);
        }

    }
    
    public void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            transform.position -= (other.transform.position - this.transform.position).normalized * Time.deltaTime * PLAYER_PUSH_FORCE;
        }

        if (other.tag == "Water")
        {
            var colpt = other.ClosestPoint(this.transform.position);
            transform.position -= (new Vector3(colpt.x,colpt.y,this.transform.position.z) - this.transform.position).normalized * Time.deltaTime * WATER_PUSH_FORCE;
        }

        if (other.tag == "Gatherer")
        {
            transform.position -= (other.transform.position - this.transform.position).normalized * Time.deltaTime * GATHERER_PUSH_FORCE;
        }
    }
}
