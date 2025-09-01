using UnityEngine;

public class MovementComponent : IMoveableInput, IMoveableApplier
{
    float _speed = 1;
    public MovementComponent(float speed)
    {
        this._speed = speed;
    }
    public void ApplyMovement(Vector3 movement)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 GetMovementVector(Vector3 input, float deltaTime)
    {
        Vector3 raw = new(input.x, 0, input.y);

        return raw.normalized * _speed * deltaTime;
    }

}
