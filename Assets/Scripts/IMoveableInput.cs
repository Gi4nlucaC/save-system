using UnityEngine;

public interface IMoveableInput
{
    Vector3 GetMovementVector(Vector3 input, float deltaTime);
}
