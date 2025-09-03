using UnityEngine;

public class QuaternionData
{
    public float x, y, z, w;

    public QuaternionData(Quaternion quaternion)
    {
        this.x = quaternion.x;
        this.y = quaternion.y;
        this.z = quaternion.z;
        this.w = quaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}