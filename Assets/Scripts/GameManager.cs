using UnityEngine;

[DefaultExecutionOrder (-999999999)]
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        SaveSystemManager.Init();
    }
}
