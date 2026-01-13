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

    [SerializeField] SlotContainerUI _savedSlotsPanel;
    [SerializeField] Button _backToMenuButton;

    private void Awake()
    {
        _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        _loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        _deleteSaveSlotButton.onClick.AddListener(OnDeleteSaveSlotButtonClicked);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        //_continueButton.gameObject.SetActive(false);
    }

    void Start()
    {
        Bootstrap();
    }

    public void Bootstrap()
    {
        //get the files first from the cloud
        if (!String.IsNullOrEmpty(_slotManager.LastSlotSaved))//todo: if there's a save slot
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
        if (_slotManager.SaveInCloud && CloudSave.CloudDatas.Count > 0)
        {
            SaveSystemManager.OnCloudLoadData(CloudSave.CloudDatas[0].values);
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
        _savedSlotsPanel.RefreshList(UISlotsStates.LOAD);
        _savedSlotsPanel.ToggleVisibility();
    }

    void OnOverwriteSaveSlotButtonClicked()
    {
        _savedSlotsPanel.RefreshList(UISlotsStates.OVERWRITE);
        _savedSlotsPanel.ToggleVisibility();
    }

    void OnDeleteSaveSlotButtonClicked()
    {
        _savedSlotsPanel.RefreshList(UISlotsStates.DELETE);
        _savedSlotsPanel.ToggleVisibility();
    }

    void OnBackToMenuButtonClicked()
    {
        _savedSlotsPanel.ToggleVisibility();
    }
}
