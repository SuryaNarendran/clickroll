using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesButtonHandler : MonoBehaviour
{
    public void OnPresed()
    {
        NotesEntryPopup.RaiseNotesPopup();
    }
}
