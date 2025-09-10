using Newtonsoft.Json;
using UnityEngine;

[DataTypeId(3)]
public class CharacterData : PhysicalEntityData, IHeaderSavable
{
    public int _hp = 1;
    public int _exp = 0;
    public int _lvl = 1;

    public CharacterData() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="scale"></param>
    /// <param name="exp"></param>
    /// <param name="lvl"></param>
    [JsonConstructor]
    public CharacterData(string id, string name = "", Vector3 pos = new Vector3(), Quaternion rot = new Quaternion(), Vector3 scale = new Vector3(), int hp = 1, int exp = 0, int lvl = 1)
    {
        this._id = id;
        this._name = name;
        this._position = new(pos);
        this._rotation = new(rot);
        this._scale = new(scale);
        this._hp = hp;
        this._exp = exp;
        this._lvl = lvl;
    }

    public void UpdateData(Vector3 pos, Quaternion rot, Vector3 scale, int hp = 1, int? exp = null, int? lvl = null)
    {
        this._position = new(pos);
        this._rotation = new(rot);
        this._scale = new(scale);
        this._hp = hp;
        this._exp = (int)(exp.HasValue ? exp : this._exp);
        this._lvl = 1;
    }
    public MetaData GetMetaDataPart() => new MetaData
    {
        PlayerName = _name
    };
}
