using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ! SINGLETON !

//  This scrip is in charge of spawning temporary visual objects aimed to aim the player to keep track of selected objects or routes
public class VisualsManager : MonoBehaviour
{
    public static VisualsManager visMgr { get; private set; }  //  Singleton for the Visuals Manager
    void Awake()
    {
        if (visMgr != null && visMgr != this)
            Destroy(this);
        else
            visMgr = this;
    }

    List<GameObject> spawnedVisuals;

    void Start()
    {
        spawnedVisuals = new List<GameObject>();
    }

    void Update()
    {
        
    }
}
