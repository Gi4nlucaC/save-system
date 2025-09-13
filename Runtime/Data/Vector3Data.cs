using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Serializable wrapper for Unity's Vector3 struct
    /// </summary>
    [System.Serializable]
    public class Vector3Data
    {
        /// <summary>X component</summary>
        public float x;
        /// <summary>Y component</summary>
        public float y;
        /// <summary>Z component</summary>
        public float z;

        /// <summary>
        /// Create from Unity Vector3
        /// </summary>
        /// <param name="vector">Vector3 to convert</param>
        [JsonConstructor]
        public Vector3Data(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }

        /// <summary>
        /// Create from individual components
        /// </summary>
        /// <param name="rX">X component</param>
        /// <param name="rY">Y component</param>
        /// <param name="rZ">Z component</param>
        public Vector3Data(float rX, float rY, float rZ)
        {
            this.x = rX;
            this.y = rY;
            this.z = rZ;
        }

        /// <summary>
        /// Convert back to Unity Vector3
        /// </summary>
        /// <returns>Unity Vector3 representation</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}