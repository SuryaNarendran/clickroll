using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGroupButtonHandler : MonoBehaviour
{
    public void AddGroup()
    {
        RollGroupStorage.AddGroup(new RollGroup());
        SelectionManager.SelectedRollGroupIndex = RollGroupStorage.LoadedGroupCount - 1;
    }
}
