using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePuzzle : MonoBehaviour
{
    private int pointsToWin;
    private int currentPoints;
    public GameObject normalShapes;
    public GameObject notCompleted;
    public GameObject completed;

    void Start()
    {
        pointsToWin = normalShapes.transform.childCount;
        notCompleted.SetActive(true);
        completed.SetActive(false);
    }

    void Update()
    {
        if (currentPoints >= pointsToWin)
        {
            // Win
            notCompleted.SetActive(false);
            completed.SetActive(true);
        }
    }

    public void AddPoints()
    {
        currentPoints++;
    }
}
