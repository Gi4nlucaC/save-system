using UnityEngine;

public class PotionSpawner : SavableMonoBehaviour
{
    [SerializeField] float _spawnTimer;
    [SerializeField] GameObject _potionPrefab;

    PotionData _potionData;
    float _nextSpawnTimer;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        _nextSpawnTimer -= Time.deltaTime;
        if (_nextSpawnTimer <= 0)
        {
            SpawnPotion();
            _nextSpawnTimer = _spawnTimer;
        }
    }

    private void SpawnPotion()
    {
        if (transform.childCount == 0)
            Instantiate(_potionPrefab, transform);
    }

    public override void LoadData()
    {
        if (SaveSystemManager.ExistData(_persistentId.Value) >= 0)
        {
            _potionData = SaveSystemManager.GetData(_persistentId.Value) as PotionData;
            _nextSpawnTimer = _potionData._newxSpawnTimer;
        }
        else
        {
            _potionData = new()
            {
                _id = _persistentId.Value,
                _newxSpawnTimer = _spawnTimer
            };
            _nextSpawnTimer = _spawnTimer;
        }

    }

    public override PureRawData SaveData()
    {
        SnapshotData();

        return _potionData;
    }

    public override void SnapshotData()
    {
        _potionData.UpdateData(_nextSpawnTimer);
    }
}
