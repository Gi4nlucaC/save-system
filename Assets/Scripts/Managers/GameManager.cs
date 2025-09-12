using System.Collections;
using UnityEngine;
using PeraphsPizza.SaveSystem;

[DefaultExecutionOrder(-999999999)]
public class GameManager : MonoBehaviour
{
    [SerializeField] Character _player;

    [SerializeField] float tickTime = 3f;
    [SerializeField] int damagePerTick = 1;

    [SerializeField] float autoSaveInterval = 30f;

    private void Awake()
    {
        SaveSystemManager.OnAllSavablesLoaded += OnApplicationReady;
        /* SaveStorage.Init("Saves");  //TODO TO BE REMOVED
        SaveSystemManager.Init(SerializationMode.Json);     //TODO TO BE REMOVED */
    }

    private void Start()
    {
        SaveSystemManager.LoadAllSavable();

        StartCoroutine(AutoSaveRoutine());
    }
    private void OnApplicationReady()
    {
        if (_player != null)
            StartCoroutine(LifeExpRoutine());
    }

    private IEnumerator LifeExpRoutine()
    {
        while (_player != null && !_player.IsDead())
        {
            _player.TakeDamage(damagePerTick);

            int expGain = Random.Range(1, 11);
            _player.AddExp(expGain);

            yield return new WaitForSeconds(tickTime);
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);

            SaveSystemManager.AutoSave("AUTOSAVE");
        }
    }
}
