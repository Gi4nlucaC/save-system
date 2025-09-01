using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterData _characterData;
    MovementComponent _movementComponent = new();
    private void Awake()
    {
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _movementComponent.HandleMovement();
    }
}
