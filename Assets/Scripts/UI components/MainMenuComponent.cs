using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuComponent : MonoBehaviour
{
    [SerializeField] Button _continueButton;
    [SerializeField] Button _newGameButton;
    [SerializeField] Button _loadGameButton;
    [SerializeField] Button _deleteSaveSlotButton;

    [SerializeField] GameObject _savedSlotsPanel;
    [SerializeField] Button _backToMenuButton;


    private void Awake()
    {
        _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        _loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        _deleteSaveSlotButton.onClick.AddListener(OnDeleteSaveSlotButtonClicked);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);


        _continueButton.gameObject.SetActive(false);
    }

    public void Bootstrap()
    {
        //get the files first from the cloud
        if (true)//todo: if there's a save slot
        {
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _continueButton.gameObject.SetActive(true);
        }

    }

    void OnContinueButtonClicked()
    {
        // TODO: prendi l’ultimo slot di salvataggio valido
        //SaveSystemManager.OnLoadData(lastSlot);
    }

    void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    void OnLoadGameButtonClicked()
    {
        _savedSlotsPanel.SetActive(true);
    }

    void OnDeleteSaveSlotButtonClicked()
    {
        _savedSlotsPanel.SetActive(true);
    }

    void OnBackToMenuButtonClicked()
    {
        _savedSlotsPanel.SetActive(false);
    }
}
