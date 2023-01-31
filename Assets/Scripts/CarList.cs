using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CarList : MonoBehaviour
{
    City selectedCity;
    [SerializeField] TMP_Text cityName;
    [SerializeField] TMP_Text cityCapacity;
    [SerializeField] GameObject CarLinePrefab;
    [SerializeField] GameObject ListContainer;

    List<GameObject> spawnedLines;  //  This keeps track of all UI lines to clear them later

    private void OnEnable()
    {
        selectedCity = GameManager.gm.selectedCity;

        spawnedLines = new List<GameObject>();

        PopulateList();

    }

    void Update()
    {
        cityName.text = "Cars in " + selectedCity.cityName;
        cityCapacity.text = selectedCity.assignedCars.Count.ToString() + "/" + selectedCity.maxCarCapacity.ToString();

        if(spawnedLines.Count != selectedCity.assignedCars.Count)   //  If there's a change with cars in this city - refresh
        {
            RemoveLines();
            PopulateList();
        }
    }

    void PopulateList()
    {
        foreach(Car c in selectedCity.assignedCars)
        {
            AddCarLine(c);
        }
    }

    void AddCarLine(Car car)
    {
        GameObject newLine = Instantiate(CarLinePrefab);        //  Spawn the line
        newLine.transform.SetParent(ListContainer.transform);   //  Setting the proper hierarchy 
        newLine.transform.localScale = new Vector3(1, 1, 1);    //  Rescaling to 1 (for some reason sets scale to 0.97 by default)
        spawnedLines.Add(newLine);                              //  Add to the list
        newLine.GetComponent<CarLine>().assignedCar = car;
    }

    void RemoveLines()
    {
        foreach (GameObject i in spawnedLines)              //  Clear the old lines
            Destroy(i);
    }

    public void Close()
    {
        RemoveLines();
        GameManager.gm.DeselectCity();
        gameObject.SetActive(false);    //  TO DO: needs a new logic and a menu manager
        // TO DO: Add sound effects
    }

    //void BackToMenu()     //  TO DO

}
