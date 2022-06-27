using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private bool sidePicked = false;

    public GameObject btnBase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sidePicked)
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -100);
        }
    }

    public void pickRed()
    {
        sidePicked = true;
        Destroy(btnBase);
        this.transform.localScale = Vector3.one;
    }

    public void pickBlue()
    {
        sidePicked = true;
        Destroy(btnBase);
        this.transform.localScale = Vector3.one;
    }
}
