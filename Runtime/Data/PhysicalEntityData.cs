using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Base class for entities that have physical properties (position, rotation, scale)
    /// </summary>
    [System.Serializable]
    [DataTypeId(1)]
    public class PhysicalEntityData : PureRawData
    {
        /// <summary>Display name of the entity</summary>
        public string _name;
        
        /// <summary>World position</summary>
        public Vector3Data _position;
        
        /// <summary>World rotation</summary>
        public QuaternionData _rotation;
        
        /// <summary>Local scale</summary>
        public Vector3Data _scale;
    }
}