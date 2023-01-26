using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
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
    [SerializeField] GameObject UIBuildEffect;
    [SerializeField] GameObject UIConnectEffect;
    [SerializeField] GameObject UIPathEffect;
    [SerializeField] GameObject UIPopUp;

    //  Active object references
    public City selectedCity;
    public Car selectedCar;

    //  Lists
    List<City> cities;
    public List<Car> cars;

    //  Prefabs     //  TO DO: probably would be nice to create a manager for them as well
    [SerializeField] GameObject roadNodePrefab;

    //  Gameplay basic settings
    public float roadNodeCost = 50.0f;

    //  Resources
    public float money = 1000.0f;

    //  Terribly implemented FSM    :)
    public GAME_STATE gState;


    void Start()
    {
        //  Error checks
        if (!moneyText)
            Debug.LogError("No moneyText assigned to the GameManager");
        if (!roadNodePrefab)
            Debug.LogError("No roadNodePrefab assigned to the GameManager");
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
        moneyText.text = money.ToString();

        UIBuildEffect.SetActive(gState == GAME_STATE.BUILD);        //  Build mode
        UIConnectEffect.SetActive(gState == GAME_STATE.CONNECT);    //  Connect mode
        UIPathEffect.SetActive(gState == GAME_STATE.PATH);          //  Path mode

        MenuManager.menuMgr.carMenu.SetActive(selectedCar);     //  If there's a selected car - activate UI menu
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
    }
    public void DeselectCity()
    {
        MenuManager.menuMgr.cityMenu.SetActive(false);
        selectedCity = null;
        foreach (City c in cities)
            c.isSelected = false;

        gState = GAME_STATE.PLAY;
    }

    public void SelectCar(Car newSelected)
    {
        selectedCar = newSelected;
        foreach (Car c in cars)
        {   //  TO DO: i'm too lazy to optimize this now)
            if (c == newSelected)
                c.isSelected = true;
            else
                c.isSelected = false;
        }

        MenuManager.menuMgr.carMenu.SetActive(false);    //  TO DO: must be a better way to implement this
        MenuManager.menuMgr.carMenu.SetActive(true);     //  rn is switching the object off and on to call its OnEnable function and update selected city

        gState = GAME_STATE.INMENU;
    }
    public void DeselectCar()
    {
        selectedCar = null;
        foreach (Car c in cars)
            c.isSelected = false;

        gState = GAME_STATE.PLAY;
    }

    public void BuildingMode()
    {
        gState = GAME_STATE.BUILD;
    }

    public void AddRoadNode(Vector2 placement)
    {
        if(money < roadNodeCost)
        {
            //  TO DO: Show an error message
            return;
        }
        money -= roadNodeCost;
        GameObject newRoad = Instantiate(roadNodePrefab, placement, Quaternion.identity);
        gState = GAME_STATE.PLAY;
    }

    public void PopUp(string content)
    {
        Debug.Log("Spawned a PopUp with a following message:\n" + content);
        UIPopUp.SetActive(true);
        TextMeshProUGUI popupText = UIPopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        popupText.text = content;
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