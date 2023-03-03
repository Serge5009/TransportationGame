using UnityEngine;
using TMPro;

public class TempRoadNode : MonoBehaviour
{
    [SerializeField] GameObject radius;
    [SerializeField] TextMeshPro priceText;

    void Start()
    {
        priceText.text = "$" + RoadNetwork.rn.roadNodeCost.ToString("F0");
        float radScale = RoadNetwork.rn.maxRoadLenght * (1 / this.transform.localScale.x) * 2;  //  Calculate how much too scale the radius based on connection range and node's default scale
        radius.transform.localScale = new Vector3 (radScale, radScale, 1.0f);

        //  TO DO: add "possible connections"
    }

    void Update()
    {
        //  Click registering
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //  If clicked on the node - spawn it
            if (Vector2.Distance(transform.position, clickPos) <= 1 && GameManager.gm.gState == GAME_STATE.PLAY)     //  TO DO: new selection logic might be needed later
            {
                RoadNetwork.rn.AddRoadNode(transform.position);         
            }

            //  If clicked further - just go to normal mode

            Destroy(gameObject);
        }
    }

}
