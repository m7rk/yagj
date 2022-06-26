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


    public void createConstPrefab(GameObject b)
    {
        Instantiate(b);
        b.transform.SetParent(null);
        foreach(Transform child in b.transform)
        {
            child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        if(constructionPrefab != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            constructionPrefab.transform.position = worldPosition;
        }

    }
}
