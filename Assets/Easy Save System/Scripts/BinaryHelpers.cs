using System.IO;

namespace PizzaCompany.SaveSystem
{
    public static class BinaryHelpers
    {
        // ---- Vector3Data ----
        public static void WriteVector3(this BinaryWriter writer, Vector3Data v)
        {
            writer.Write(v.x);
            writer.Write(v.y);
            writer.Write(v.z);
        }

        public static Vector3Data ReadVector3(this BinaryReader reader)
        {
            return new Vector3Data(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        // ---- QuaternionData ----
        public static void WriteQuaternion(this BinaryWriter writer, QuaternionData q)
        {
            writer.Write(q.x);
            writer.Write(q.y);
            writer.Write(q.z);
            writer.Write(q.w);
        }

        public static QuaternionData ReadQuaternion(this BinaryReader reader)
        {
            return new QuaternionData(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}