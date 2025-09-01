using UnityEngine;

public class EntityData : ISavableData
{
    public string _id;
    public string _name;
    public Vector3 _position;
    public Quaternion _rotation;
    public Vector3 _scale;

    //public string SaveableId { get => ""; set => value; }

    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }

    public ISavableData LoadData(string data)
    {
        return JsonUtility.FromJson<ISavableData>(data);
    }

    public string SaveData()
    {
        string data = JsonUtility.ToJson(this);
        return data;
    }
}
