using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antag : MonoBehaviour
{
    public Sprite redAlt;

    public char team;

    public bool findRock;

    public float waitTime;
    
    public Tuple<int,int> dest;

    public void changeToRed()
    {
        transform.Find("starm").GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f);
        transform.Find("r").GetComponent<SpriteRenderer>().sprite = redAlt;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            GetComponent<Animator>().SetBool("Walking", false);
            return;
        }

        dest = CrapBFS.find((int)(this.transform.position.x / 0.6f), (int)(this.transform.position.y / 0.6), findRock ? 2 : 1, GameState.getTerrainAdapter(team));

        if (dest.Item1 == 0 && dest.Item2 == 0)
        {
            waitTime = UnityEngine.Random.Range(0f, 5f);
            findRock = !findRock;
            // 5 spawnables, pick one, try to build if we have the res.
            int rand = ((int)UnityEngine.Random.Range(0, 10));

            FindObjectOfType<GameState>().spawnBeaver(this.gameObject,team);

            /**
            int[] numberOfPieces = new int[] { 2, 2, 0, 0, 0 };
            int[] numberOfBuilds = new int[] { 0, 0, 0, 0, 0, 1, 0, 0 };
            List<int> osg = (new CostAnalysis.TurrentCostAnalysis()).Ordering_ExpensiveGreedy(numberOfPieces, numberOfBuilds);
            */
            
        }



        this.transform.position += new Vector3(dest.Item1, dest.Item2, 0).normalized * Time.deltaTime;

        GetComponent<Animator>().SetBool("Walking",true);

    }


}
