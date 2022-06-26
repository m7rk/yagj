using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject missionSelectPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectMission()
    {
        mainMenuPanel.SetActive(false);
        missionSelectPanel.SetActive(true);
    }

    public void PlayVsAI()
    {
        // code to load the player vs AI scene
        // SceneManager.LoadScene("nameScene");
        SceneManager.LoadScene("Overworld");
    }

    public void ReturnToMainMenu()
    {
        missionSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void LoadSelectedMission(System.Int32 rec)
    {
        try
        {
            SceneManager.LoadScene("Mission" + rec);
        }
        catch (Exception e)
        {
            //condition if not loading map
        }
    }


}
