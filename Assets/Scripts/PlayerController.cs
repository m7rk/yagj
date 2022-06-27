using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Sprite redAlt;

    private const float PLAYER_HARVEST_RANGE = 0.6f;

    public bool inMenu;

    private int[] harvCoord;

    public char team;
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
    }

    public void changeToRed()
    {
        transform.Find("starm").GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f);
        transform.Find("r").GetComponent<SpriteRenderer>().sprite = redAlt;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.P))
        {
            Time.timeScale = 5f;
        } else
        {
            Time.timeScale = 1f;
        }


        if (animTimeout > 0)
        {
            GetComponent<Animator>().SetBool("Walking", false);

            // check for a good harvest.
            if (animTimeout - Time.deltaTime <= 0)
            {
                GameState.hm.attack(harvCoord[0],harvCoord[1]);
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
            var cell = GameState.gm.terrWalkable.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // OOB exception? even possible, this??
            bool valid = (GameState.worldState[cell.x, cell.y] == GameState.TerrainType.GRASS);
            Color baseCol = team == 'R' ? new Color(1f, 0f, 0f, 0.5f) : new Color(0f, 0f, 1f, 0.5f);
            StructureManager.paint(constructionPrefab,valid ? baseCol : new Color(1f,1f,1f,0.2f));

            constructionPrefab.transform.position = new Vector3(cell.x * 0.6f, cell.y * 0.6f, -10f);
            if (Input.GetMouseButtonDown(0) && valid)
            {
                switch(constructionPrefab.name)
                {
                    case "Shed": FindObjectOfType<StructureManager>().putShed(cell.x, cell.y, team); break;
                    case "Turret": FindObjectOfType<StructureManager>().putTurret(cell.x, cell.y, team); break;
                    case "PFactory": FindObjectOfType<StructureManager>().putPFact(cell.x, cell.y, team); break;
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
                animTimeout = 2f;
                // REALLY need a cut down button
            }
        }

    }


}
