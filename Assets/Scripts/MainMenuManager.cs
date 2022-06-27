using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject missionSelectPanel;

    private bool missionLoaded;

    public Animator fadeAnimator;
    public float transitionTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        missionLoaded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (missionLoaded)
        {
            StartCoroutine(PlayFadeAnimation());

            SceneManager.LoadScene("Overworld");
        }

        
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
        missionLoaded = true;
        GameState.MISSION_SEL = -1;
        // (just load the game)

    }

    public void ReturnToMainMenu()
    {
        missionSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void LoadSelectedMission(System.Int32 rec)
    {
        GameState.MISSION_SEL = rec;
        SceneManager.LoadScene("Overworld");
    }

    IEnumerator PlayFadeAnimation()
    {
        fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);
    }

}
