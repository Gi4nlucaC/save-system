using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PeraphsPizza.SaveSystem;

public class SaveSlotUIComponent : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _characterName;
    [SerializeField] TMP_Text _timePlayed;
    [SerializeField] TMP_Text _dateInGame;

    [SerializeField] ConfirmPanelUI _confirmPanel;

    const string emptyValue = "EMPTY";
    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
    public void UpdateUIValues(string charName, string time, string date)
    {
        _characterName.text = charName;
        _timePlayed.text = time;
        _dateInGame.text = date;
        _button.interactable = true;
    }

    public void SetSlotEmpty()
    {
        _characterName.text = emptyValue;
        _timePlayed.text = "##";
        _dateInGame.text = "##";
    }

    public void SetInteractable(bool value)
    {
        _button.interactable = value;
    }

    public void RegisterForLoadSlot(string slotID)
    {
        _button.onClick.AddListener(() => OnLoadSlotButtonClicked(slotID));
    }
    public void RegisterForOverwriteSlot(string slotID)
    {
        _button.onClick.AddListener(() => OnOverwriteLoadSlotButtonClicked(slotID));
    }
    public void RegisterForDeleteSlot(string slotID)
    {
        _button.onClick.AddListener(() => OnDeleteLoadSlotButtonClicked(slotID));
    }

    void OnLoadSlotButtonClicked(string slotID)
    {
        SaveSystemManager.OnLoadData(slotID);
        SceneManager.LoadScene(1);
    }

    void OnOverwriteLoadSlotButtonClicked(string slotID)
    {
        _confirmPanel.Enable(UISlotsStates.OVERWRITE, slotID);
    }

    void OnDeleteLoadSlotButtonClicked(string slotID)
    {
        _confirmPanel.Enable(UISlotsStates.DELETE, slotID);
    }

}
