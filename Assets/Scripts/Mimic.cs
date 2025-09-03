using System.Collections;
using UnityEngine;

public class Mimic : SavableMonoBehaviour, ISavable
{
    MovementComponent _movementComponent;
    EnemyData _enemyData;

    [Header("Random Movement")]
    [SerializeField] float minInterval = 0.6f;
    [SerializeField] float maxInterval = 1.8f;
    [SerializeField] float idleChance = 0.25f;
    Vector2 _currentInput;
    Coroutine _inputRoutine;

    void Awake()
    {
        _movementComponent = new MovementComponent(1f);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


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

    void OnEnable()
    {
        _inputRoutine = StartCoroutine(RandomInputRoutine());
    }

    void OnDisable()
    {
        StopCoroutine(_inputRoutine);
    }

    IEnumerator RandomInputRoutine()
    {
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            if (Random.value < idleChance)
                _currentInput = Vector2.zero;
            else
            {
                Vector2 v = Random.insideUnitCircle;
                if (v.sqrMagnitude > 0.0001f) v.Normalize();
                _currentInput = v;
            }

            float t = Random.Range(minInterval, maxInterval);
            wait = new WaitForSeconds(t);
            yield return wait;
        }
    }
    void HandleMovement()
    {
        Vector3 movement = _movementComponent.GetMovementVector(_currentInput, Time.deltaTime);
        if (movement.sqrMagnitude > 0f)
        {
            transform.position += movement;

            // Rotazione opzionale verso direzione
            Vector3 flat = new(movement.x, 0f, movement.z);
            if (flat.sqrMagnitude > 0.0001f)
                transform.rotation = Quaternion.LookRotation(flat);
        }
    }

    public void SnapshotData()
    {
        _enemyData.UpdateData(transform.position, transform.rotation, transform.localScale);
    }

    public void DeleteData()
    {

    }

    public EntityData SaveData()
    {
        SnapshotData();

        return _enemyData;
    }

    public void LoadData()
    {
        if (SaveSystemManager.ExistData(_persistentId.Value) >= 0)
        {
            _enemyData = SaveSystemManager.GetData(_persistentId.Value) as EnemyData;
            transform.SetPositionAndRotation(_enemyData._position.ToVector3(), _enemyData._rotation.ToQuaternion());
        }
        else
            _enemyData = new(_persistentId.Value, "Mimic", transform.position, transform.rotation, transform.localScale, EnemyStates.IDLE);
    }
}
