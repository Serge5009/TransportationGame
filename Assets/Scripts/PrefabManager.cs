using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
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
