using UnityEngine;
using TMPro;

public class CarList : MonoBehaviour
{
    City selectedCity;
    [SerializeField] TMP_Text cityName;


    private void OnEnable()
    {
        selectedCity = GameManager.gm.selectedCity;
        cityName.text = "Cars in " + selectedCity.cityName;
    }

    public void Close()
    {
        GameManager.gm.DeselectCity();
        gameObject.SetActive(false);    //  TO DO: needs a new logic and a menu manager
        // TO DO: Add sound effects
    }

    //void BackToMenu()     //  TO DO

}
