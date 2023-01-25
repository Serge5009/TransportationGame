using UnityEngine;
using TMPro;

public class CarLine : MonoBehaviour
{
    public Car assignedCar;
    [SerializeField] TMP_Text CarID;
    //  TO DO: Add more text fields

    void Start()
    {
        if (!assignedCar)
            Debug.LogWarning("No assignedCar found!");

        CarID.text = "Car #" + assignedCar.carID;
    }

    void Update()
    {
        
    }

    public void OpenCarMenu()
    {
        GameManager.gm.SelectCar(assignedCar);
        GameManager.gm.DeselectCity();
    }
}
