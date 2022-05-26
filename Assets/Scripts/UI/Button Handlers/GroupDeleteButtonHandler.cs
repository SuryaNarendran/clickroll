using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupDeleteButtonHandler : MonoBehaviour
{
    public void Delete()
    {
        SelectionManager.ActiveGroupDisplay.DeleteReferencedGroup();
        RollGroupStorage.SaveToFile();
    }
}
