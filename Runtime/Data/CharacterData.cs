using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Data container for character-specific information
    /// </summary>
    [System.Serializable]
    [DataTypeId(3)]
    public class CharacterData : PhysicalEntityData
    {
        /// <summary>Current health points</summary>
        public int _hp = 1;
        
        /// <summary>Current experience points</summary>
        public int _exp = 0;
        
        /// <summary>Current level</summary>
        public int _lvl = 1;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CharacterData() { }

        /// <summary>
        /// Full constructor for creating character data
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="name">Character name</param>
        /// <param name="pos">World position</param>
        /// <param name="rot">World rotation</param>
        /// <param name="scale">Local scale</param>
        /// <param name="hp">Health points</param>
        /// <param name="exp">Experience points</param>
        /// <param name="lvl">Character level</param>
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

        /// <summary>
        /// Update character data with new values
        /// </summary>
        /// <param name="pos">New position</param>
        /// <param name="rot">New rotation</param>
        /// <param name="scale">New scale</param>
        /// <param name="hp">New health points</param>
        /// <param name="exp">New experience (optional, keeps current if null)</param>
        /// <param name="lvl">New level (optional, keeps current if null)</param>
        public void UpdateData(Vector3 pos, Quaternion rot, Vector3 scale, int hp = 1, int? exp = null, int? lvl = null)
        {
            this._position = new(pos);
            this._rotation = new(rot);
            this._scale = new(scale);
            this._hp = hp;
            this._exp = (int)(exp.HasValue ? exp : this._exp);
            this._lvl = lvl.HasValue ? lvl.Value : this._lvl;
        }
    }
}