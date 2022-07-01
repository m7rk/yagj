using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critter : Entity
{

    public float GATHERER_PUSH_FORCE = 0.2f;
    public float PLAYER_PUSH_FORCE = 0.5f;
    public float WATER_PUSH_FORCE = 2f;

    public float animTimeout = 0f;
    public enum GTYPE { BEAV, GOL, CAT, RAT }

    public GTYPE collectType;


    private float CAT_SCALE = 0.9f;

    public GameObject collectedItem;
    public Player.GramType pickedUp;

    public void Start()
    {
        collectedItem = null;
    }

    // ask gamestate where nearest resource is.
    public void Update()
    {
        if (collectType == GTYPE.BEAV || collectType == GTYPE.GOL || collectType == GTYPE.RAT)
        {
            gather();
        }
        if(collectType == GTYPE.CAT)
        {
            collect();
        }
    }

    public void tryCollect()
    {
        GameObject best = null;
        float bestdist = 1000000;
        foreach(var v in FindObjectsOfType<Pickupable>())
        {
            if (Vector2.Distance(v.transform.position, this.transform.position) < bestdist)
            {
                best = v.gameObject;
                bestdist = Vector2.Distance(v.transform.position, this.transform.position);
            }
        }

        if (best != null && bestdist < 1f)
        {
            pickedUp = best.GetComponent<Pickupable>().gt;
            collectedItem = best.gameObject;
            collectedItem.transform.SetParent(this.transform);
            collectedItem.transform.localPosition = 2 * Vector3.up;
            collectedItem.transform.eulerAngles = Vector3.zero;
            Destroy(best.GetComponent<Pickupable>());
        }
    }


    public void tryDeposit()
    {
        foreach (var v in FindObjectsOfType<Shed>())
        {
            if (Vector2.Distance(v.transform.position,this.transform.position) < 1f)
            {
                // static gamestate..........

                if (name[0] == FindObjectOfType<Player>().team)
                {
                    FindObjectOfType<GameState>().p1.incr(true,pickedUp);
                } else
                {
                    FindObjectOfType<GameState>().p2.incr(false,pickedUp);

                }

                // enemy cheats right now LOL

                Destroy(collectedItem);
                collectedItem = null;
                return;
            }
        }
    }

    public void collect()
    {

        int start_x = (int)(this.transform.position.x / 0.6);
        int start_y = (int)(this.transform.position.y / 0.6);

        if (collectedItem == null)
        {
            var ta = GameState.getCatPickUpAdapter(name[0]);
            var move = CrapBFS.find(start_x, start_y, 2, ta);

            if(move.Item1 == 0 && move.Item2 == 0)
            {
                tryCollect();
            }

            var posDelt = new Vector3(move.Item1, move.Item2, 0).normalized;
            this.transform.position += posDelt * Time.deltaTime * CAT_SCALE;
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(posDelt.x > 0 ? 1f : -1f, 1f, 1f);
        }
        else
        {
            var ta = GameState.getTerrainAdapter(name[0]);
            var move = CrapBFS.find(start_x, start_y, 3, ta);
            if (move.Item1 == 0 && move.Item2 == 0)
            {
                tryDeposit();
            }
            var posDelt = new Vector3(move.Item1, move.Item2, 0).normalized;
            this.transform.position += posDelt * Time.deltaTime * (CAT_SCALE * 0.7f);
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(posDelt.x > 0 ? 1f : -1f, 1f, 1f);
        }
    }

    public void gather()
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
                if (collectType == GTYPE.BEAV && GameState.hm.objAt(start_x, start_y) == "tree")
                {
                    GameState.hm.attack(start_x, start_y);
                }

                if (collectType == GTYPE.GOL && GameState.hm.objAt(start_x, start_y) == "rock")
                {
                    GameState.hm.attack(start_x, start_y);
                }

                if (collectType == GTYPE.RAT)
                {
                    GameState.hm.attack(start_x, start_y);
                }
            }

            animTimeout -= Time.deltaTime;

            return;
        }


        var tres = GameState.TerrainType.TREES;
        int sVal = 1;

        switch(collectType)
        {
            case GTYPE.GOL: tres = GameState.TerrainType.ROCKS; sVal = 2; break;
            case GTYPE.RAT: tres = GameState.TerrainType.STRUCTURE; sVal = 5; break;
        }

       

        //this.transform.position += new Vector3(path[1][0] - path[0][0], path[1][1] - path[0][1]).normalized * Time.deltaTime;

        if (GameState.worldState[start_x, start_y] == tres)
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

            var ta = GameState.getTerrainAdapter(name[0]);
            var move = CrapBFS.find(start_x, start_y, sVal, ta);
            var posDelt = new Vector3(move.Item1, move.Item2, 0).normalized;
            this.transform.position += posDelt * Time.deltaTime;
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(posDelt.x > 0 ? 1f : -1f, 1f, 1f);
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

        if (other.tag == "Pickupable")
        {
            // caterpiller get
            Debug.Log("get!");
        }
    }
}
