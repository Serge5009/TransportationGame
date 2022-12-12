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


        float newX = (ends[0].transform.position.x + ends[1].transform.position.x) / 2;
        float newY = (ends[0].transform.position.y + ends[1].transform.position.y) / 2;
        transform.position = new Vector2(newX, newY);

        //  Making the road face it's nodes
        transform.LookAt(ends[0].transform.position);
        transform.Rotate(0.0f, 90.0f, 90.0f);
    }

}
