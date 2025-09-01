using UnityEngine;

public class CharacterData : EntityData
{
    public int? _exp;
    public int? _lvl;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    public CharacterData(string name = "", Vector3 pos = new Vector3(), Quaternion rot = new Quaternion(), Vector3 scale = new Vector3(), int exp = 0, int lvl = 1)
    {
        _id = System.Guid.NewGuid().ToString();
        this._name = name;
        this._position = pos;
        this._rotation = rot;
        this._scale = scale;
        this._exp = exp;
        this._lvl = lvl;
    }

    public void UpdateData(Vector3 pos, Quaternion rot, Vector3 scale, int? exp = null, int? lvl = null)
    {
        this._position = pos;
        this._rotation = rot;
        this._scale = scale;
        _exp = exp.HasValue ? exp : this._exp;
        //_lvl = _lvl.HasValue ? lvl : this._lvl;
        _lvl = 1;
    }
}
