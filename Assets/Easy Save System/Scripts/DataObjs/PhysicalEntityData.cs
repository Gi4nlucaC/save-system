using UnityEngine;

namespace PizzaCompany.SaveSystem
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
