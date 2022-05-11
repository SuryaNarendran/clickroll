using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupDeleteButtonHandler : MonoBehaviour
{
    [SerializeField] RollGroupDisplay rollGroupDisplay;

    public void Delete()
    {
        rollGroupDisplay.DeleteReferencedGroup();
        RollGroupStorage.SaveToFile();
    }
}
