using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Serializable wrapper for Unity's Quaternion struct
    /// </summary>
    [System.Serializable]
    public class QuaternionData
    {
        /// <summary>X, Y, Z, W components of the quaternion</summary>
        public float x, y, z, w;

        /// <summary>
        /// Create from Unity Quaternion
        /// </summary>
        /// <param name="quaternion">Quaternion to convert</param>
        [JsonConstructor]
        public QuaternionData(Quaternion quaternion)
        {
            this.x = quaternion.x;
            this.y = quaternion.y;
            this.z = quaternion.z;
            this.w = quaternion.w;
        }

        /// <summary>
        /// Create from individual components
        /// </summary>
        /// <param name="rX">X component</param>
        /// <param name="rY">Y component</param>
        /// <param name="rZ">Z component</param>
        /// <param name="rW">W component</param>
        public QuaternionData(float rX, float rY, float rZ, float rW)
        {
            this.x = rX;
            this.y = rY;
            this.z = rZ;
            this.w = rW;
        }

        /// <summary>
        /// Convert back to Unity Quaternion
        /// </summary>
        /// <returns>Unity Quaternion representation</returns>
        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }
}