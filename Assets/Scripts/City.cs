using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public string cityName;

    [SerializeField] int defaultPopulation = 0;
    public int population;
    public bool isOwned = false;      //  If true - player can buy vehicles and start routes in this city
    public bool isAccessed = false;   //  If true - player's routs passing thru this city will bring profit
    [HideInInspector] public float priceToOwn = 100000;
    [HideInInspector] public float priceToAccess = 100000;
    public int maxCarCapacity = 10; //  TO DO: add some logic

    public int passengers = 0;
    public List<Car> assignedCars;

    [HideInInspector] public bool isSelected = false;

    void Start()
    {
        if (!(cityName.Length > 3))
            Debug.LogError("No name added to the city or the name is too short");
        if (population <= 0)
            Debug.LogWarning("There's a city with no people");

        assignedCars = new List<Car>();
        ResetCity();
    }

    float tickTimer = 0.0f;
    void Update()
    {
        tickTimer += Time.deltaTime;
        if(tickTimer >= 1)
        {
            Tick();
            tickTimer -= 1;
        }

        //  Click registering
        //  https://www.youtube.com/watch?v=5KLV6QpSAdI
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(Vector2.Distance(transform.position, clickPos) <= 1 && GameManager.gm.gState == GAME_STATE.PLAY)     //  TO DO: new selection logic might be needed later
            {
                GameManager.gm.SelectCity(this);

                //TO DO: add sound effects
            }
        }
    }

    void AdjustSprite()
    {
        //  Determining what sprite should represent the city
        Sprite toShow = PrefabManager.prefMgr.defaultCitySprite;

        if (population >= 5000000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[7];
        else if (population >= 1000000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[6];
        else if (population >= 500000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[5];
        else if (population >= 100000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[4];
        else if (population >= 50000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[3];
        else if (population >= 25000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[2];
        else if (population >= 10000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[1];
        else if (population >= 5000)
            toShow = PrefabManager.prefMgr.sizeCitySprites[0];

        //  Display the selected sprite
        this.GetComponent<SpriteRenderer>().sprite = toShow;

        //  Be default color.a is set to a low value for easier map editing. This line sets alpha back to normal
        this.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 255);

        //  Scale
        float scaleFactor = 0.2f;

        if (population >= 5000000)
            scaleFactor = 1.0f;
        else if (population >= 1000000)
            scaleFactor = 0.8f;
        else if (population >= 500000)
            scaleFactor = 0.6f;
        else if (population >= 100000)
            scaleFactor = 0.5f;
        else if (population >= 50000)
            scaleFactor = 0.4f;
        else if (population >= 25000)
            scaleFactor = 0.33f;
        else if (population >= 10000)
            scaleFactor = 0.3f;
        else if (population >= 5000)
            scaleFactor = 0.25f;

        scaleFactor /= 3;   //  TO DO:  this is a quick fix while I'm experimenting with the map

        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }

    void Tick()
    {
        priceToOwn = population / 100;
        priceToAccess = population / 10000;

        float populationFactor = population / 100000;

        if (Random.Range(0.0f, 1.0f) < 0.5f && isAccessed)    //  Random passenger increase
        {
            passengers += Random.Range(1, (int)(3 * populationFactor));
        }

        population += Random.Range(-population / 20000, population / 10000);  //  For dynamic population  //  TO DO: needs more factors
    }

    public void ResetCity(bool deleteCars = false)
    {
        population = defaultPopulation;
        isAccessed = false;
        isOwned = false;

        if(deleteCars)
            foreach(Car c in assignedCars)
            {
                Destroy(c.gameObject);
            }

        assignedCars.Clear();

        AdjustSprite();
    }

    public void BuyCityHub()
    {
        if (GameManager.gm.money >= priceToOwn)
        {
            GameManager.gm.TakeMoney(priceToOwn);
            isOwned = true;
            isAccessed = true;

            //  Tutorial
            ProgressController.pControll.OnCityHubPurchase(this);
        }
        else if (isOwned)
            GameManager.gm.PopUp("This city is already accessed");
        else if (GameManager.gm.money < priceToOwn)
            GameManager.gm.PopUp("Not enough money");
    }
    public void BuyCityAccess()
    {
        if (GameManager.gm.money >= priceToAccess)
        {
            GameManager.gm.TakeMoney(priceToAccess);
            isAccessed = true;

            //  Tutorial
            ProgressController.pControll.OnCityAccess(this);    
        }
        else if (isAccessed)
            GameManager.gm.PopUp("This city is already owned");
        else if (GameManager.gm.money < priceToAccess)
            GameManager.gm.PopUp("Not enough money");
    }

    public void BuyNewCar()
    {
        float price = GameManager.gm.defaultCarCost * (assignedCars.Count + 1);                 //  Calculate the price

        if(GameManager.gm.money < price)                                                        //  If not enough money - return
        {
            GameManager.gm.PopUp("You need at least $" + price + "\nto buy a new car here!");
            return;
        }
        GameObject carPrefab = PrefabManager.prefMgr.carPrefab;                                 //  Load the prefab
        GameManager.gm.TakeMoney(price);                                                        //  Take money
        GameObject newCarObj = Instantiate(carPrefab, transform.position, Quaternion.identity); //  Spawn a new Car
        Car newCar = newCarObj.GetComponent<Car>();                                             //  Rememver it's script
        newCar.homeCity = this.gameObject;                                                      //  Set the home base
        assignedCars.Add(newCar);                                                               //  Add our new car to the list
        GameManager.gm.cars.Add(newCar);                                                        //  Add it to GM cars list
    }
}
