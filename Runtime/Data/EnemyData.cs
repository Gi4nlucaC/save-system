using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Data container for enemy-specific information
    /// </summary>
    [System.Serializable]
    [DataTypeId(2)]
    public class EnemyData : PhysicalEntityData
    {
        /// <summary>Current enemy state</summary>
        public EnemyStates _enemyState = EnemyStates.IDLE;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EnemyData() { }

        /// <summary>
        /// Full constructor for creating enemy data
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="name">Enemy name</param>
        /// <param name="pos">World position</param>
        /// <param name="rot">World rotation</param>
        /// <param name="scale">Local scale</param>
        /// <param name="enemyState">Current enemy state</param>
        [JsonConstructor]
        public EnemyData(string id, string name = "", Vector3 pos = new Vector3(), Quaternion rot = new Quaternion(), Vector3 scale = new Vector3(), EnemyStates enemyState = EnemyStates.IDLE)
        {
            this._id = id;
            this._name = name;
            this._position = new(pos);
            this._rotation = new(rot);
            this._scale = new(scale);
            this._enemyState = enemyState;
        }

        /// <summary>
        /// Update enemy data with new values
        /// </summary>
        /// <param name="pos">New position</param>
        /// <param name="rot">New rotation</param>
        /// <param name="scale">New scale</param>
        /// <param name="enemyState">New enemy state</param>
        public void UpdateData(Vector3 pos, Quaternion rot, Vector3 scale, EnemyStates enemyState = EnemyStates.IDLE)
        {
            this._position = new(pos);
            this._rotation = new(rot);
            this._scale = new(scale);
            this._enemyState = enemyState;
        }
    }
}