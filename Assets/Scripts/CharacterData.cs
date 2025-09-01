using UnityEngine;

public class CharacterData : EntityData, ISavableData
{
    protected int _exp;
    protected int _lvl;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    public CharacterData (string name = "", Vector3 pos = new Vector3(), Quaternion rot = new Quaternion(), Vector3 scale = new Vector3(), int exp = 0, int lvl = 1)
    {
        this._name = name;
        this._position = pos;
        this._rotation = rot;
        this._scale = scale;
        this._exp = exp;
        this._lvl = lvl;
    }
}
