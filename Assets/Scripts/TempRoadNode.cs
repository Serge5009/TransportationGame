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
    }

    void Update()
    {
        
    }
}
