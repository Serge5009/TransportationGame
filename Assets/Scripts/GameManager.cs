using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    public City selectedCity;
    [SerializeField] GameObject cityMenuUI;
    List<City> cities;

    public int money = 1000;

    void Start()
    {
        if (!moneyText)
            Debug.LogError("No moneyText assigned to the GameManager");
        if (!cityMenuUI)
            Debug.LogError("No cityMenuUI assigned to the GameManager");

        //  Add all existing cities to the list
        cities = new List<City>();          
        List<GameObject> CityObjs = new List<GameObject>();
        CityObjs.AddRange(GameObject.FindGameObjectsWithTag("City"));
        foreach (GameObject o in CityObjs)
            cities.Add(o.GetComponent<City>());
        Debug.Log("GameManager found " + cities.Count + " cities on the map.");

        if (cities.Count <= 0)
            Debug.LogWarning("GM couldn't find any cities!");
    }

    void Update()
    {
        moneyText.text = money.ToString();

        cityMenuUI.SetActive(selectedCity); //  If there's a selected city - activate UI menu
    }

    public void SelectCity(City newSelected)
    {
        selectedCity = newSelected;
        foreach (City c in cities)
        {   //  TO DO: i'm too lazy to optimize now)
            if (c == newSelected)
                c.isSelected = true;
            else
                c.isSelected = false;
        }
    }

    public void DeselectCity()
    {
        selectedCity = null;
        foreach (City c in cities)
            c.isSelected = false;
    }
}
