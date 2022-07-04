using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGroupButtonHandler : MonoBehaviour
{
    public void AddGroup()
    {
        SelectionManager.EditingOverlay.gameObject.SetActive(true);
        SelectionManager.CurrentEditingMode = SelectionManager.EditingMode.AddGroup;
        SelectionManager.EditableDisplay.SetRollGroup(new RollGroup());
    }
}
