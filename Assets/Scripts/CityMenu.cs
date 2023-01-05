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
    [SerializeField] TMP_Text population;

    void OnEnable()
    {
        selectedCity = GameManager.gm.selectedCity;
        if (!selectedCity)
        {
            Debug.LogError("CityMenu couldn't find a selectedCity");
            cityName.text = "ERROR 404!";   //  City not found)
            GameManager.gm.PopUp("ERROR! There's a city without a name");
        }

        cityName.text = selectedCity.name;
        population.text = selectedCity.population.ToString();
    }

    public void Buy()
    {
        selectedCity.BuyCity();
        // TO DO: Add sound effects
    }

    public void Close()
    {
        GameManager.gm.DeselectCity();
        // TO DO: Add sound effects
    }

    public void ConnectThis()
    {
        RoadNode node = selectedCity.gameObject.GetComponent<RoadNode>();
        node.ConnectFromThis();
    }
}
