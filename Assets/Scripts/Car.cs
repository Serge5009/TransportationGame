using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Car : MonoBehaviour
{
    //  Track keeping
    public int carID;
    static int numCars = 0;

    //  Stats
    public GameObject homeCity;
    public GameObject destination;
    public float speed = 20.0f;
    public int capacity = 10;
    public int load = 0;
    float parkedTimer = 0.0f;

    //  Settings
    public float interactDistance = 25.0f;
    public bool isSelected = false;

    //  Route
    public List<RoadNode> path;
    int nextNode;
    bool isMovingTo = true;
        //  Static
    [HideInInspector] public static List<RoadNode> newPath;
    [HideInInspector] public static Car newPathAssociatedCar;

    //  Prefabs     //  TO DO: move to another object
    [SerializeField] GameObject FlyingTextPrefab;
    [SerializeField] GameObject AddedTextPrefab;
    [SerializeField] GameObject PathCreatedTextPrefab;

    //  Components
    SpriteRenderer thisSprite;

    void Start()
    {
        CheckPath();
        nextNode = 0;
        carID = numCars;
        numCars++;

        if (!FlyingTextPrefab)
            Debug.LogError("No FlyingTextPrefab found");
        if (!AddedTextPrefab)
            Debug.LogError("No AddedTextPrefab found");
        if (!PathCreatedTextPrefab)
            Debug.LogError("No PathCreatedTextPrefab found");

        thisSprite = GetComponent<SpriteRenderer>();
        if (!thisSprite)
            Debug.LogError("No thisSprite found");

        if (!homeCity.GetComponent<City>().assignedCars.Contains(this)) //  Add the car to the list in its city if needed
            homeCity.GetComponent<City>().assignedCars.Add(this);
    }

    void Update()
    {
            //  Error checks
        if (!homeCity)
            Debug.LogError("No homeCity found");
        if (!destination)
        {
            ResetToHomeCity();
            parkedTimer = 1.0f;
        }

            //  Interaction checks

        if(destination) //  Run this code only if the car is running
        {
            if (isNear(homeCity) && nextNode == 0)     //  If within range with home city will try to load more
            {
                if (!isMovingTo)
                    UnloadTo(homeCity.GetComponent<City>());
                isMovingTo = true;
            }
            if (isNear(destination) && nextNode == path.Count - 1)     //  If within range with destination will try to unload
            {
                if (isMovingTo)
                    UnloadTo(destination.GetComponent<City>());
                isMovingTo = false;
            }
            if (isNear(path[nextNode].gameObject))     //  If within range with with the next node will swith to the next one
            {
                if (path[nextNode].GetComponent<City>())
                    LoadFrom(path[nextNode].GetComponent<City>());

                if (isMovingTo)
                    nextNode++;
                else
                    nextNode--;
            }
        }

        //  Parking check
        if (parkedTimer > 0.0f)    //  If the car is in porcess of interaction - skip the rest of the update and make the car invisible
        {
            thisSprite.color = new Color(1, 1, 1, 0);
            parkedTimer -= Time.deltaTime;
            return;
        }
        thisSprite.color = new Color(1, 1, 1, 1);
        parkedTimer = 0.0f;
        
            //  Movement
        transform.position += (path[nextNode].transform.position - transform.position).normalized * speed * Time.deltaTime;   //  Move towards the next node
        transform.LookAt(path[nextNode].transform.position);    //  Making the car face it's direction
        transform.Rotate(0.0f, 90.0f, 90.0f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f); //  Keep the car on z=0

        //  Click registering
        //  https://www.youtube.com/watch?v=5KLV6QpSAdI
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(transform.position, clickPos) <= 1 && GameManager.gm.gState == GAME_STATE.PLAY)     //  TO DO: new selection logic might be needed later
            {
                GameManager.gm.SelectCar(this);

                //TO DO: add sound effects
            }
        }
    }

    public static void ResetCars()
    {
        numCars = 0;
    }

    void UnloadTo(City toUnload)
    {
        if (load == 0)
            return;

        GameObject newText = Instantiate(FlyingTextPrefab, transform.position, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = load.ToString();
        newText.GetComponent<TextMeshPro>().color = Color.yellow;

        parkedTimer += load / 10;       //  Take some time to unload

        GameManager.gm.TakeMoney(-load, false);         //  TO DO: make money logic more ineresting ;   TO DO: take -load is not really readable)))
        load = 0;
    }

    void LoadFrom(City toLoad)
    {
        while (load < capacity && toLoad.passengers > 0)
        {
            load++;                 //  Add a passenger to the car
            toLoad.passengers--;    //  Remove a passenger from the city
            parkedTimer += 0.1f;    //  Take some time to load
        }
    }

    bool isNear(GameObject place)   //  This function allows to check if a certain object is within car's range
    {
        float distance = Vector2.Distance(transform.position, place.transform.position);
        return (distance <= interactDistance);
    }

    bool CheckPath()
    {
        if (path.Count == 0)
            return false;

        bool returnVal = true;

        if (path[0].gameObject != homeCity)
        {
            Debug.LogWarning("Path should start with a home city");
            GameManager.gm.PopUp("Path should start with a home city"); //  TO DO:  Probably should get rid of it, cuz it's specific for each call
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            if(!path[i].connections.Contains(path[i + 1]))
            {
                GameManager.gm.PopUp("This car has an invalid path!");  //  TO DO:  Probably should get rid of it, cuz it's specific for each call
                Debug.LogWarning("This car has an invalid path!");
                return false;
            }
        }

        return returnVal;
    }
    bool CheckPath(List<RoadNode> nodeList)
    {
        bool returnVal = true;

        if (nodeList[0].gameObject != homeCity)
        {
            Debug.LogWarning("Path should start with a home city");
            GameManager.gm.PopUp("Path should start with a home city"); //  TO DO:  Probably should get rid of it, cuz it's specific for each call
        }

        for (int i = 0; i < nodeList.Count - 1; i++)
        {
            if (!nodeList[i].connections.Contains(nodeList[i + 1]))
            {
                GameManager.gm.PopUp("This car has an invalid path!");  //  TO DO:  Probably should get rid of it, cuz it's specific for each call
                Debug.LogWarning("This car has an invalid path!");
                return false;
            }
        }

        return returnVal;
    }

    public void CreatePath()
    {
        GameManager.gm.DeselectCar();
        GameManager.gm.gState = GAME_STATE.PATH;
        newPathAssociatedCar = this;
        newPath = new List<RoadNode>();
        newPath.Add(homeCity.GetComponent<RoadNode>());

        //  Tutorial
        ProgressController.pControll.OnPathModeEnter();
    }

    public void AddPathNode(RoadNode nodeToAdd)
    {
        if (nodeToAdd == homeCity.GetComponent<RoadNode>() && newPath.Count <= 1)   //  We already have the home city as the first node
            return;

        if(nodeToAdd == newPath[newPath.Count - 1]) //  If clicked on the same node twice - finish the path
        {
            FinishPath();
            return;
        }

        newPath.Add(nodeToAdd);

        if(!CheckPath(newPath))             //  If sequence is incorrect
        {
            newPath.Remove(nodeToAdd);      //  Remove the new node
            //  TO DO: Add sound effect
        }
        else                                //  If the route is fine 
        {
            GameObject newAddedText = Instantiate(AddedTextPrefab, nodeToAdd.transform.position, Quaternion.identity);
            VisualsManager.visMgr.PathUpdate(); //  Refreshes path visuals
        }

        //  Tutorial
        ProgressController.pControll.OnPathNodeAdded();
    }

    public void FinishPath()
    {
        GameManager.gm.gState = GAME_STATE.PLAY;
        //  TO DO:  maybe should open this car's menu?

        if (!CheckPath(newPath)) //  If the path is invalid (Should be prevented on AddPathNode stage
        {
            newPath = null;
            GameManager.gm.PopUp("This should never happen,\nbut there's something wrong here!");
            //  TO DO: Add sound effect
            return;
        }

        path = newPath;
        destination = path[path.Count - 1].gameObject;
        newPath = null;
        ResetToHomeCity();
        GameObject newpopupobject = Instantiate(PathCreatedTextPrefab, destination.transform.position, Quaternion.identity);
        newpopupobject.transform.localScale = new Vector3(3, 3, 1);

        if(!CheckPath())    //  Just another check, why not?    (Should'n trigger)
        {
            GameManager.gm.PopUp("This should never happen,\nbut there's something wrong here 2!");
            Debug.LogError("Oh no, you messed up the path!");
        }

        //  Tutorial
        ProgressController.pControll.OnPathFinish();
    }

    void ResetToHomeCity()  //  This function teleports the car to its hub and reset data
    {
        nextNode = 0;                                       //  Reset path following order
        transform.position = homeCity.transform.position;   //  Teleport home
        load = 0;                                           //  Empty the trunk
    }

    public float GetDistanceFrom(Vector3 fromWhere)
    {
        return Vector3.Distance(transform.position, fromWhere);
    }
    public float GetDistanceFromHome()
    {
        return Vector3.Distance(transform.position, homeCity.transform.position);
    }

    public City GetClosestCity()
    {
        City closest = homeCity.GetComponent<City>();   //  By default home is the closest
        float dist = GetDistanceFromHome();             //  Find distance
            
        foreach (City c in GameManager.gm.cities)       //  Loop thru all cities
        {
            float newDist = GetDistanceFrom(c.transform.position);  //  Remeber distance
            if (newDist < dist)                                     //  Compare
            {
                closest = c;                                        //  Update with the new closest
                dist = newDist;
            }
        }
            
        return closest;
    }
}
