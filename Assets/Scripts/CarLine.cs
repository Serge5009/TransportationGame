using UnityEngine;
using TMPro;

public class CarLine : MonoBehaviour
{
    public Car assignedCar;
    [SerializeField] TMP_Text CarID;
    [SerializeField] TMP_Text LocationText;
    //  TO DO: Add more text fields

    void Start()
    {
        if (!assignedCar)
            Debug.LogWarning("No assignedCar found!");

        CarID.text = "Car #" + assignedCar.carID;
    }

    void Update()
    {
        bool isInCity = (assignedCar.GetDistanceFrom(assignedCar.GetClosestCity().transform.position) < assignedCar.interactDistance);
        bool isInHUB = (isInCity && assignedCar.GetClosestCity() == assignedCar.homeCity.GetComponent<City>());

        if(isInHUB)
        {
            if (!assignedCar.destination)                   //  If car needs a new route
                LocationText.text = "Waiting in the HUB";
            else                                            //  If just passing by
                LocationText.text = "Nearby";
        }
        else if (isInCity)
        {
            LocationText.text = "In " + assignedCar.GetClosestCity().cityName + " (" + assignedCar.GetDistanceFromHome().ToString("F2") + "km)";
        }
        else
        {
            LocationText.text = "On the road (" + assignedCar.GetDistanceFromHome().ToString("F2") + "km)";
        }
        
    }

    public void OpenCarMenu()
    {
        GameManager.gm.DeselectCity();
        MenuManager.menuMgr.carList.GetComponent<CarList>().Close();
        GameManager.gm.SelectCar(assignedCar);
    }
}
