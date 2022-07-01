using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private const float PLAYER_HARVEST_RANGE = 0.6f;

    public bool inMenu;

    private int[] harvCoord;

    public class GramCollection
    {
        public int LT = 0;
        public int MT = 0;
        public int ST = 2;
        public int S = 2;
        public int P = 0;

        public void incr(bool player, GramType gramType)
        {
            if (player)
            {
                FindObjectOfType<UIManager>().setIncrFlash(gramType);
            }

            switch (gramType)
            {
                case GramType.LT:
                    LT += 1; break;
                case GramType.MT:
                    MT += 1; break;
                case GramType.ST:
                    ST += 1; break;
                case GramType.P:
                    P += 1; break;
                case GramType.S:
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

        public void incr(bool player, string gramType)
        {
            switch (gramType)
            {
                case "lt":
                    incr(player, GramType.LT); break;
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

    public GramCollection gc;

    // Start is called before the first frame update
    void Start()
    {
        inMenu = false;
    }

    GameObject constructionPrefab = null;

    public float animTimeout = 0f;


    public void createConstPrefab(GameObject b)
    {
        constructionPrefab = Instantiate(b);
        constructionPrefab.transform.SetParent(null);
        constructionPrefab.name = b.name;
        constructionPrefab.GetComponent<BoxCollider2D>().enabled = false;

        MonoBehaviour[] scripts = constructionPrefab.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    public void changeToRed()
    {
        transform.Find("starm").GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f);
        //transform.Find("r").GetComponent<SpriteRenderer>().sprite = redAlt;
    }

    // Update is called once per frame
    void Update()
    {
        /**
        if(Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 5f;
        } else
        {
            Time.timeScale = 1f;
        }
        */


        if (animTimeout > 0)
        {
            GetComponent<Animator>().SetBool("Walking", false);

            // check for a good harvest.
            if (animTimeout - Time.deltaTime <= 0)
            {
                GameState.gs.hm.attack(harvCoord[0],harvCoord[1]);
            }

            animTimeout -= Time.deltaTime;

            return;
        }

        if(inMenu)
        {
            return;
        }

        bool walk = false;
        if (Input.GetKey(KeyCode.W))
        {
            walk = true;
            this.transform.position += new Vector3(0,Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            walk = true;
            this.transform.position += new Vector3(-Time.deltaTime, 0);
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            walk = true;
            this.transform.position += new Vector3(0, -Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            walk = true;
            this.transform.position += new Vector3(Time.deltaTime,0);
            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            this.transform.GetComponentInChildren<Animator>().transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }

        GetComponent<Animator>().SetBool("Walking",walk);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (constructionPrefab != null)
        {
            var cell = GameState.gs.gm.terrWalkable.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // OOB exception? even possible, this??
            bool valid = (GameState.gs.worldState[cell.x, cell.y] == GameState.TerrainType.GRASS);
            Color baseCol = team == 'R' ? new Color(1f, 0f, 0f, 0.5f) : new Color(0f, 0f, 1f, 0.5f);
            
            //StructureManager.paint(constructionPrefab,valid ? baseCol : new Color(1f,1f,1f,0.2f));

            constructionPrefab.transform.position = new Vector3(cell.x * 0.6f, cell.y * 0.6f, -10f);
            if (Input.GetMouseButtonDown(0) && valid)
            {
                switch(constructionPrefab.name)
                {
                    case "Shed": FindObjectOfType<StructureManager>().putShed(cell.x, cell.y, team); break;
                    case "Turret": FindObjectOfType<StructureManager>().putTurret(cell.x, cell.y, team); break;
                    case "Factory": FindObjectOfType<StructureManager>().putPFact(cell.x, cell.y, team); break;
                }
                // use name to build (shitty)
                
                Destroy(constructionPrefab);
                constructionPrefab = null;
            }
        }
        else
        {
            worldPosition.z = this.transform.position.z;
            // only attack if within 50 px?
            if (Input.GetMouseButtonDown(0) && (worldPosition - this.transform.position).magnitude < PLAYER_HARVEST_RANGE)
            {
                harvCoord = new int[2] { (int)(worldPosition.x / 0.6), (int)(worldPosition.y / 0.6) };
                GetComponent<Animator>().SetTrigger("Attack");
                animTimeout = 1f;
                // REALLY need a cut down button
            }
        }

    }


}
