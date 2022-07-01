using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // really need to clean this up.

    public GameObject player;
    public GameObject antag;
    public GameObject book;

    private bool sidePicked = false;

    public GameObject btnBase;

    bool choseRed = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (sidePicked)
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -30);
        }
    }

    void setUp()
    {
        FindObjectOfType<GameState>().putAtBase(player,choseRed);
        FindObjectOfType<GameState>().putAtBase(antag,!choseRed);
        FindObjectOfType<Player>(true).team = choseRed ? 'R' : 'B';

        FindObjectOfType<Player>(true).gameObject.SetActive(true);
        if (choseRed)
        {
            FindObjectOfType<Player>(true).changeToRed();
        } else
        {
            FindObjectOfType<Antag>(true).changeToRed();
        }
        book.SetActive(true);
    }

    public void pickRed()
    {
        choseRed = true;
        sidePicked = true;
        Destroy(btnBase);
        this.transform.localScale = Vector3.one;
        setUp();
    }

    public void pickBlue()
    {
        sidePicked = true;
        Destroy(btnBase);
        this.transform.localScale = Vector3.one;
        setUp();
    }
}
