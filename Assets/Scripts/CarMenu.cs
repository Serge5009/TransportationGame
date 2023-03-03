using UnityEngine;
using TMPro;

public class CarMenu : MonoBehaviour
{
    public Car selectedCar;

    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text load;
    [SerializeField] TMP_Text home;
    [SerializeField] TMP_Text distance;

    private void OnEnable()
    {
        selectedCar = GameManager.gm.selectedCar;
        if(!selectedCar)
        {
            Debug.LogError("CarMenu couldn't find a selectedCar");
            GameManager.gm.PopUp("CarMenu couldn't find a selectedCar");
        }
    }

    void Update()
    {
        title.text = "Car #" + selectedCar.carID.ToString();
        load.text = selectedCar.load.ToString() + " / " + selectedCar.capacity.ToString();  //  Update load of the car
        home.text = selectedCar.homeCity.GetComponent<City>().cityName;
        distance.text = selectedCar.GetDistanceFromHome().ToString("F2") + "km";
    }

    public void Close()
    {
        GameManager.gm.DeselectCar();
        // TO DO: Add sound effects
    }

    public void NewRoute()
    {
        selectedCar.CreatePath();

        //  TO DO: Add sound effect
    }
}
