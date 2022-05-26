using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupEditButtonHandler : MonoBehaviour
{
    public void EditSelectedGroup()
    {
        SelectionManager.EditingOverlay.gameObject.SetActive(true);
        SelectionManager.EditableDisplay.SetRollGroup(SelectionManager.SelectedRollGroup.Clone());
    }
}
