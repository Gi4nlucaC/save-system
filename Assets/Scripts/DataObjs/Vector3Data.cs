using UnityEngine;

public class Vector3Data
{
    public float x;
    public float y;
    public float z;

    public Vector3Data(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}