using UnityEngine;

public class EnemyData : EntityData
{
    EnemyStates _enemyState = EnemyStates.IDLE;

    public EnemyData(string id, string name = "", Vector3 pos = new Vector3(), Quaternion rot = new Quaternion(), Vector3 scale = new Vector3(), EnemyStates enemyState = EnemyStates.IDLE)
    {
        this._id = id;
        this._name = name;
        this._position = new(pos);
        this._rotation = new(rot.eulerAngles);
        this._scale = new(scale);
        this._enemyState = enemyState;
    }
    public void UpdateData(Vector3 pos, Quaternion rot, Vector3 scale, EnemyStates enemyState = EnemyStates.IDLE)
    {
        this._position = new(pos);
        this._rotation = new(rot.eulerAngles);
        this._scale = new(scale);

        this._enemyState = enemyState;
    }
}
