using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{


    [SerializeField] GameObject carPrefab;
    void Start()
    {
        if (!carPrefab)
            Debug.LogError("No carPrefab added");
    }

    float timer = 0.0f;
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 5.0f;
            GameObject car = Instantiate(carPrefab, transform.position, Quaternion.identity);
            Car carScript = car.GetComponent<Car>();
            carScript.homeCity = this.gameObject;

            List<GameObject> Cities = new List<GameObject>(GameObject.FindGameObjectsWithTag("City"));

            foreach(GameObject c in Cities)
            {
                if(c != this.gameObject)
                {
                    carScript.destination = c;
                    break;
                }
            }

        }
    }
}
