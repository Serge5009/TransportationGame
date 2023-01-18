using UnityEngine;
using TMPro;

public class CarMenu : MonoBehaviour
{
    public Car selectedCar;

    [SerializeField] TMP_Text load;

    private void OnEnable()
    {
        selectedCar = GameManager.gm.selectedCar;
        if(!selectedCar)
        {
            Debug.LogError("CarMenu couldn't find a selectedCar");
            GameManager.gm.PopUp("CarMenu couldn't find a selectedCar");
        }

        //  ADD Load update here
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
