//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//  This script is attached to ONE UI object that player uses to interact with any city


public class CityMenu : MonoBehaviour
{
    public City selectedCity;

    [SerializeField] TMP_Text cityName;

    void OnEnable()
    {
        selectedCity = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().selectedCity;    //  TO DO: change to singleton
        if (!selectedCity)
        {
            Debug.LogError("CityMenu couldn't find a selectedCity");
            cityName.text = "ERROR 404!";   //  City not found)
        }

        cityName.text = selectedCity.name;
    }

    public void Buy()
    {
        selectedCity.BuyCity();
        // TO DO: Add sound effects
    }

    public void Close()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().DeselectCity();    //  TO DO: change to singleton
        // TO DO: Add sound effects
    }
}
