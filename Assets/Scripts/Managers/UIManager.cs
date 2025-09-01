using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnSaveButtonClicked()
    {
        SaveSystemManager.OnSaveData("firstTest");
    }
}
