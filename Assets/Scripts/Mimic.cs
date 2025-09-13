using System.Collections;
using UnityEngine;

namespace PizzaCompany.SaveSystem
{
    public class Mimic : SavableMonoBehaviour
    {
        [SerializeField] CharacterController _characterController;
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
            RegisterForSave();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
            {
                character.TakeDamage(5);
                Debug.Log("Gnam! the Mimic Bites U!");
            }
        }
        void OnEnable()
        {
            _inputRoutine = StartCoroutine(RandomInputRoutine());
        }

        void OnDisable()
        {
            if (_inputRoutine != null) StopCoroutine(_inputRoutine);
        }

        IEnumerator RandomInputRoutine()
        {
            while (true)
            {
                if (Random.value < idleChance) _currentInput = Vector2.zero;
                else
                {
                    Vector2 v = Random.insideUnitCircle;
                    if (v.sqrMagnitude > 0.0001f) v.Normalize();
                    _currentInput = v;
                }
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            }
        }
        void HandleMovement()
        {
            Vector3 movement = _movementComponent.GetMovementVector(_currentInput, Time.deltaTime);
            if (movement.sqrMagnitude > 0f)
            {
                _characterController.Move(movement);

                // Rotazione opzionale verso direzione
                Vector3 flat = new(movement.x, 0f, movement.z);
                if (flat.sqrMagnitude > 0.0001f)
                    transform.rotation = Quaternion.LookRotation(flat);
            }
        }

        public override void SnapshotData() => _enemyData.UpdateData(transform.position, transform.rotation, transform.localScale);
        public override void DeleteData() { }

        public override PureRawData SaveData()
        {
            SnapshotData();
            return _enemyData;
        }

        public override void LoadData()
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
}
