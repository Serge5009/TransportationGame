//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    public int passengers = 0;

    [SerializeField] GameObject carPrefab;

    TextMeshProUGUI counter;

    void Start()
    {
        if (!carPrefab)
            Debug.LogError("No carPrefab added");
        counter = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    float timer = 0.0f;
    float tickTimer = 0.0f;
    void Update()
    {
        timer -= Time.deltaTime;    //  Car spawner
        if(timer <= 0)
        {
            timer = Random.Range(5.0f, 10.0f);
            GameObject car = Instantiate(carPrefab, transform.position, Quaternion.identity);
            Car carScript = car.GetComponent<Car>();
            carScript.homeCity = this.gameObject;

            List<GameObject> Cities = new List<GameObject>(GameObject.FindGameObjectsWithTag("City"));

            foreach(GameObject c in Cities) //  Find target
            {
                if(c != this.gameObject)
                {
                    carScript.destination = c;
                    break;
                }
            }

            car.transform.SetParent(transform.parent);   //  Car should be on the canvas
        }

        tickTimer += Time.deltaTime;
        if(tickTimer >= 1)
        {
            Tick();
            tickTimer -= 1;
        }

        counter.text = passengers.ToString();
    }

    void Tick()
    {
        if (Random.Range(0.0f, 1.0f) < 0.5f)    //  Random passenger increase
        {
            passengers += Random.Range(1, 3);
        }
    }
}
