using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isTutorialActive = true;

    [SerializeField] List<GameObject> tutorialObj;

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
    }

    public void StartTutorial()
    {
        tutorialStage = 1;
        isTutorialActive = true;
    }
    public void StopTutorial()
    {
        isTutorialActive = false;
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
    }
    public void OnCitySelect(City selected)
    {
        if (tutorialStage == 4)
            tutorialStage++;
    }
    public void OnCityDeSelect()
    {
        if (tutorialStage == 5 || tutorialStage == 6 || tutorialStage == 7)
            tutorialStage = 4;
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
    public void OnNodeBuilt()
    {
        if (tutorialStage == 13)
            tutorialStage++;
    }
}
