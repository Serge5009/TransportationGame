using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
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
    [SerializeField] float interactDistance = 25.0f;
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
            if (isNear(homeCity))     //  If within range with home city will try to load more
            {
                if (!isMovingTo)
                    UnloadTo(homeCity.GetComponent<City>());
                isMovingTo = true;
            }
            if (isNear(destination))     //  If within range with destination will try to unload
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

    void UnloadTo(City toUnload)
    {
        if (load == 0)
            return;

        GameObject newText = Instantiate(FlyingTextPrefab, transform.position, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().text = load.ToString();
        newText.GetComponent<TextMeshPro>().color = Color.yellow;

        parkedTimer += load / 10;       //  Take some time to unload

        GameManager.gm.money += load;                                   //  TO DO: make money logic more ineresting
        load = 0;
    }

    void LoadFrom(City toLoad)
    {
        while (load < capacity && toLoad.passengers > 0)
        {
            load++;
            toLoad.passengers--;                                        //  TO DO: this seems like it doesn't work
            parkedTimer += 0.1f;    //  Take some time to load

            /*  //  Pop up text, looks bad  //  TO DO: make it nice!
            GameObject newText = Instantiate(FlyingTextPrefab, transform.position, Quaternion.identity);
            newText.GetComponent<TextMeshPro>().text = "1";
            newText.GetComponent<TextMeshPro>().color = Color.green;
            MiniPopup textScript = newText.GetComponent<MiniPopup>();
            textScript.sideShakeIntens = 0.0f;
            textScript.maxSpeed = 3.0f;
            textScript.slowDown = 0.3f;
            */
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

        if(!CheckPath(newPath))
        {
            newPath.Remove(nodeToAdd);
            //  TO DO: Add sound effect
        }
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
        GameManager.gm.PopUp("New path created!");

        if(!CheckPath())    //  Just another check, why not?    (Should'n trigger)
        {
            GameManager.gm.PopUp("This should never happen,\nbut there's something wrong here 2!");
            Debug.LogError("Oh no, you messed up the path!");
        }
    }

    void ResetToHomeCity()  //  This function teleports the car to its hub and reset data
    {
        nextNode = 0;                                       //  Reset path following order
        transform.position = homeCity.transform.position;   //  Teleport home
        load = 0;                                           //  Empty the trunk
    }
}
