using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] SerializationMode _serializationType;
    [SerializeField] string _folderName;

    private string _lastSlotSaved;

    public string LastSlotSaved => _lastSlotSaved;

    private void Awake()
    {
        SaveSystemManager.Init(_serializationType);
        SaveStorage.Init(_folderName);

        _lastSlotSaved = SaveSystemManager.GetLastSlotSaved();
    }

}