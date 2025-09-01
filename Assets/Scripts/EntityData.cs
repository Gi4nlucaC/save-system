using UnityEngine;

public class EntityData : ISavableData
{
    protected string _id;
    protected string _name;
    protected Vector3 _position;
    protected Quaternion _rotation;
    protected Vector3 _scale;

    public string SaveableId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
