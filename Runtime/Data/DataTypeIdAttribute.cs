using System;

namespace SaveSystem
{
    /// <summary>
    /// Attribute to assign unique type IDs to data classes for serialization
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DataTypeIdAttribute : Attribute
    {
        /// <summary>
        /// Unique identifier for this data type
        /// </summary>
        public byte Id { get; }

        /// <summary>
        /// Initialize with a unique ID
        /// </summary>
        /// <param name="id">Unique identifier (0-255)</param>
        public DataTypeIdAttribute(byte id)
        {
            Id = id;
        }
    }
}