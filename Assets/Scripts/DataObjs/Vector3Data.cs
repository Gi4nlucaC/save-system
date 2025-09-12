using Newtonsoft.Json;
using UnityEngine;

namespace PeraphsPizza.SaveSystem
{
    public class Vector3Data
    {
        public float x;
        public float y;
        public float z;

        [JsonConstructor]
        public Vector3Data(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        public Vector3Data(float rX, float rY, float rZ)
        {
            this.x = rX;
            this.y = rY;
            this.z = rZ;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}