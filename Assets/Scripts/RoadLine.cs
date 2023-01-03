using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to every road segment spawned by each pair of nodes

public class RoadLine : MonoBehaviour
{
    RoadNetwork rNet;

    public List<RoadNode> ends;

    public float roadLenght;

    void Start()
    {
        rNet = RoadNetwork.rn;              //  Saving a refereence to the RoadNetwork singletone
        rNet.roads.Add(this);               //  Adding this rode to a list
        rNet.RoadDuplicateCheck(this);      //  Removing duplicating roads
        transform.parent = rNet.transform;  //  Setting this road as a child of RoadNetwork

        if (ends.Count != 2)                //  Another useless check
        {
            Debug.LogError("This road got an unusual number of ends -_- ");
            GameManager.gm.PopUp("ERROR! Something happened to one of your roads -_-");
            Destroy(gameObject);
        }

        roadLenght = Vector2.Distance(ends[0].transform.position, ends[1].transform.position);  //  Checking the length
        if (roadLenght > rNet.maxRoadLenght)
            Debug.LogWarning("There's a long road!");

        float newX = (ends[0].transform.position.x + ends[1].transform.position.x) / 2;
        float newY = (ends[0].transform.position.y + ends[1].transform.position.y) / 2;
        transform.position = new Vector2(newX, newY);

        //  Scaling the road
        float endsDistance = Vector2.Distance(ends[0].transform.position, ends[1].transform.position);
        float scaleReduce = 0.1f;   //  Leave a gap
        transform.localScale = new Vector3(0.1f, endsDistance - scaleReduce, 1.0f);

        //  Making the road face it's nodes
        transform.LookAt(ends[0].transform.position);
        transform.Rotate(0.0f, 90.0f, 90.0f);

    }

}
