using UnityEngine;
using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem
{
    [DataTypeId(1)]
    public class PhysicalEntityData : PureRawData
    {
        public string _name;
        public Vector3Data _position;
        public QuaternionData _rotation;
        public Vector3Data _scale;
    }
}
