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

    //  Path
    [SerializeField] GameObject pathNodePrefab;
    [SerializeField] GameObject pathCityPrefab;
    [SerializeField] GameObject pathEndsPrefab;
    [SerializeField] GameObject pathPointPrefab;
    List<GameObject> pathObjs;

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
            PathStart(GameManager.gm.selectedCar.path);
        }
        if(GameManager.gm.selectedCar && carTracker)
        {
            SelectedCarUpdate();
        }
        if(!GameManager.gm.selectedCar && carTracker)
        {
            SelectedCarStop();
            PathStop();
        }
    }

    //  Selected car
    void SelectedCarStart()
    {
        carTracker = Instantiate(selectedCarPrefab, GameManager.gm.selectedCar.transform.position, Quaternion.identity);
        spawnedVisuals.Add(carTracker);
    }
    void SelectedCarUpdate()
    {
        carTracker.transform.position = GameManager.gm.selectedCar.transform.position;
    }
    void SelectedCarStop()
    {
        Destroy(carTracker);
        carTracker = null;
        spawnedVisuals.Remove(carTracker);
    }

    void PathStart(List<RoadNode> path)
    {
        pathObjs = new List<GameObject>();
        foreach (RoadNode node in path)
        {
            GameObject prefabToSpawn = pathNodePrefab;
            if (node.gameObject.GetComponent<City>())
                prefabToSpawn = pathCityPrefab;
            if (node == path[0] || node == path[path.Count - 1])
                prefabToSpawn = pathEndsPrefab;

            GameObject vis = Instantiate(prefabToSpawn, node.transform.position, Quaternion.identity);
            pathObjs.Add(vis);
            spawnedVisuals.Add(vis);
        }
    }
    void PathStop()
    {
        foreach (GameObject i in pathObjs)
        {
            Destroy(i);
        }
        pathObjs.Clear();
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
