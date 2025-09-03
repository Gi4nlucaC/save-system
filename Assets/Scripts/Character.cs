using UnityEngine;

public class Character : SavableMonoBehaviour, ISavable
{
    CharacterData _characterData;
    MovementComponent _movementComponent;


    InputSystem_Actions _playerInput;

    [SerializeField] CharacterController _controller;
    [SerializeField] Animator _anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = new();
        _playerInput.Enable();
        _movementComponent = new(1f);

        LoadData();

        if (!string.IsNullOrEmpty(PersistentId))
            SaveSystemManager.RegisterSavable(this);
        else
            Debug.LogWarning($"{name}: PersistentId non generato. Premi 'Generate' sul campo UniqueGUID o usa il context menu.");
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

    public void SnapshotData()
    {
        _characterData.UpdateData(transform.position, transform.rotation, transform.localScale);
    }

    public EntityData SaveData()
    {
        SnapshotData();

        return _characterData;
    }

    public void LoadData()
    {
        if (SaveSystemManager.ExistData(_persistentId.Value) >= 0)
        {
            _characterData = SaveSystemManager.GetData(_persistentId.Value) as CharacterData;
            transform.SetPositionAndRotation(_characterData._position.ToVector3(), _characterData._rotation.ToQuaternion());
        }
        else
            _characterData = new(_persistentId.Value, "Gino", Vector3.zero, Quaternion.identity, Vector3.one, 1, 1);
    }

    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }
}
