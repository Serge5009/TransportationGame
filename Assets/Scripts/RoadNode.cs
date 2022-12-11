using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to every node of the road network and to each city
public class RoadNode : MonoBehaviour
{
    public List<RoadNode> connections;

    RoadNetwork rNet;

    void Start()
    {
        rNet = GameObject.FindGameObjectWithTag("RoadNetwork").GetComponent<RoadNetwork>();

        foreach (RoadNode n in connections) //  Loop thru all connected nodes and make sure that they're aware about the connection
        {
            bool isThisInConnections = false;
            foreach (RoadNode i in n.connections)
            {
                if (i == this)
                    isThisInConnections = true;
            }
            if (!isThisInConnections)
                n.connections.Add(this);
        }

        rNet.nodes.Add(this);
    }

    void Update()
    {
        
    }
}
