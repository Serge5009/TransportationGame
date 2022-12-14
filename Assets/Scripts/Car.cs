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

    void Start()
    {
        if (path[0] != homeCity)
            Debug.LogWarning("Path should start with a home city");

        if (path[path.Count] != destination)
        {
            Debug.LogWarning("Path should end with a destination");
            path.Add(destination.GetComponent<RoadNode>());
        }
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
}
