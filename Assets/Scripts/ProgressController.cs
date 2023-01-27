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
        if(isTutorialActive)
        {
            foreach (GameObject i in tutorialObj)
            {
                i.SetActive(false);
            }
            tutorialObj[tutorialStage].SetActive(true);
        }

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
}
