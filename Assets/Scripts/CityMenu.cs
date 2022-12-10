using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to ONE UI object that player uses to interact with any city


public class CityMenu : MonoBehaviour
{
    public City selectedCity;

    void Start()
    {
        selectedCity = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().selectedCity;    //  TO DO: change to singleton
        if (!selectedCity)
            Debug.LogError("CityMeny couldn't find a selectedCity");
    }

    public void Buy()
    {

    }

    public void Close()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DeselectCity();    //  TO DO: change to singleton
        // TO DO: Add sound effects
    }
}
