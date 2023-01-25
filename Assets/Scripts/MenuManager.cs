using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuMgr { get; private set; } //  Singleton for this manager
    void Awake()
    {
        if (menuMgr != null && menuMgr != this)
            Destroy(this);
        else
            menuMgr = this;
    }

    public GameObject cityMenu;
    public GameObject carMenu;
    public GameObject carList;

    void Start()
    {
        if (!cityMenu)
            Debug.LogWarning("No cityMenu assigned to the MenuManager");
        if (!carMenu)
            Debug.LogWarning("No carMenu assigned to the MenuManager");
        if (!carList)
            Debug.LogWarning("No carList assigned to the MenuManager");
    }

    void Update()
    {
        
    }
}
