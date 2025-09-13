using UnityEngine;
using UnityEngine.UI;
using PizzaCompany.SaveSystem;

namespace PizzaCompany.SaveSystem
{
    public class ConfirmPanelUI : MonoBehaviour
    {
        [SerializeField] Button _acceptButton;
        [SerializeField] Button _declineButton;
        [SerializeField] SlotContainerUI _saveSlotContainer;

        private void Awake()
        {
            _declineButton.onClick.AddListener(ToggleVisibility);
        }

        public void Enable(UISlotsStates state, string ID)
        {
            RegisterOnAcceptButton(state, ID);
            ToggleVisibility();
        }

        private void OnDisable()
        {
            _acceptButton.onClick.RemoveAllListeners();
        }

        void RegisterOnAcceptButton(UISlotsStates state, string ID)
        {
            switch (state)
            {
                case UISlotsStates.OVERWRITE:
                    _acceptButton.onClick.AddListener(() => OnOverwriteConfirmClicked(ID));
                    break;
                case UISlotsStates.DELETE:
                    _acceptButton.onClick.AddListener(() => OnDeleteConfirmClicked(ID));
                    break;
                default:
                    break;
            }
        }

        void OnOverwriteConfirmClicked(string ID)
        {
            SaveSystemManager.OnSaveData(ID);
            _saveSlotContainer.RefreshList(UISlotsStates.OVERWRITE);
            ToggleVisibility();
        }

        void OnDeleteConfirmClicked(string ID)
        {
            SaveSystemManager.DeleteData(ID);
            _saveSlotContainer.RefreshList(UISlotsStates.DELETE);
            ToggleVisibility();
        }

        public void ToggleVisibility()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
