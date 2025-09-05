using UnityEngine;
using UnityEngine.UI;

public class MainMenuComponent : MonoBehaviour
{
    [SerializeField] Button _continueButton;
    [SerializeField] Button _newGameButton;
    [SerializeField] Button _loadGameButton;
    [SerializeField] Button _deleteSaveSlotButton;



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
        //prendi l'ultimo slot di salvataggio
    }

    void OnNewGameButtonClicked()
    {
        //fai partire una nuova partita
    }

    void OnLoadGameButtonClicked()
    {
        //apre un pannello con gli slot salvati per caricarli
    }

    void OnDeleteSaveSlotButtonClicked()
    {
        //apre un pannello con gli slot salvati per cancellarli
    }
}
