using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuComponent : MonoBehaviour
{
    [SerializeField] SlotsManager _slotManager;

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

        Bootstrap();

        //_continueButton.gameObject.SetActive(false);
    }

    public void Bootstrap()
    {
        //get the files first from the cloud
        if (String.IsNullOrEmpty(_slotManager.LastSlotSaved))//todo: if there's a save slot
        {
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _continueButton.gameObject.SetActive(true);
        }

    }

    void OnContinueButtonClicked()
    {
        // TODO: prendi l�ultimo slot di salvataggio valido
        //SaveSystemManager.OnLoadData(lastSlot);
        //SaveSystemManager.OnLoadData(_slotManager.LastSlotSaved);

        var cloudSavings = CloudSave.LoadData();
        if (cloudSavings.Count > 0)
        {
            foreach (var item in cloudSavings)
            {

            }

        }
        else
        {
            SaveSystemManager.OnLoadData(_slotManager.LastSlotSaved);
        }
        SceneManager.LoadScene(1);

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
