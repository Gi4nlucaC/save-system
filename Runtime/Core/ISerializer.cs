using System.Collections.Generic;

namespace SaveSystem
{
    /// <summary>
    /// Interface for serializing data to different formats
    /// </summary>
    /// <typeparam name="TFormat">The format type (string, byte[], etc.)</typeparam>
    public interface ISerializer<TFormat>
    {
        /// <summary>
        /// Serialize data to the specified format
        /// </summary>
        /// <typeparam name="T">Type of data to serialize</typeparam>
        /// <param name="data">Data to serialize</param>
        /// <returns>Serialized data in the specified format</returns>
        TFormat Serialize<T>(T data) where T : List<PhysicalEntityData>;
        
        /// <summary>
        /// Deserialize data from the specified format
        /// </summary>
        /// <typeparam name="T">Type of data to deserialize to</typeparam>
        /// <param name="raw">Raw data to deserialize</param>
        /// <returns>Deserialized data</returns>
        T Deserialize<T>(TFormat raw) where T : List<PhysicalEntityData>;
    }
}