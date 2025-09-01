using UnityEngine;

public interface ISavableData
{
    public string SaveableId { get; set; }
    public string SaveData();

    public void LoadData();

    public void DeleteData();
}
