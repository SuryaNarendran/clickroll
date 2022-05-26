using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButtonHandler : MonoBehaviour
{
    public void Save()
    {
        RollGroupStorage.UpdateSelectedRollGroup(SelectionManager.EditableDisplay.GetRollGroupCopy());
        RollGroupStorage.SaveToFile();

        SelectionManager.EditingOverlay.gameObject.SetActive(false);
    }
}
