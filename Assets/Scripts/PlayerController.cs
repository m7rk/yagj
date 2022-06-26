using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    GameObject constructionPrefab = null;
    public float animTimeout = 0f;


    public void createConstPrefab(GameObject b)
    {
        constructionPrefab = Instantiate(b);
        constructionPrefab.transform.SetParent(null);
        foreach(Transform child in constructionPrefab.transform)
        {
            child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
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
                GameState.hm.cutDown(start_x, start_y);
            }

            animTimeout -= Time.deltaTime;

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

        if (constructionPrefab != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var cell = GameState.gm.terrWalkable.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            constructionPrefab.transform.position = new Vector3(cell.x * 0.6f, cell.y * 0.6f, -20);

            if (Input.GetMouseButtonDown(0))
            {
                constructionPrefab.transform.SetParent(FindObjectOfType<GameState>().BuildingParent.transform);
                foreach (Transform child in constructionPrefab.transform)
                {
                    child.GetComponent<SpriteRenderer>().color = Color.white;
                }
                constructionPrefab = null;

            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Animator>().SetTrigger("Attack");
                animTimeout = 2f;

            }
        }

    }


}
