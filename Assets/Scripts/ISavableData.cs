using UnityEngine;

public interface ISavableData
{
    public string SaveableId { get; set; }
    public string SaveData();

    public ISavableData LoadData(string data);

    public void DeleteData();
}
