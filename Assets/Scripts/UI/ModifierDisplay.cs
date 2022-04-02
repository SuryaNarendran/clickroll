using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierDisplay : MonoBehaviour, IModifierDisplay
{
    [SerializeField] Modifier modifier;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text valueText;

    private IRollGroupDisplay rollGroupDisplay;

    public void Refresh()
    {
        nameText.text = modifier.name;
        string modifierPrefix = (modifier.value >= 0) ? "+" : "-";
        valueText.text = modifierPrefix + " " + Mathf.Abs(modifier.value);
    }

    public void SetData(Modifier modifier, IRollGroupDisplay rollGroupDisplay = null)
    {
        this.modifier = modifier;
        this.rollGroupDisplay = rollGroupDisplay;
        Refresh();
    }
}
