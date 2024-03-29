using System.Collections.Generic;
using UnityEngine;
using TMPro;

//  ! SINGLETON !

public class GameManager : MonoBehaviour
{
    public static GameManager gm { get; private set; }  //  Singleton for the Game Manager
    void Awake()
    {
        if (gm != null && gm != this)
            Destroy(this);
        else
            gm = this;
    }

    //  UI references   //  TO DO: probably would be nice to create a manager for them
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] GameObject UIBuildEffect;
    [SerializeField] GameObject UIConnectEffect;
    [SerializeField] GameObject UIPathEffect;
    [SerializeField] GameObject UIPopUp;

    //  Active object references
    public City selectedCity;
    public Car selectedCar;

    //  Lists
    public List<City> cities;
    public List<Car> cars;

    //  Prefabs     //  TO DO: probably would be nice to create a manager for them as well
    [SerializeField] GameObject moneyChangePrefab;

    //  Gameplay basic settings
    public float defaultCarCost = 100.0f;

    //  Defaults
    [SerializeField] float defaultMoney = 5000;

    //  Resources variables
    public float money { get; private set; }

    //  Terribly implemented FSM    :)
    public GAME_STATE gState;


    void Start()
    {
        //  Error checks
        if (!moneyText)
            Debug.LogError("No moneyText assigned to the GameManager");
        if (!fpsText)
            Debug.LogError("No fpsText assigned to the GameManager");
        if (!UIBuildEffect)
            Debug.LogError("No UIBuildEffect assigned to the GameManager");
        if (!UIConnectEffect)
            Debug.LogError("No UIConnectEffect assigned to the GameManager");
        if (!UIPathEffect)
            Debug.LogError("No UIPathEffect assigned to the GameManager");
        if (!UIPopUp)
            Debug.LogError("No UIPopUp assigned to the GameManager");

        //  Add all existing cities to the list
        cities = new List<City>();
        List<GameObject> CityObjs = new List<GameObject>();
        CityObjs.AddRange(GameObject.FindGameObjectsWithTag("City"));
        foreach (GameObject o in CityObjs)
            cities.Add(o.GetComponent<City>());
        Debug.Log("GameManager found " + cities.Count + " cities on the map.");

        //  Error check 2
        if (cities.Count <= 0)
            Debug.LogWarning("GM couldn't find any cities!");

        //  Add all existing cars to the list
        cars = new List<Car>();
        List<GameObject> CarObjs = new List<GameObject>();
        CarObjs.AddRange(GameObject.FindGameObjectsWithTag("Car"));
        foreach (GameObject o in CarObjs)
            cars.Add(o.GetComponent<Car>());
        Debug.Log("GameManager found " + cars.Count + " cars on the map.");

        gState = GAME_STATE.PLAY;
    }

    void Update()
    {
        //  Error checks
        if (gState >= GAME_STATE.NUM_GAME_STATE)
            Debug.LogError("FSM Error!");

        //  UI update
        moneyText.text = "$" + money.ToString("F0");
        UpdateFPS();

        UIBuildEffect.SetActive(gState == GAME_STATE.BUILD);        //  Build mode
        UIConnectEffect.SetActive(gState == GAME_STATE.CONNECT);    //  Connect mode
        UIPathEffect.SetActive(gState == GAME_STATE.PATH);          //  Path mode

        MenuManager.menuMgr.carMenu.SetActive(selectedCar);     //  If there's a selected car - activate UI menu
    }

    public void StartNewGame()
    {
        Debug.Log("Starting game in normal mode");

        //  Resources
        money = defaultMoney;

        //  Cities and cars
        foreach(City c in cities)
        {
            c.ResetCity(true);  //  Reset all cities including assigned cars
        }
        Car.ResetCars();

        //  Roads
        RoadNetwork.rn.DeleteWholeNetwork();

        //  Game Settings
        gState = GAME_STATE.PLAY;
        DeselectCar();
        DeselectCity();

        //  Camera settings     //  TO DO:  work on it
    }

    public void SelectCity(City newSelected)
    {
        selectedCity = newSelected;
        foreach (City c in cities)
        {   //  TO DO: i'm too lazy to optimize this now)
            if (c == newSelected)
                c.isSelected = true;
            else
                c.isSelected = false;
        }

        MenuManager.menuMgr.cityMenu.SetActive(false);    //  TO DO: must be a better way to implement this
        MenuManager.menuMgr.cityMenu.SetActive(true);     //  rn is switching the object off and on to call its OnEnable function and update selected city

        gState = GAME_STATE.INMENU;

        //  Tutorial
        ProgressController.pControll.OnCitySelect(newSelected);
    }
    public void DeselectCity()
    {
        MenuManager.menuMgr.cityMenu.SetActive(false);
        selectedCity = null;
        foreach (City c in cities)
            c.isSelected = false;

        gState = GAME_STATE.PLAY;

        //  Tutorial
        ProgressController.pControll.OnCityDeSelect();
    }

    public void SelectCar(Car newSelected)
    {
        selectedCar = newSelected;  
        foreach (Car c in cars)     //  Change isSelected for each car
            c.isSelected = (c == newSelected);

        gState = GAME_STATE.INMENU;

        //  Tutorial
        ProgressController.pControll.OnCarSelect(newSelected);
    }
    public void DeselectCar()
    {
        selectedCar = null;
        foreach (Car c in cars)
            c.isSelected = false;

        gState = GAME_STATE.PLAY;

        //  Tutorial
        ProgressController.pControll.OnCarDeSelect();
    }

    public void BuildingMode()
    {
        gState = GAME_STATE.BUILD;

        //  Tutorial
        ProgressController.pControll.OnBuildModeEnter();
    }

    public void PopUp(string content)
    {
        Debug.Log("Spawned a PopUp with a following message:\n" + content);
        UIPopUp.SetActive(true);
        TextMeshProUGUI popupText = UIPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        popupText.text = content;
    }

    public bool TakeMoney(float amount, bool isShowingAnimation = true)
    {
        bool isEnoughMoney = true;

        if (amount > money)
        {
            Debug.LogError("Tried to take more money than owned");
            isEnoughMoney = false;
        }

        money -= amount;
        if(isShowingAnimation)
        {
            GameObject changeObj = Instantiate(moneyChangePrefab, moneyText.transform.position, Quaternion.identity);
            changeObj.transform.SetParent(moneyText.transform.parent);
            changeObj.GetComponent<TextMeshProUGUI>().text = "-$" + amount.ToString("F0");
        }

        return isEnoughMoney;
    }

    public void SetMoney(float amount)  //  Should only be used by Progress Manager
    {
        money = amount;
    }

    float fpsTimer = 0.0f;
    int fpsCounter = 0;
    void UpdateFPS()    //  This littrally counts number of frames every second
    {
                                        //  Every update
        fpsTimer += Time.deltaTime;     //  Timer
        fpsCounter++;                   //  +1 frame

        if(fpsTimer >= 1.0f)            //  Every second
        {
            fpsText.text = fpsCounter.ToString() + "fps";   //  Output

            fpsTimer -= 1.0f;                               //  Remove a second from the timer
            fpsCounter = 0;                                 //  Reset counter
        }

    }
}

public enum GAME_STATE
{
    PLAY,
    BUILD,
    CONNECT,
    PATH,
    INMENU,

    NUM_GAME_STATE
}