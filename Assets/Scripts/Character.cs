using Newtonsoft.Json;
using UnityEngine;

public class Character : MonoBehaviour, ISavable
{
    CharacterData _characterData;
    MovementComponent _movementComponent;

    public string SaveableId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    InputSystem_Actions _playerInput;
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented
    };

    [SerializeField] CharacterController _controller;
    [SerializeField] Animator _anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = new();
        _playerInput.Enable();
        _movementComponent = new(1f);

        _characterData = new("Gino", Vector3.zero, Quaternion.identity, Vector3.back, 1, 1);
        SaveSystemManager.RegisterSavable(this);
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

        if (movement.magnitude > 0.001f)
        {
            _controller.Move(movement);

            Quaternion targetRotation = Get8DirRotation(input);
            transform.rotation = targetRotation;
            PlayAnimation("Walking");
        }
        else
        {
            PlayAnimation("Idle");
        }

    }

    private Quaternion Get8DirRotation(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f)
            return transform.rotation; // niente input → non ruotare

        // angolo in radianti → gradi
        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;

        // quantizza a multipli di 45°
        float snapped = Mathf.Round(angle / 45f) * 45f;

        return Quaternion.Euler(0, snapped, 0);
    }

    void PlayAnimation(string anim)
    {
        _anim.Play(anim);
    }
    void SnapshotData()
    {
        _characterData.UpdateData(transform.position, transform.rotation, transform.localScale);
    }

    public string SaveData()
    {
        SnapshotData();

        return JsonUtility.ToJson(_characterData);
    }

    public ISavableData LoadData(string data)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }
}
