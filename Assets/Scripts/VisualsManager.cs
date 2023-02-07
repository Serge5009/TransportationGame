using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ! SINGLETON !

//  This scrip is in charge of spawning temporary visual objects aimed to aim the player to keep track of selected objects or routes
public class VisualsManager : MonoBehaviour
{
    public static VisualsManager visMgr { get; private set; }  //  Singleton for the Visuals Manager
    void Awake()
    {
        if (visMgr != null && visMgr != this)
            Destroy(this);
        else
            visMgr = this;
    }

    VISUALS_MODE currentMode = VISUALS_MODE.NONE;
    List<GameObject> spawnedVisuals;

    //  Selected car
    [SerializeField] GameObject selectedCarPrefab;
    GameObject carTracker;

    [SerializeField] GameObject pointPrefab;

    void Start()
    {
        spawnedVisuals = new List<GameObject>();
    }

    void Update()
    {
        //  Selected car
        if(GameManager.gm.selectedCar && !carTracker)
        {
            SelectedCarStart();
        }
        if(GameManager.gm.selectedCar && carTracker)
        {
            SelectedCarUpdate();
        }
        if(!GameManager.gm.selectedCar && carTracker)
        {
            SelectedCarStop();
        }
    }

    //  Selected car
    void SelectedCarStart()
    {
        carTracker = Instantiate(selectedCarPrefab, GameManager.gm.selectedCar.transform.position, Quaternion.identity);
    }
    void SelectedCarUpdate()
    {
        carTracker.transform.position = GameManager.gm.selectedCar.transform.position;
    }
    void SelectedCarStop()
    {
        Destroy(carTracker);
        carTracker = null;
    }

    public void RemoveAllVisuals()
    {
        foreach (GameObject v in spawnedVisuals)
        {
            Destroy(v);
        }
        spawnedVisuals.Clear();
        currentMode = VISUALS_MODE.NONE;
    }
}

enum VISUALS_MODE
{
    NONE,
    PATH,

    NUM_VISUALS_MODE
}
