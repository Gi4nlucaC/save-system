using System;
using UnityEngine;
using PeraphsPizza.SaveSystem;

public class PotionSpawner : SavableMonoBehaviour
{
    [SerializeField] float _spawnTimer;

    PotionData _potionData;
    float _nextSpawnTimer;
    Collider _collider;

    private void Awake()
    {
        RegisterForSave();

        TryGetComponent<Collider>(out _collider);
    }

    private void Update()
    {
        if (_nextSpawnTimer > 0)
            _nextSpawnTimer -= Time.deltaTime;
        else
            SpawnPotion(true);

    }

    private void SpawnPotion(bool shouldSpawn)
    {
        transform.GetChild(0).gameObject.SetActive(shouldSpawn);
        _collider.enabled = shouldSpawn;
    }

    public override void LoadData()
    {
        if (SaveSystemManager.ExistData(_persistentId.Value) >= 0)
        {
            _potionData = SaveSystemManager.GetData(_persistentId.Value) as PotionData;
            _nextSpawnTimer = _potionData._nextSpawnTimer;
        }
        else
        {
            _potionData = new()
            {
                _id = _persistentId.Value,
                _nextSpawnTimer = _spawnTimer,
                _healAmount = 2
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out var character))
        {
            character.Heal(_potionData._healAmount);

            SpawnPotion(false);
            _nextSpawnTimer = _spawnTimer;
        }
    }
}
