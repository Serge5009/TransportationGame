using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//  This script is responsible for tutorials and other progress related stages


public class ProgressController : MonoBehaviour
{
    public static ProgressController pControll { get; private set; }  //  Singleton for the Progress Controller
    void Awake()
    {
        if (pControll != null && pControll != this)
            Destroy(this);
        else
            pControll = this;
    }

    public int tutorialStage = 0;
    public bool isTutorialActive = false;

    public bool isSandboxMode = false;

    [SerializeField] List<GameObject> tutorialObj;

    //  Stats
    int roadsBuilt = 0;

    void Start()
    {
        
    }

    void Update()
    {
        foreach (GameObject i in tutorialObj)           //  Turn off all tutorial pop-ups
            i.SetActive(false);
        if (isTutorialActive)                           //  Turn on the active one 
            tutorialObj[tutorialStage].SetActive(true);

        if (tutorialStage >= tutorialObj.Count || tutorialStage < 0)
            isTutorialActive = false;

        if (tutorialStage < 19 && roadsBuilt >= 2)
            tutorialStage = 19;
        
        if (GameManager.gm.selectedCity)
            if (GameManager.gm.selectedCity.assignedCars.Count > 0)
                tutorialStage = 21;

        if (isSandboxMode)
            GameManager.gm.money = 1000000;
    }

    public void StartTutorial()
    {
        tutorialStage = 1;
        isTutorialActive = true;
        isSandboxMode = true;
    }
    public void StopTutorial()
    {
        isTutorialActive = false;
        isSandboxMode = false;

        StartNewGame();
    }
    public void OnCameraMove()
    {
        if (tutorialStage == 1)
            tutorialStage++;
    }
    public void OnCameraZoom()
    {
        if (tutorialStage == 2)
            tutorialStage++;
    }
    public void OnCameraClick()
    {
        if (tutorialStage == 3)
            tutorialStage++;
        else if (tutorialStage == 5)
            tutorialStage++;
        else if (tutorialStage == 6)
            tutorialStage++;
        else if (tutorialStage == 8)
            tutorialStage++;
        else if (tutorialStage == 10)
            tutorialStage++;
        else if (tutorialStage == 11)
            tutorialStage++;
        else if (tutorialStage == 22)
            tutorialStage++;
        else if (tutorialStage == 26)
            tutorialStage++;
        else if (tutorialStage == 27)
            tutorialStage++;
    }
    public void OnCitySelect(City selected)
    {
        if (tutorialStage == 4)
            tutorialStage++;
        else if (tutorialStage == 14)
            tutorialStage++;
        else if (tutorialStage == 19 && selected.isOwned)
            tutorialStage++;
    }
    public void OnCityDeSelect()
    {
        if (tutorialStage == 5 || tutorialStage == 6 || tutorialStage == 7)
            tutorialStage = 4;
        else if (tutorialStage == 15)
            tutorialStage = 14;
        else if (tutorialStage == 20 || tutorialStage == 21)
            tutorialStage = 19;
    }
    public void OnCityAccess(City accessed)
    {
        if (tutorialStage == 7)
            tutorialStage++;
    }
    public void OnCityHubPurchase(City hubCity)
    {
        if (tutorialStage == 9)
            tutorialStage++;
    }
    public void OnBuildModeEnter()
    {
        if (tutorialStage == 12)
            tutorialStage++;
    }
    public void OnConnectModeEnter()
    {
        if (tutorialStage == 15)
            tutorialStage++;
    }
    public void OnPathModeEnter()
    {
        if (tutorialStage == 23)
            tutorialStage++;
    }
    public void OnNodeBuilt()
    {
        if (tutorialStage == 13)
            tutorialStage++;
    }
    public void OnNodeConnectFail()
    {
        if (tutorialStage == 16)
            tutorialStage = 17;
    }
    public void OnNodeConnectSuccess()
    {
        if (tutorialStage == 16)
            tutorialStage = 18;

        roadsBuilt++;
    }
    public void OnCarSelect(Car selected)
    {
        if (tutorialStage == 21)
            tutorialStage++;
    }
    public void OnCarDeSelect()
    {
        if (tutorialStage == 22)
            tutorialStage = 19;
    }
    public void OnPathNodeAdded()
    {
        if (tutorialStage == 24)
            tutorialStage++;
    }
    public void OnPathFinish()
    {
        if (tutorialStage == 25)
            tutorialStage++;
    }

    //  Last choice functions
    public void StartSandboxMode()
    {
        tutorialStage = 0;
        isTutorialActive = false;
        isSandboxMode = true;
    }
    public void StartNewGame()
    {
        tutorialStage = 0;
        isTutorialActive = false;
        isSandboxMode = false;

        GameManager.gm.StartNewGame();
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
