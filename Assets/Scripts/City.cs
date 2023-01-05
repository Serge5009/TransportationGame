//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class City : MonoBehaviour
{
    public string name;

    public int population = 0;

    [SerializeField] bool isOwned = false;      //  If true - player can buy vehicles and start routes in this city
    [SerializeField] bool isAccessed = false;   //  If true - player's routs passing thru this city will bring profit

    float priceToOwn = 100000;
    float priceToAccess = 100000;
    public int passengers = 0;

    [SerializeField] GameObject carPrefab;

    TextMeshProUGUI counter;
    [HideInInspector] public bool isSelected = false;

    void Start()
    {
        if (!carPrefab)
            Debug.LogError("No carPrefab added");
        if (!(name.Length > 3))
            Debug.LogError("No name added to the city or the name is too short");
        if (population <= 0)
            Debug.LogWarning("There's a city with no people");

        //counter = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    float timer = 0.01f;
    float tickTimer = 0.0f;
    void Update()
    {
        timer -= Time.deltaTime;    //  Car spawner
        if(timer <= 0 && isOwned)
        {
            timer = Random.Range(5.0f, 10.0f);                

            /*
            GameObject car = Instantiate(carPrefab, transform.position, Quaternion.identity);
            Car carScript = car.GetComponent<Car>();
            carScript.homeCity = this.gameObject;

            List<GameObject> Cities = new List<GameObject>(GameObject.FindGameObjectsWithTag("City"));

            foreach(GameObject c in Cities) //  Find target
            {
                if(c != this.gameObject)
                {
                    carScript.destination = c;
                    break;
                }
            }

            car.transform.SetParent(transform.parent);   //  Car should be on the canvas*/
        }

        tickTimer += Time.deltaTime;
        if(tickTimer >= 1)
        {
            Tick();
            tickTimer -= 1;
        }

        //counter.text = passengers.ToString();

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

    void Tick()
    {
        priceToOwn = population / 100;
        priceToAccess = population / 10000;

        if (Random.Range(0.0f, 1.0f) < 0.5f && isOwned)    //  Random passenger increase
        {
            passengers += Random.Range(1, 3);
        }
    }

    public void BuyCity()
    {
        if (GameManager.gm.money >= priceToOwn && !isOwned)
        {
            GameManager.gm.money -= priceToOwn;
            isOwned = true;
            GameManager.gm.DeselectCity();
        }
        else if (isOwned)
            GameManager.gm.PopUp("This city is already owned");
        else if (GameManager.gm.money < priceToOwn)
            GameManager.gm.PopUp("Not enough money");
    }
}
