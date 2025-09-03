using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : SavableMonoBehaviour
{
    CharacterData _characterData;
    MovementComponent _movementComponent;


    InputSystem_Actions _playerInput;

    [SerializeField] CharacterController _controller;
    [SerializeField] Animator _anim;

    // ---- RPG Stats ----
    const int BaseHp = 50;
    const int HpGrowth = 10;

    int _level;
    int _exp;
    int _currentHp;

    int ExpToNextLevel => 100 * (_level * _level);
    int MaxHp => BaseHp + (_level * HpGrowth);


    [Header("UI elements")]
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _lvlText;
    [SerializeField] TMP_Text _expText;
    [SerializeField] TMP_Text _hpText;
    [SerializeField] Image _hpBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInput = new();
        _playerInput.Enable();
        _movementComponent = new(1f);

        Initialize();
        UpdateUI();
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
            return transform.rotation;


        float angle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;


        float snapped = Mathf.Round(angle / 45f) * 45f;

        return Quaternion.Euler(0, snapped, 0);
    }

    void PlayAnimation(string anim)
    {
        _anim.Play(anim);
    }

    // --- RPG Methods ---
    public void AddExp(int amount)
    {
        _exp += amount;

        while (_exp >= ExpToNextLevel)
        {
            _exp -= ExpToNextLevel;
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        _level++;
        _currentHp = MaxHp;
        Debug.Log($"Level Up! -> Lvl {_level}, MaxHp = {MaxHp}");
    }

    public void TakeDamage(int dmg)
    {
        _currentHp = Mathf.Max(0, _currentHp - dmg);
        UpdateUI();
    }

    public void Heal(int amount)
    {
        _currentHp = Mathf.Min(MaxHp, _currentHp + amount);
        UpdateUI();
    }

    public bool IsDead() => _currentHp <= 0;

    void UpdateUI()
    {
        _nameText.text = _characterData._name;
        _lvlText.text = $"{_level}";
        _expText.text = $"{_exp}/{ExpToNextLevel}";
        _hpText.text = $"{_currentHp}/{MaxHp}";
        _hpBar.fillAmount = (float)_currentHp / MaxHp;
    }

    // --- Save / Load ---

    public override void SnapshotData()
    {
        _characterData.UpdateData(transform.position, transform.rotation, transform.localScale, _currentHp, _exp, _level);
    }

    public override PureRawData SaveData()
    {
        SnapshotData();

        return _characterData;
    }

    public override void LoadData()
    {
        if (SaveSystemManager.ExistData(_persistentId.Value) >= 0)
        {
            _characterData = SaveSystemManager.GetData(_persistentId.Value) as CharacterData;
            transform.SetPositionAndRotation(_characterData._position.ToVector3(), _characterData._rotation.ToQuaternion());
            _level = Mathf.Max(1, _characterData._lvl);
            _exp = Mathf.Max(0, _characterData._exp);
            _currentHp = Mathf.Clamp(_characterData._hp, 0, MaxHp);
        }
        else
        {
            _level = 1;
            _exp = 0;
            _currentHp = MaxHp;

            _characterData = new(
                _persistentId.Value,
                "Gino",
                Vector3.zero,
                Quaternion.identity,
                Vector3.one,
                _level,
                _currentHp,
                _exp
            );
        }



        UpdateUI();
    }

    public override void DeleteData()
    {
        throw new System.NotImplementedException();
    }
}
