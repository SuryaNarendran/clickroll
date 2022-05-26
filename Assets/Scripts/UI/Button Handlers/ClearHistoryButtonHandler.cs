using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearHistoryButtonHandler : MonoBehaviour
{
    public void ConfirmClearHistory()
    {
        ConfirmationPopup.Raise("Clear All Roll History?", ClearHistory);
    }

    private void ClearHistory()
    {
        RollGroupStorage.ClearAllHistory();
        RollGroupStorage.SaveToFile();
    }
    
}
