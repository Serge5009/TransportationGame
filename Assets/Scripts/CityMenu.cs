using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to ONE UI object that player uses to interact with any city


public class CityMenu : MonoBehaviour
{
    public City activeCity;



    void Start()
    {
        if (!activeCity)
            Debug.LogError("CityMeny couldn't find an active city");
    }

    void Update()
    {
        
    }
}
