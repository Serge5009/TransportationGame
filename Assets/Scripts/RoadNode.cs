using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to every node of the road network and to each city
public class RoadNode : MonoBehaviour
{
    public List<RoadNode> connections;

    RoadNetwork rNet;

    [SerializeField] GameObject roadPrefab;

    void Start()
    {
        rNet = RoadNetwork.rn;

        if (!roadPrefab)
            Debug.LogError("No roadPrefab attached");
        //  TO DO: rn i'm creating road segments in a very stupid way, because I'm spawning them from each node to each connection
        //  it leads to two segments spawnd insted of one, so after that I make Road Network to delete duplicates

        foreach (RoadNode n in connections) //  Loop thru all connected nodes and make sure that they're aware about the connection
        {
            AddConnection(n);
        }

        rNet.nodes.Add(this);
    }

    public void AddConnection(RoadNode other)
    {
        if(!connections.Contains(other))    //  If this node doesn't list other in connections - add
        {
            connections.Add(other);
        }

        bool isThisInConnections = false;   //  If other node doesn't list this in connections - add
        foreach (RoadNode i in other.connections)
        {
            if (i == this)
                isThisInConnections = true;
        }
        if (!isThisInConnections)
            other.connections.Add(this);

        //  Spawn a road between nodes
        GameObject newRoad = Instantiate(roadPrefab, transform.position, Quaternion.identity);
        RoadLine roadScript = newRoad.GetComponent<RoadLine>();
        roadScript.ends.Add(this);
        roadScript.ends.Add(other);
    }

    void Update()
    {
        
    }
}
