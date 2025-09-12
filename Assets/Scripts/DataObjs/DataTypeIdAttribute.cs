using System;

namespace PeraphsPizza.SaveSystem
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DataTypeIdAttribute : Attribute
    {
        public byte Id { get; }

        public DataTypeIdAttribute(byte id)
        {
            Id = id;
        }
    }
}
