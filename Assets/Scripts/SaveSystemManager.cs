using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystemManager : MonoBehaviour
{
    List<ISavable> _savableItems = new();

    [SerializeField] Button _newGameButton;

    FileSaveStorage _fileSaveStorage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fileSaveStorage = new("Saves");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSaveData(string slotId)
    {
        StringBuilder stringBuilder = new();

        foreach (var item in _savableItems)
        {
            stringBuilder.Append(item.SaveData());
        }

        _fileSaveStorage.Write(slotId, stringBuilder.ToString());
    }

    public void AddSavableItem(ISavable data)
    {
        _savableItems.Add(data);
    }

    public void RemoveSavableData(ISavable data)
    {
        _savableItems.Remove(data);
    }
}
