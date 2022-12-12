using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to every road segment spawned by each pair of nodes

public class RoadLine : MonoBehaviour
{
    RoadNetwork rNet;

    public List<RoadNode> ends;

    void Start()
    {
        if (ends.Count != 2)
        {
            Debug.LogError("This road got unusual number of ends -_- ");
            Destroy(gameObject);
        }

        rNet = GameObject.FindGameObjectWithTag("RoadNetwork").GetComponent<RoadNetwork>();
        rNet.roads.Add(this);
        rNet.RoadDuplicateCheck(this);
        transform.parent = rNet.transform;


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
