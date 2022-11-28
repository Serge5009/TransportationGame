using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject homeCity;
    public GameObject destination;
    public float speed = 5.0f;

    public int capacity = 10;
    public int load = 0;

    [SerializeField] float interactDistance = 1.0f;

    void Start()
    {
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f); //  Keep the car on z=0

        if (!homeCity)
            Debug.LogError("No homeCity found");

        if (!destination)
            Debug.LogError("No destination found");

        transform.position += (destination.transform.position - transform.position).normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, homeCity.transform.position) <= interactDistance )     //  If within range with home city will try to load more
        {
            while (load < capacity && homeCity.GetComponent<City>().passengers > 0)
            {
                load++;
                homeCity.GetComponent<City>().passengers--;
            }
        }
    }
}
