using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

            //  Variables:

    List<GameObject> spawnedVisuals; // Keeps track of all visuals on the screen

    //  Selected car
    [SerializeField] GameObject selectedCarPrefab;          //prefab
    GameObject carTracker;                                  //  A visual that is following the selected car

    //  Path
    [SerializeField] float distanceBetweenPoints = 0.5f;    //  How far are the small points gonna spawn from each other
    bool isInPathCreateMode = false;                        //  For keeping track of the mode
    [SerializeField] GameObject pathNodePrefab;             //prefab
    [SerializeField] GameObject pathCityPrefab;             //prefab
    [SerializeField] GameObject pathEndsPrefab;             //prefab
    [SerializeField] GameObject pathPointPrefab;            //prefab
    List<GameObject> pathObjs;                              //  Keeps track of all visuals related to the path display

    //  Connect mode
    [SerializeField] GameObject canConnectPrefab;           //prefab
    [SerializeField] GameObject currentConnectPrefab;       //prefab
    bool isInConnectMode = false;                           //  For keeping track of the mode
    List<GameObject> connectObjs;                           //  Keeps track of all visuals related to connect mode


    void Start()
    {
        //  Create lists
        spawnedVisuals = new List<GameObject>();
        pathObjs = new List<GameObject>();
        connectObjs = new List<GameObject>();
    }


    void Update()
    {
        //  Selected car
        if(GameManager.gm.selectedCar && !carTracker)   //  When selected
        {
            SelectedCarStart();                             //  Draw a car tracker
            PathStart(GameManager.gm.selectedCar.path);     //  Paint the path
        }   
        if(GameManager.gm.selectedCar && carTracker)    //  When active
        {
            SelectedCarUpdate();                            //  Move the tracker
        }
        if(!GameManager.gm.selectedCar && carTracker)   //  When deselected
        {
            SelectedCarStop();                              //  Delete the tracker
            PathStop();                                     //  Erase path visuals
        }

        //  Path creation mode
        if(GameManager.gm.gState == GAME_STATE.PATH && !isInPathCreateMode)
        {
            PathStart(Car.newPath);         //  Draw the new path
            isInPathCreateMode = true;      //  Keep track of the mode
        }
        if (GameManager.gm.gState != GAME_STATE.PATH && isInPathCreateMode)
        {
            PathStop();                     //  Clear the path
            isInPathCreateMode = false;     //  Keep track of the mode
        }

        //  Connect mode
        if(GameManager.gm.gState == GAME_STATE.CONNECT && !isInConnectMode)
        {
            ConnectStart();
            isInConnectMode = true;
        }
        if(GameManager.gm.gState != GAME_STATE.CONNECT && isInConnectMode)
        {
            ConnectStop();
            isInConnectMode = false;
        }
    }


    //  Selected car
    void SelectedCarStart()     //  Spawn the car tracker and add it to the list of visuals
    {
        carTracker = Instantiate(selectedCarPrefab, GameManager.gm.selectedCar.transform.position, Quaternion.identity);
        spawnedVisuals.Add(carTracker);
    }
    void SelectedCarUpdate()    //  Move the car tracker with selected car
    {
        carTracker.transform.position = GameManager.gm.selectedCar.transform.position;
    }
    void SelectedCarStop()      //  Delete the car tracker and remove it from the list
    {
        Destroy(carTracker);
        carTracker = null;
        spawnedVisuals.Remove(carTracker);
    }

    //  Path creation mode
    void PathStart(List<RoadNode> path)     //  Draws the path
    {
        //Debug.Log("Drawing a path with " + path.Count + " nodes");

        if (path.Count < 2)     //  If there's no path - we dont draw it *_*
            return;

        foreach (RoadNode node in path)     //  Loop thru all nodes of this path
        {
            GameObject prefabToSpawn = pathNodePrefab;  //  Default node prefab
            if (node.gameObject.GetComponent<City>())   //  If it's a city - change the prefab
                prefabToSpawn = pathCityPrefab;
            if (node == path[0] || node == path[path.Count - 1])    //  If it's one of the ends - change the prefab
                prefabToSpawn = pathEndsPrefab;

            GameObject vis = Instantiate(prefabToSpawn, node.transform.position, Quaternion.identity);  //  Spawn the point
            pathObjs.Add(vis);          //  Add to the list
            spawnedVisuals.Add(vis);    //  Add to the list
        }


        Vector2 pointPointer = new Vector2();       //  Funny name, isn't it?
        pointPointer = path[0].transform.position;  //  We're gonna move this vector along the path and spawn points where needed

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
        GameObject newPoint = Instantiate(pathPointPrefab, pos, Quaternion.identity);   //  Spawn the point
        pathObjs.Add(newPoint);         //  Add to the list
        spawnedVisuals.Add(newPoint);   //  Add to the list
    }
    void PathStop()
    {
        foreach (GameObject i in pathObjs)  //  Loop thru all path visuals
        {
            Destroy(i);         //  Delete 
        }
        pathObjs.Clear();       //  Clear the list
    }
    public void PathUpdate()    //  This is called every time there's a change in path, redraws all path visuals
    {
        PathStop();
        PathStart(Car.newPath);
    }

    //  Connect mode
    void ConnectStart()
    {
        foreach (RoadNode n in RoadNetwork.rn.nodes)
        {
            if(RoadNetwork.rn.CanConnectNodes(n, RoadNetwork.rn.activeForConnection))   //  If this node can be connected to the active one
            {
                GameObject nodeVis = Instantiate(canConnectPrefab, n.transform.position, Quaternion.identity);
                connectObjs.Add(nodeVis);
                spawnedVisuals.Add(nodeVis);

                nodeVis.GetComponentInChildren<TextMeshPro>().text = "$" + RoadNetwork.rn.ConnectionPrice(n).ToString("F0");

            }
        }

        GameObject currentVis = Instantiate(currentConnectPrefab, RoadNetwork.rn.activeForConnection.transform.position, Quaternion.identity);
        connectObjs.Add(currentVis);
        spawnedVisuals.Add(currentVis);
    }
    void ConnectStop()
    {
        foreach (GameObject v in connectObjs)
        {
            Destroy(v);
        }
        connectObjs.Clear();
    }
    public void ConnectUpdate() //  This is called every time there's a change in connections, redraws all connect visuals
    {
        ConnectStop();
        ConnectStart();
    }

    public void RemoveAllVisuals()
    {
        foreach (GameObject v in spawnedVisuals)    //  Loop thru all visuals
        {
            Destroy(v);         //  Delete
        }
        spawnedVisuals.Clear(); //  Clear the list
    }
}
