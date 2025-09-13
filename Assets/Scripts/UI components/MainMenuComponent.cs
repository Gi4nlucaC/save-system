using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PizzaCompany.SaveSystem;

namespace PizzaCompany.SaveSystem
{
    /// <summary>
    /// Sample main menu component demonstrating how to integrate the Save System.
    /// This script is included as an example UI and can be removed or replaced.
    /// </summary>
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

            Bootstrap();

            //_continueButton.gameObject.SetActive(false);
        }


        public void Bootstrap()
        {
            // Enable continue only if a previous slot exists (local or cloud)
            if (String.IsNullOrEmpty(_slotManager.LastSlotSaved)) // TODO: refine logic based on real last valid slot
            {
                _continueButton.onClick.AddListener(OnContinueButtonClicked);
                _continueButton.gameObject.SetActive(true);
            }

        }


        void OnContinueButtonClicked()
        {
            // Loads last known data (cloud preferred if any cached payload is present)
            if (CloudSave.CloudDatas.Count > 0)
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
}
