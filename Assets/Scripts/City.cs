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

    float timer = 5.0f;
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 5.0f;
            GameObject car = Instantiate(carPrefab, transform.position, Quaternion.identity);
        }
    }
}
