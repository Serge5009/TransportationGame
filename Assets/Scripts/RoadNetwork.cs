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

    public float maxRoadLenght = 5.0f;

    void Awake()
    {
        if (rn != null && rn != this)
            Destroy(this);
        else
            rn = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
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
}
