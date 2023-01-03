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
        if(other == this)
        {
            GameManager.gm.PopUp("You can't connect this node to itself!");
            return;
        }

        if (!connections.Contains(other))    //  If this node doesn't list other in connections - add
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

        //  Connections:
        if (Input.GetMouseButtonDown(0) && GameManager.gm.gState == GAME_STATE.PLAY || Input.GetMouseButtonDown(0) && GameManager.gm.gState == GAME_STATE.CONNECT)    //  Click monitor   //  TO DO: move all controls to camera controller
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(transform.position, clickPos) <= 1)     //  TO DO: new selection logic might be needed later
            {
                if (rNet.activeForConnection)    //  If first node for connection is selected
                {
                    ConnectToThis();
                }
                else                            //  If not in connection mode
                {
                    if (gameObject.GetComponent<City>() != null)    //  If this node is a city
                        return;                                     //  Don't select
                    ConnectFromThis();                              //  Else - start connecting
                }
                //TO DO: add sound effects
            }
        }

    }

    public void ConnectFromThis()   //  Starts connect mode and selects this node
    {                                   
        rNet.activeForConnection = this;                
        GameManager.gm.DeselectCity();  //  Remove city selection
        GameManager.gm.gState = GAME_STATE.CONNECT;

    }

    void ConnectToThis()            //  Final step of connection between actibe node and this
    {
        AddConnection(rNet.activeForConnection);    //  Connect 2 nodes
                                                    //  TO DO: add cost
        rNet.activeForConnection = null;
        GameManager.gm.DeselectCity();  //  Remove city selection
        GameManager.gm.gState = GAME_STATE.PLAY;
    }

}
