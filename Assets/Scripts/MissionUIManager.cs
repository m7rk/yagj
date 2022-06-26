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

    void Start()
    {

    }

    void Update()
    {
        switch(SceneManager.GetActiveScene().name)
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
}
