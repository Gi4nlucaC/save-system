using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] UniqueGUID _userId;
    [SerializeField] SerializationMode _serializationType;
    [SerializeField] string _folderName;

    private string _lastSlotSaved;

    public string LastSlotSaved => _lastSlotSaved;

    private void Awake()
    {
        CloudSave.Init(userId: _userId.Value);
        SaveSystemManager.Init(_serializationType);
        SaveStorage.Init(_folderName);

        _lastSlotSaved = SaveSystemManager.GetLastSlotSaved();
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnValidate()
    {
        Generate();
    }

    [ExecuteInEditMode]
    private void Generate()
    {
        if (_userId == null)
            _userId = new UniqueGUID();

        if (!_userId.IsValid)
            _userId.Set(System.Guid.NewGuid().ToString("N"));
    }
#endif

}