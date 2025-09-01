using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterData _characterData;

    InputSystem_Actions _playerInput;

    MovementComponent _movementComponent;

    [SerializeField] CharacterController _controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = new();
        _playerInput.Enable();
        _movementComponent = new(1f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 input = _playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 movement = _movementComponent.GetMovementVector(input, Time.deltaTime);
        _controller.Move(movement);
    }
}
