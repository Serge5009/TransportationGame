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
    [SerializeField] TMP_Text hubText;
    [SerializeField] TMP_Text hubButtonText;

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

        if (!selectedCity.isAccessed)
        {
            hubText.text = "You don't have access to this city, \nbuy access for: " + selectedCity.priceToAccess;
            hubButtonText.text = "Buy";
        }
        else if(!selectedCity.isOwned)
        {
            hubText.text = "You can sell here, but can't buy vehicles, \nbuy a hub for: " + selectedCity.priceToOwn;
            hubButtonText.text = "Buy";
        }
        else
        {
            hubText.text = "You can start routs here, \naccess your hub: ";
            hubButtonText.text = "Hub";
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
            GameManager.gm.PopUp("Not implemented!");
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
}
