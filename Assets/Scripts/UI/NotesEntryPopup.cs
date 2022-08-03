using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class NotesEntryPopup : Singleton<NotesEntryPopup>
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Transform notesOverlay;

    public void OnSaveButtonPress()
    {
        RollGroupStorage.SetNotes(inputField.text, SelectionManager.SelectedHistoryGroupIndex);
        inputField.text = "";
        notesOverlay.gameObject.SetActive(false);
    }

    public void OnCancelButtonPress()
    {
        inputField.text = "";
        notesOverlay.gameObject.SetActive(false);
    }

    public static void RaiseNotesPopup()
    {
        Instance.notesOverlay.gameObject.SetActive(true);
        Instance.inputField.text = RollGroupStorage.GetHistoryNotesAtIndex(SelectionManager.SelectedHistoryGroupIndex);
    }
}
