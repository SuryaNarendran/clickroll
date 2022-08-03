using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class NotesField : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] SelectableUISet historyScroller;

    private void OnEnable()
    {
        inputField.onEndEdit.AddListener(OnInputProcessed);
        SelectionManager.onSelectedHistoryIndexUpdated += SetupNotes;
    }

    private void OnDisable()
    {
        inputField.onEndEdit.RemoveListener(OnInputProcessed);
        SelectionManager.onSelectedHistoryIndexUpdated -= SetupNotes;
    }
    private void OnInputProcessed(string input)
    {
        RollGroupStorage.SetNotes(input, SelectionManager.SelectedHistoryGroupIndex);
    }

    public void SetupNotes()
    {
        inputField.text = RollGroupStorage.GetHistoryNotesAtIndex(SelectionManager.SelectedHistoryGroupIndex);
    }
}
