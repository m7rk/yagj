using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionGameManager : MonoBehaviour
{
    [SerializeField]
    GameObject defeatPanel;
    [SerializeField]
    GameObject victoryPanel;

    [SerializeField]
    private bool victory;
    [SerializeField]
    private bool defeat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (victory)
        {
            victoryPanel.SetActive(true);

            // victory actions: sound, UI, etc...

        }

        if (defeat)
        {
            defeatPanel.SetActive(true);

            // defeat actions: sound, UI, etc...

        }
    }

    public void ReplayMission()
    {
        // reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextMission()
    {
        if (SceneManager.GetActiveScene().name == "Mission1")
        {
            SceneManager.LoadScene("Mission2");
        }
        else if (SceneManager.GetActiveScene().name == "Mission2")
        {
            SceneManager.LoadScene("Mission3");
        }
        else if (SceneManager.GetActiveScene().name == "Mission3")
        {
            SceneManager.LoadScene("Mission4");
        }
        else if (SceneManager.GetActiveScene().name == "Mission4")
        {
            SceneManager.LoadScene("Mission5");
        }
        else
        {
            // condition if mission5 or player vs AI
        }
    }
    public void MainMenu()
    {
        // load main menu scene
    }
}
