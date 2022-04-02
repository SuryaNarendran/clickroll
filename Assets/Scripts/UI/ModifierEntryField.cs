using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierEntryField : MonoBehaviour, IModifierDisplay
{
    [SerializeField] Modifier modifier;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField valueInput;
    [SerializeField] Button deleteButton;

    private IRollGroupDisplay rollGroupDisplay;

    private void Awake()
    {
        nameInput.onEndEdit.AddListener(UpdateName);
        valueInput.onEndEdit.AddListener(UpdateValue);
        deleteButton.onClick.AddListener(Remove);
    }

    public void Refresh()
    {
        nameInput.text = modifier.name;
        string modifierPrefix = (modifier.value >= 0) ? "+" : "-";
        valueInput.text = modifierPrefix + " " + Mathf.Abs(modifier.value);
    }

    public void SetData(Modifier modifier, IRollGroupDisplay rollGroupDisplay = null)
    {
        this.modifier = modifier;
        this.rollGroupDisplay = rollGroupDisplay;
        Refresh();
    }

    public void UpdateName(string value)
    {
        modifier.name = value;
        UpdateData();
    }

    public void UpdateValue(string value)
    {
        value = value.TrimStart('+');

        if (int.TryParse(value, out int intValue))
        {
            modifier.value = intValue;
            UpdateData();
        }
    }

    public void Remove()
    {
        if (rollGroupDisplay is RollGroupDisplay && rollGroupDisplay != null)
        {
            (rollGroupDisplay as RollGroupDisplay).RemoveModifier(this);
        }

    }

    private void UpdateData()
    {
        if (rollGroupDisplay is RollGroupDisplay && rollGroupDisplay != null)
        {
            (rollGroupDisplay as RollGroupDisplay).UpdateRollGroupData(this, modifier);
        }
    }
}
