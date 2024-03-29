using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollButtonHandler : MonoBehaviour
{
    [SerializeField] RerollSelection rerollSelection;
    public void OnPressed()
    {
        if (rerollSelection == null) return;
        rerollSelection.Reroll(false);
    }
}
