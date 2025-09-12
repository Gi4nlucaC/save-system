using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PeraphsPizza.SaveSystem;

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
        SaveSystemManager.OnGameSavedManually -= OnSaveAndQuitSucessfully;
        SaveSystemManager.OnGameSavedManually -= OnGameSavedSucessfully;
        SceneManager.LoadScene(0);
    }

    public void OnLoadButtonClicked()
    {
        _savedSlotsPanel.RefreshList(UISlotsStates.LOAD);
        _savedSlotsPanel.ToggleVisibility();
    }

    public void OnQuitButtonClicked()
    {
        SaveSystemManager.OnResetGame();

        SceneManager.LoadScene(0);
    }

    void OnAutoSave()
    {
        ShowMessage("Autosave...");
    }

    void ShowMessage(string message)
    {
        if (_currentMessageRoutine != null)
            StopCoroutine(_currentMessageRoutine);

        _currentMessageRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        _savingLoadingText.text = message;
        _savingLoadingText.alpha = 1f;

        yield return new WaitForSeconds(_messageDuration);

        float elapsedTime = 0f;
        float startAlpha = _savingLoadingText.alpha;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / _fadeDuration);
            _savingLoadingText.alpha = alpha;

            yield return null;
        }

        _savingLoadingText.alpha = 0f;
    }
}
