using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    List<ISavableData> _savableItems = new List<ISavableData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSaveData()
    {
        foreach (var item in _savableItems)
        {
            item.SaveData();
        }
    }
}
