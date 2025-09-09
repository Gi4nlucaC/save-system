using UnityEngine;

public class SlotContainerUI : MonoBehaviour
{
    [SerializeField] SaveSlotUIComponent[] _saveSlots;
    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void RefreshList(UISlotsStates state)
    {
        var saves = SaveSystemManager.GetSlotMetaInfo();

        for (int i = 0; i < _saveSlots.Length; i++)
        {
            SaveSlotUIComponent slotUi = _saveSlots[i];

            bool hasSave = saves != null && i < saves.Count;

            // Aggiorna i valori UI
            if (hasSave)
            {
                var save = saves[i];
                slotUi.UpdateUIValues(
                    save.PlayerName,
                    SaveSystemManager.FormatPlayTime(save.PlayTimeSeconds),
                    SaveSystemManager.BuildDateString(save)
                );
            }
            else
            {
                slotUi.SetSlotEmpty();
                slotUi.SetInteractable(false);
            }

            RegisterSlotCallback(slotUi, state, hasSave ? saves[i].SlotId : i.ToString());
        }
    }

    private void RegisterSlotCallback(SaveSlotUIComponent slotUi, UISlotsStates state, string slotId)
    {
        switch (state)
        {
            case UISlotsStates.LOAD:
                slotUi.RegisterForLoadSlot(slotId);
                break;
            case UISlotsStates.OVERWRITE:
                slotUi.RegisterForOverwriteSlot(slotId);
                slotUi.SetInteractable(true);
                break;
            case UISlotsStates.DELETE:
                slotUi.RegisterForDeleteSlot(slotId);
                break;
        }
    }

}
