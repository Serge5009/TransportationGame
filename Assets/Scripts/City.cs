//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class City : MonoBehaviour
{
    public string name;

    [SerializeField] bool isOwned = false;
    [SerializeField] int price = 100;
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

        //counter = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    float timer = 3.0f;
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
            if(Vector2.Distance(transform.position, clickPos) <= 1)     //  TO DO: new selection logic might be needed later
            {
                GameManager.gm.SelectCity(this);

                //TO DO: add sound effects
            }
        }
    }

    void Tick()
    {
        if (Random.Range(0.0f, 1.0f) < 0.5f && isOwned)    //  Random passenger increase
        {
            passengers += Random.Range(1, 3);
        }
    }

    public void BuyCity()
    {
        if(GameManager.gm.money >= price && !isOwned)
        {
            GameManager.gm.money -= price;
            isOwned = true;
            GameManager.gm.DeselectCity();
        }
    }
}
