using System.Collections.Generic;
using UnityEngine;

//  ! SINGLETON !

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager prefMgr { get; private set; }  //  Singleton for the Prefab Manager
    void Awake()
    {
        if (prefMgr != null && prefMgr != this)
            Destroy(this);
        else
            prefMgr = this;
    }


    public GameObject carPrefab;
    public Sprite defaultCitySprite;
    public List<Sprite> sizeCitySprites;

    private void Start()
    {
        if (!carPrefab)
            Debug.LogWarning("Prefab manager could not find carPrefab");
        if (!defaultCitySprite)
            Debug.LogWarning("Prefab manager could not find defaultCitySprite");
        if(sizeCitySprites.Count == 0)
            Debug.LogWarning("Prefab manager could not find sizeCitySprites");

    }
}
