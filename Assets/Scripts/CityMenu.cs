using UnityEngine;
using TMPro;

//  This script is attached to ONE UI object that player uses to interact with any city


public class CityMenu : MonoBehaviour
{
    public City selectedCity;

    [SerializeField] TMP_Text cityName;
    [SerializeField] TMP_Text population;
    [SerializeField] TMP_Text hubText;
    [SerializeField] TMP_Text hubButtonText;
    [SerializeField] TMP_Text hubUnderBuyText;
    [SerializeField] TMP_Text passengersText;
    [SerializeField] GameObject carListMenu;

    void OnEnable()
    {
        selectedCity = GameManager.gm.selectedCity;
        if (!selectedCity)
        {
            Debug.LogError("CityMenu couldn't find a selectedCity");
            cityName.text = "ERROR 404!";   //  City not found)
            GameManager.gm.PopUp("CityMenu couldn't find a selectedCity");
        }
    }

    void Update()
    {
        cityName.text = selectedCity.cityName;
        population.text = selectedCity.population.ToString();
        passengersText.text = selectedCity.passengers.ToString();

        if (!selectedCity.isAccessed)
        {
            hubText.text = "You don't have access to this city, \nVehicles won't interact when passing by";
            hubButtonText.text = "Get access";
            hubUnderBuyText.text = "$" + selectedCity.priceToAccess;
        }
        else if (!selectedCity.isOwned)
        {
            hubText.text = "Your cars load here,\nCan't buy new vehicles here";
            hubButtonText.text = "Buy a HUB";
            hubUnderBuyText.text = "$" + selectedCity.priceToOwn;
        }
        else
        {
            hubText.text = "You have a HUB here, \nYou can buy and manage vehicles";
            hubButtonText.text = "Vehicles";
            hubUnderBuyText.text = selectedCity.assignedCars.Count.ToString();
        }
    }

    public void OnHubButtonClick()
    {
        if (!selectedCity.isAccessed)
        {
            selectedCity.BuyCityAccess();
        }
        else if (!selectedCity.isOwned)
        {
            selectedCity.BuyCityHub();
        }
        else
        {
            OpenCarList();
        }
        // TO DO: Add sound effects
    }

    public void BuyCar()
    {
        if (selectedCity.isOwned)
            selectedCity.BuyNewCar();
        else
            GameManager.gm.PopUp("You need to buy a hub\nin this city first!");

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

    void OpenCarList()
    {
        carListMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
