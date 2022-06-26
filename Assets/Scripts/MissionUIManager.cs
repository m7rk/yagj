using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionUIManager : MonoBehaviour
{
    public TMPro.TMP_Text missionDisplayText;

    [SerializeField]
    private string mission1;
    [SerializeField]
    private string mission2;
    [SerializeField]
    private string mission3;
    [SerializeField]
    private string mission4;
    [SerializeField]
    private string mission5;

    [SerializeField]
    private GameObject defeatPanel;
    [SerializeField]
    private GameObject victoryPanel;
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private bool victory;
    [SerializeField]
    private bool defeat;

    void Start()
    {

    }

    void Update()
    {
        AssignMissionText();

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

    private void AssignMissionText()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Mission1":
                missionDisplayText.text = mission1;
                break;
            case "Mission2":
                missionDisplayText.text = mission2;
                break;
            case "Mission3":
                missionDisplayText.text = mission3;
                break;
            case "Mission4":
                missionDisplayText.text = mission4;
                break;
            case "Mission5":
                missionDisplayText.text = mission5;
                break;
            default:
                missionDisplayText.text = "Destroy all the enemies in the area.";
                break;
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
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPauseMenu()
    {
        pausePanel.SetActive(true);
    }

    public void ResumeMission()
    {
        pausePanel.SetActive(false);
    }
}
