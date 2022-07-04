using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollSelectedButtonHandler : MonoBehaviour
{
    [SerializeField] RerollSelection rerollSelection;
    public void OnPressed()
    {
        if (rerollSelection == null) return;
        rerollSelection.Reroll(true);
    }
}
