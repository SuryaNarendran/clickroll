using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryDeleteButtonHandler : MonoBehaviour
{
    public void DeleteHistory()
    {
        RollGroupStorage.RemoveHistory(SelectionManager.SelectedHistoryGroupIndex);
    }
}
