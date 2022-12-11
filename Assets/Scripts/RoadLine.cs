using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is attached to every road segment spawned by each pair of nodes

public class RoadLine : MonoBehaviour
{
    public List<RoadNode> ends;

    void Start()
    {
        
    }

    void Update()
    {
        if (ends.Count != 2)
            Debug.LogError("This road got unusual number of ends -_- ");
    }
}
