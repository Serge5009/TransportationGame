using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ! SINGLETON !
//  This script must be attached to ONE object that contains the information about all roads and nodes
public class RoadNetwork : MonoBehaviour
{
    public static RoadNetwork rn { get; private set; }

    public List<RoadNode> nodes;
    public List<RoadLine> roads;

    public RoadNode activeForConnection = null;

    //  Settings
    public float roadNodeCost = 50.0f;
    public float baseRoadPrice = 10.0f;
    public float maxRoadLenght = 5.0f;


    //  Prefabs
    [SerializeField] GameObject roadNodePrefab;
    [SerializeField] GameObject tempRoadNodePrefab;

    void Awake()
    {
        if (rn != null && rn != this)
            Destroy(this);
        else
            rn = this;
    }

    public void PlaceTempNode(Vector2 placement)
    {
        Instantiate(tempRoadNodePrefab, placement, Quaternion.identity);
        GameManager.gm.gState = GAME_STATE.PLAY;
    }

    public void AddRoadNode(Vector2 placement)
    {
        if (GameManager.gm.money < roadNodeCost)
        {
            //  TO DO: Show an error message
            return;
        }

        GameManager.gm.TakeMoney(roadNodeCost);
        GameObject newRoad = Instantiate(roadNodePrefab, placement, Quaternion.identity);
        GameManager.gm.gState = GAME_STATE.PLAY;

        //  Tutorial
        ProgressController.pControll.OnNodeBuilt();
    }

    public void DeleteWholeNetwork()
    {
        foreach (RoadLine r in roads)
            Destroy(r.gameObject);                  //  Destroy all roads

        foreach (RoadNode n in nodes)
        {
            n.ResetConnections();                   //  Remove all connections in nodes (doesn't affect roads)
        }
        foreach (RoadNode n in nodes)
        {
            if (!n.gameObject.GetComponent<City>())  //  Unless it's a city
            {
                Destroy(n.gameObject);              //  Delete the node
            }
        }    
    }

    public void RoadDuplicateCheck(RoadLine toCheck)    //  Called by each segment when spawned, deletes duplicate segments
    {
        RoadNode t0 = toCheck.ends[0];
        RoadNode t1 = toCheck.ends[1];

        foreach (RoadLine r in roads)
        {
            if (r == toCheck)   //  No need to compare with itself
                continue;

            RoadNode r0 = r.ends[0];
            RoadNode r1 = r.ends[1];

            int similarNodes = 0;

            if (t0 == r0 || t0 == r1)
                similarNodes++;
            if (t1 == r0 || t1 == r1)
                similarNodes++;

            if (similarNodes == 2)
            {
                roads.Remove(toCheck);
                Destroy(toCheck.gameObject);
                Debug.Log("Duplicate road was destroyed");
                return;
            }
            else if (similarNodes > 2)
                Debug.LogError("What the...?");
        }
    }

    public static bool DoesRoadExistBetween(RoadNode n0, RoadNode n1)
    {
        foreach (RoadLine line in RoadNetwork.rn.roads)                            //  Loop thru all roads
            if(line.ends.Contains(n0) && line.ends.Contains(n1))    //  If road had both nodes among its ends
                return true;                                        //  Return true

        return false;                                              //  Else - return false
    }
}
