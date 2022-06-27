using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionUIManager : MonoBehaviour
{
    public TMPro.TMP_Text missionDisplayText;

    [SerializeField]
    private string mission1Text;
    [SerializeField]
    private string mission2Text;
    [SerializeField]
    private string mission3Text;
    [SerializeField]
    private string mission4Text;
    [SerializeField]
    private string mission5Text;

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

    public Animator fadeAnimator;
    public float transitionTime = 1f;

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
                missionDisplayText.text = mission1Text;
                break;
            case "Mission2":
                missionDisplayText.text = mission2Text;
                break;
            case "Mission3":
                missionDisplayText.text = mission3Text;
                break;
            case "Mission4":
                missionDisplayText.text = mission4Text;
                break;
            case "Mission5":
                missionDisplayText.text = mission5Text;
                break;
            default:
                missionDisplayText.text = "Destroy all the enemies in the area.";
                break;
        }
    }

    public void ReplayMission()
    {

        StartCoroutine(PlayFadeAnimation());

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextMission()
    {
        if (SceneManager.GetActiveScene().name == "Mission1")
        {
            StartCoroutine(PlayFadeAnimation());

            SceneManager.LoadScene("Mission2");
        }
        else if (SceneManager.GetActiveScene().name == "Mission2")
        {
            StartCoroutine(PlayFadeAnimation());

            SceneManager.LoadScene("Mission3");
        }
        else if (SceneManager.GetActiveScene().name == "Mission3")
        {
            StartCoroutine(PlayFadeAnimation());

            SceneManager.LoadScene("Mission4");
        }
        else if (SceneManager.GetActiveScene().name == "Mission4")
        {
            StartCoroutine(PlayFadeAnimation());

            SceneManager.LoadScene("Mission5");
        }
        else
        {
            StartCoroutine(PlayFadeAnimation());

            SceneManager.LoadScene("MainMenu");
        }
    }
    public void MainMenu()
    {
        // if the game is paused and the main menu button is clicked, resume the time
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        StartCoroutine(PlayFadeAnimation());

        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPauseMenu()
    {
        pausePanel.SetActive(true);
        // pause the game
        Time.timeScale = 0;
    }

    public void ResumeMission()
    {
        pausePanel.SetActive(false);
        // unpause the game
        Time.timeScale = 1;
    }

    IEnumerator PlayFadeAnimation()
    {
        fadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
    }
}
