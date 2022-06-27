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
    [SerializeField]
    private GameObject factionSelectPanel;

    [SerializeField]
    private bool isGGISelected;
    [SerializeField]
    private bool isBTangramSelected;

    private int missionNumber;
    private bool isPlayVsAI;

    public Animator fadeAnimator;
    public float transitionTime = 1f;


    // Start is called before the first frame update
    void Start()
    {
        isGGISelected = false;
        isBTangramSelected = false;
        isPlayVsAI = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGGISelected || isBTangramSelected)
        {
            if(!isPlayVsAI)
            {
                try
                {
                    StartCoroutine(PlayFadeAnimation());

                    SceneManager.LoadScene("Mission" + missionNumber);
                }
                catch (Exception e)
                {
                    //condition if not loading map
                }
            }
            else
            {
                StartCoroutine(PlayFadeAnimation());

                SceneManager.LoadScene("Overworld");
            }
            
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
        isPlayVsAI = true;
        factionSelectPanel.SetActive(true);

    }

    public void ReturnToMainMenu()
    {
        missionSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void LoadSelectedMission(System.Int32 rec)
    {
        missionNumber = rec;
        factionSelectPanel.SetActive(true);
        missionSelectPanel.SetActive(false);
    }

    public void GGISleected()
    {
        isGGISelected = true;
    }

    public void BTangramSelected()
    {
        isBTangramSelected = true;
    }

    IEnumerator PlayFadeAnimation()
    {
        fadeAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(transitionTime);
    }

}
