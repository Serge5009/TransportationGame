using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

//  ! SINGLETON !

public class GameManager : MonoBehaviour
{
    public static GameManager gm { get; private set; }

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] GameObject UIBuildEffect;
    public City selectedCity;
    [SerializeField] GameObject cityMenuUI;
    List<City> cities;
    [SerializeField] GameObject roadNodePrefab;

    //  Gameplay settings
    public float roadNodeCost = 50.0f;


    public float money = 1000.0f;

    public GAME_STATE gState;   //  Sorta FSM for the game

    void Awake()
    {
        if (gm != null && gm != this)
            Destroy(this);
        else
            gm = this;
    }

    void Start()
    {
        if (!moneyText)
            Debug.LogError("No moneyText assigned to the GameManager");
        if (!cityMenuUI)
            Debug.LogError("No cityMenuUI assigned to the GameManager");
        if (!roadNodePrefab)
            Debug.LogError("No roadNodePrefab assigned to the GameManager");
        if (!UIBuildEffect)
            Debug.LogError("No UIBuildEffect assigned to the GameManager");

        //  Add all existing cities to the list
        cities = new List<City>();
        List<GameObject> CityObjs = new List<GameObject>();
        CityObjs.AddRange(GameObject.FindGameObjectsWithTag("City"));
        foreach (GameObject o in CityObjs)
            cities.Add(o.GetComponent<City>());
        Debug.Log("GameManager found " + cities.Count + " cities on the map.");

        if (cities.Count <= 0)
            Debug.LogWarning("GM couldn't find any cities!");

        gState = GAME_STATE.PLAY;
    }

    void Update()
    {
        moneyText.text = money.ToString();

        cityMenuUI.SetActive(selectedCity); //  If there's a selected city - activate UI menu

        if (gState >= GAME_STATE.NUM_GAME_STATE)
            Debug.LogError("FSM Error!");

        UIBuildEffect.SetActive(gState == GAME_STATE.BUILD);    //  If in build mode - will show UI effect
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

        cityMenuUI.SetActive(false);    //  TO DO: must be a better way to implement this
        cityMenuUI.SetActive(true);     //  rn is switching the object off and on to call its OnEnable function and update selected city
    }

    public void DeselectCity()
    {
        selectedCity = null;
        foreach (City c in cities)
            c.isSelected = false;
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

    public void ConnectRoadNodes()
    {

    }
}

public enum GAME_STATE
{
    PLAY,
    PAUSE,
    BUILD,
    CONNECT,
    SLOW,
    FAST,

    NUM_GAME_STATE
}