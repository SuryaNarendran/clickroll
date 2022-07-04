using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButtonHandler : MonoBehaviour
{
    public void Save()
    {
        if(SelectionManager.CurrentEditingMode == SelectionManager.EditingMode.EditGroup)
        {
            RollGroupStorage.UpdateSelectedRollGroup(SelectionManager.EditableDisplay.GetRollGroupCopy());
        }
        else if(SelectionManager.CurrentEditingMode == SelectionManager.EditingMode.AddGroup)
        {
            RollGroupStorage.AddGroup(SelectionManager.EditableDisplay.GetRollGroupCopy());
        }

        RollGroupStorage.SaveToFile();
        SelectionManager.EditingOverlay.gameObject.SetActive(false);
    }
}
