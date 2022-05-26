using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGroupButtonHandler : MonoBehaviour
{
    public void AddGroup()
    {
        RollGroupStorage.AddGroup(new RollGroup());
        SelectionManager.SelectedRollGroupIndex = RollGroupStorage.LoadedGroupCount - 1;
        SelectionManager.EditingOverlay.gameObject.SetActive(true);
        SelectionManager.EditableDisplay.SetRollGroup(SelectionManager.SelectedRollGroup.Clone());
    }
}
