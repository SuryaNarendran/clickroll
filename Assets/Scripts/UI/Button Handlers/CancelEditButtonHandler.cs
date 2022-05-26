using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelEditButtonHandler : MonoBehaviour
{
    public void ConfirmCancelEdit()
    {
        RollGroup editedRollGroup = SelectionManager.EditableDisplay.GetRollGroupCopy();
        if (editedRollGroup.IsDataEquivalent(SelectionManager.SelectedRollGroup))
        {
            CancelEdit();
        }
        else ConfirmationPopup.Raise("Discard unsaved changes to this roll?", CancelEdit);
    }

    private void CancelEdit()
    {
        SelectionManager.EditingOverlay.gameObject.SetActive(false);
    }
}
