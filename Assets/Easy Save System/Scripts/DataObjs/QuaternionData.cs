using Newtonsoft.Json;
using UnityEngine;

namespace PizzaCompany.SaveSystem
{
    public class QuaternionData
    {
        public float x, y, z, w;

        [JsonConstructor]
        public QuaternionData(Quaternion quaternion)
        {
            this.x = quaternion.x;
            this.y = quaternion.y;
            this.z = quaternion.z;
            this.w = quaternion.w;
        }

        public QuaternionData(float rX, float rY, float rZ, float rW)
        {
            this.x = rX;
            this.y = rY;
            this.z = rZ;
            this.w = rX;
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }
}