using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text _savingLoadingText;

    [SerializeField] Button _menuButtonOpen;
    [SerializeField] Button _menuButtonClose;
    [SerializeField] Button _savedSlotMenuButtonClose;

    [SerializeField] GameObject _menuPanel;

    [SerializeField] Button _saveGameButton;
    [SerializeField] Button _saveAndQuitGameButton;
    [SerializeField] Button _loadGameButton;
    [SerializeField] Button _quitGameButton;

    [SerializeField] SlotContainerUI _savedSlotsPanel;

    [SerializeField] float _messageDuration = 2f;
    [SerializeField] float _fadeDuration = 0.5f;

    Coroutine _currentMessageRoutine;

    private void Awake()
    {
        SaveSystemManager.OnAutoSave += OnAutoSave;

        _menuButtonOpen.onClick.AddListener(OnMenuOpenClicked);
        _menuButtonClose.onClick.AddListener(OnMenuCloseClicked);
        _savedSlotMenuButtonClose.onClick.AddListener(OnSavedSlotMenuCloseClicked);

        _saveGameButton.onClick.AddListener(OnSaveButtonClicked);
        _saveAndQuitGameButton.onClick.AddListener(OnSaveAndQuitButtonClicked);
        _loadGameButton.onClick.AddListener(OnLoadButtonClicked);
        _quitGameButton.onClick.AddListener(OnQuitButtonClicked);
    }

    public void OnMenuOpenClicked()
    {
        Time.timeScale = 0;
        _menuPanel.SetActive(true);
    }

    public void OnMenuCloseClicked()
    {
        Time.timeScale = 1;
        _menuPanel.SetActive(false);
    }

    public void OnSavedSlotMenuCloseClicked()
    {
        SaveSystemManager.OnGameSavedManually -= OnGameSavedSucessfully;
        SaveSystemManager.OnGameSavedManually -= OnSaveAndQuitSucessfully;

        _savedSlotsPanel.ToggleVisibility();
    }

    public void OnSaveButtonClicked()
    {
        SaveSystemManager.OnGameSavedManually += OnGameSavedSucessfully;
        _savedSlotsPanel.RefreshList(UISlotsStates.OVERWRITE);
        _savedSlotsPanel.ToggleVisibility();

    }

    public void OnGameSavedSucessfully()
    {
        _savedSlotsPanel.RefreshList(UISlotsStates.OVERWRITE);
        ShowMessage("Game Saved!");
    }

    public void OnSaveAndQuitButtonClicked()
    {
        SaveSystemManager.OnGameSavedManually += OnSaveAndQuitSucessfully;
        _savedSlotsPanel.RefreshList(UISlotsStates.OVERWRITE);
        _savedSlotsPanel.ToggleVisibility();

    }

    void OnSaveAndQuitSucessfully()
    {
        ShowMessage("Game Saved. Quitting...");
        SceneManager.LoadScene(0);
    }

    public void OnLoadButtonClicked()
    {
        SaveSystemManager.OnAllSavablesLoaded += OnGameLoadedSucessfully;
        _savedSlotsPanel.RefreshList(UISlotsStates.LOAD);
        _savedSlotsPanel.ToggleVisibility();
    }

    void OnGameLoadedSucessfully()
    {
        SaveSystemManager.OnAllSavablesLoaded -= OnGameLoadedSucessfully;
        Time.timeScale = 1;
    }

    public void OnQuitButtonClicked()
    {
        ShowMessage("Quitting Game...");
        SceneManager.LoadScene(0);
    }

    void ShowMessage(string message)
    {
        if (_currentMessageRoutine != null)
            StopCoroutine(_currentMessageRoutine);

        _currentMessageRoutine = StartCoroutine(ShowMessageRoutine(message));
    }
    IEnumerator ShowMessageRoutine(string message)
    {
        _savingLoadingText.gameObject.SetActive(true);
        _savingLoadingText.text = message;

        Color c = _savingLoadingText.color;
        c.a = 1f;
        _savingLoadingText.color = c;

        yield return new WaitForSeconds(_messageDuration);

        // Fade out
        float elapsed = 0f;
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);
            c.a = alpha;
            _savingLoadingText.color = c;
            yield return null;
        }

        _savingLoadingText.gameObject.SetActive(false);
        _currentMessageRoutine = null;
    }

    void OnAutoSave()
    {
        ShowMessage("Saving... Dont Turn OFF!");
    }
}
