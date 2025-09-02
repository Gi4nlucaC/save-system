using System.Collections;
using UnityEngine;

public class Mimic : MonoBehaviour, ISavable
{
    [SerializeField] private UniqueGUID _persistentId;
    public string PersistentId => throw new System.NotImplementedException();
    MovementComponent _movementComponent;

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
    void OnEnable()
    {
        _inputRoutine = StartCoroutine(RandomInputRoutine());
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

    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }

    public ISavableData LoadData(string data)
    {
        throw new System.NotImplementedException();
    }

    public string SaveData()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
}
