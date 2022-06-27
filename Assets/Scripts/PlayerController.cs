using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private const float PLAYER_HARVEST_RANGE = 1f;

    public bool inMenu;

    public char team;
    // Start is called before the first frame update
    void Start()
    {
        inMenu = false;
        team = 'R';
    }

    GameObject constructionPrefab = null;

    public float animTimeout = 0f;


    public void createConstPrefab(GameObject b)
    {
        constructionPrefab = Instantiate(b);
        constructionPrefab.transform.SetParent(null);
        StructureManager.paint(constructionPrefab, team == 'R' ? new Color(1f,0f,0f,0.5f) : new Color(0f, 0f, 1f, 0.5f));
        constructionPrefab.name = b.name;
    }

    // Update is called once per frame
    void Update()
    {


        if (animTimeout > 0)
        {
            GetComponent<Animator>().SetBool("Walking", false);
            int start_x = (int)(this.transform.position.x / 0.6);
            int start_y = (int)(this.transform.position.y / 0.6);
            // check for a good harvest.
            if (animTimeout - Time.deltaTime <= 0)
            {
                GameState.hm.attack(start_x, start_y);
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
            this.transform.eulerAngles = new Vector3(0f, 180f, 0f);
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
        }

        GetComponent<Animator>().SetBool("Walking",walk);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (constructionPrefab != null)
        {
            var cell = GameState.gm.terrWalkable.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            constructionPrefab.transform.position = new Vector3(cell.x * 0.6f, cell.y * 0.6f, -10f);
            if (Input.GetMouseButtonDown(0))
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
                GetComponent<Animator>().SetTrigger("Attack");
                animTimeout = 2f;
                // REALLY need a cut down button
            }
        }

    }


}
