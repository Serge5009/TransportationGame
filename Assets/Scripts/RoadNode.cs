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

        if (Input.GetMouseButtonDown(0))    //  Click monitor   //  TO DO: move all controls to camera controller
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(transform.position, clickPos) <= 1)     //  TO DO: new selection logic might be needed later
                OnNodeClick();
        }
    }

    public void ResetConnections()
    {
        connections.Clear();   

        //  TO DO:  Make it delete roads
    }

    void OnNodeClick()
    {
        //  Connections:
        if (GameManager.gm.gState == GAME_STATE.PLAY || GameManager.gm.gState == GAME_STATE.CONNECT)    
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

        //  Path Building
        if (GameManager.gm.gState == GAME_STATE.PATH)
        {
            Car.newPathAssociatedCar.AddPathNode(this);
        }
    }

    public void ConnectFromThis()   //  Starts connect mode and selects this node
    {                                   
        rNet.activeForConnection = this;

        //  Tutorial
        ProgressController.pControll.OnConnectModeEnter();

        GameManager.gm.DeselectCity();  //  Remove city selection
        GameManager.gm.gState = GAME_STATE.CONNECT;         //  TO DO:  should be moved to a GameManager

    }

    void ConnectToThis()            //  Final step of connection between actibe node and this
    {
        float connectionDistance = Vector2.Distance(transform.position, rNet.activeForConnection.transform.position);
        float price = GameManager.gm.baseRoadPrice * connectionDistance;

        if (GameManager.gm.money < price)                                                        //  If not enough money - return
        {
            GameManager.gm.PopUp("You need at least $" + price + "\nto build this connection!");
            return;
        }
        GameManager.gm.TakeMoney(price);

        if (connectionDistance <= rNet.maxRoadLenght) //  If distance is fine
        {
            AddConnection(rNet.activeForConnection);    //  Connect 2 nodes

            //  Tutorial
            ProgressController.pControll.OnNodeConnectSuccess();
        }
        else
        {
            GameManager.gm.PopUp("This road is too long, \ntry adding more nodes!");    //  TO DO: maybe replace with a small floating text?

            //  Tutorial
            ProgressController.pControll.OnNodeConnectFail();
        }

        //  TO DO: add cost
        rNet.activeForConnection = null;
        GameManager.gm.DeselectCity();  //  Remove city selection
        GameManager.gm.gState = GAME_STATE.PLAY;
    }

}
