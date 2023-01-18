using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class Car : MonoBehaviour
{
    public GameObject homeCity;
    public GameObject destination;
    public float speed = 20.0f;
    public int capacity = 10;
    public int load = 0;

    [SerializeField] float interactDistance = 25.0f;

    public List<RoadNode> path;
    int nextNode;
    bool isMovingTo = true;

    public bool isSelected = false;

    [HideInInspector] public static List<RoadNode> newPath;

    void Start()
    {
        CheckPath();
        nextNode = 0;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f); //  Keep the car on z=0

        if (!homeCity)
            Debug.LogError("No homeCity found");

        if (!destination)
            Debug.LogError("No destination found");

        transform.position += (path[nextNode].transform.position - transform.position).normalized * speed * Time.deltaTime;   //  Move towards the next node


        if (isNear(homeCity))     //  If within range with home city will try to load more
        {
            if (!isMovingTo)
                UnloadTo(homeCity.GetComponent<City>());

            isMovingTo = true;
            LoadFrom(homeCity.GetComponent<City>());
        }
        if (isNear(destination))     //  If within range with destination will try to unload
        {
            isMovingTo = false;
            UnloadTo(destination.GetComponent<City>());
            //LoadFrom(destination.GetComponent<City>());
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

        //  Making the car face it's direction
        transform.LookAt(path[nextNode].transform.position);
        transform.Rotate(0.0f, 90.0f, 90.0f);

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
        GameManager.gm.money += load; //  TO DO: make money logic more ineresting
        load = 0;
    }

    void LoadFrom(City toLoad)
    {
        while (load < capacity && toLoad.passengers > 0)
        {
            load++;
            toLoad.passengers--; //  TO DO: this seems like it doesn't work
        }
    }

    bool isNear(GameObject place)   //  This function allows to check if a certain object is within car's range
    {
        float distance = Vector2.Distance(transform.position, place.transform.position);
        return (distance <= interactDistance);
    }

    bool CheckPath()
    {
        bool returnVal = true;

        if (path[0].gameObject != homeCity)
        {
            Debug.LogWarning("Path should start with a home city");
            GameManager.gm.PopUp("Path should start with a home city");
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            if(!path[i].connections.Contains(path[i + 1]))
            {
                GameManager.gm.PopUp("This car has an invalid path!");
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
            GameManager.gm.PopUp("Path should start with a home city");
        }

        for (int i = 0; i < nodeList.Count - 1; i++)
        {
            if (!nodeList[i].connections.Contains(nodeList[i + 1]))
            {
                GameManager.gm.PopUp("This car has an invalid path!");
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
        newPath = new List<RoadNode>();
        newPath.Add(homeCity.GetComponent<RoadNode>());
    }
}
