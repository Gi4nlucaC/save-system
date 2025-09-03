using System.Collections;
using UnityEngine;

[DefaultExecutionOrder (-999999999)]
public class GameManager : MonoBehaviour
{
    [SerializeField] Character _player;

    [SerializeField] float tickTime = 3f;
    [SerializeField] int damagePerTick = 1;

    private void Awake()
    {
        SaveSystemManager.Init();
    }
    private void Start()
    {
        if (_player != null)
            StartCoroutine(LifeExpRoutine());
    }

    private IEnumerator LifeExpRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("coroutine");
        while (_player != null && !_player.IsDead())
        {
            _player.TakeDamage(damagePerTick);

            int expGain = Random.Range(1, 11);
            _player.AddExp(expGain);

            yield return new WaitForSeconds(tickTime);
        }
    }
}
