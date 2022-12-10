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

    }

    void Update()
    {
        moneyText.text = money.ToString();

        cityMenuUI.SetActive(selectedCity); //  If there's a selected city - activate UI menu
    }

    public void SelectCity(City newSelected)
    {
        selectedCity = newSelected;
    }
}
