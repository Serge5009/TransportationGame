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

    List<GameObject> spawnedVisuals;    

    //  Selected car
    [SerializeField] GameObject selectedCarPrefab;
    GameObject carTracker;

    //  Path
    [SerializeField] float distanceBetweenPoints = 0.5f;

    [SerializeField] GameObject pathNodePrefab;
    [SerializeField] GameObject pathCityPrefab;
    [SerializeField] GameObject pathEndsPrefab;
    [SerializeField] GameObject pathPointPrefab;
    List<GameObject> pathObjs;

    void Start()
    {
        spawnedVisuals = new List<GameObject>();
        pathObjs = new List<GameObject>();
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
        //Debug.Log("Drawing a path with " + path.Count + " nodes");

        if (path.Count < 2)
            return;

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


        Vector2 pointPointer = new Vector2();   //  Funny name, isn't it?
        pointPointer = path[0].transform.position;

        SpawnAPathPoint(pointPointer);

        for (int i = 0; i < path.Count - 1; i++)    //  Loop thru each node of the path but the last one
        {
            while (Vector2.Distance(pointPointer, path[i + 1].transform.position) > distanceBetweenPoints)  //  As long as pointer is far enough from the next node
            {
                Vector2 moveVector = path[i + 1].transform.position - (Vector3)pointPointer;    //  Find the vector between pointer and the next node           //  TO DO: this conversions are not nice))
                moveVector = moveVector.normalized * distanceBetweenPoints;                     //  Calculate the length of the next step
                pointPointer += moveVector;                                                     //  Move the pointer to a new position
                SpawnAPathPoint(pointPointer);                                                  //  Spawn a new point
            }
                
            pointPointer = path[i + 1].transform.position;                                      //  Jump to the next node
        }
    }
    void SpawnAPathPoint(Vector2 pos)
    {
        GameObject newPoint = Instantiate(pathPointPrefab, pos, Quaternion.identity);
        pathObjs.Add(newPoint);
        spawnedVisuals.Add(newPoint);
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
    }
}
