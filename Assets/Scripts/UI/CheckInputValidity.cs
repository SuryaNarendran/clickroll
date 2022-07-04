using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckInputValidity : MonoBehaviour
{
    [SerializeField] RollGroupDisplay editableDisplay;
    [SerializeField] Transform editableFieldsParent;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Button saveDataButton;
    [SerializeField] Color warningColor;

    public event System.Action<bool> onInputValidityChecked;

    private bool initialized = false;

    private void Start()
    {
        if (!initialized) FirstTimeSetup();
    }

    public void FirstTimeSetup()
    {
        if (!initialized)
        {
            editableDisplay.onDataUpdated += ValidateData;
            onInputValidityChecked += SetSaveButtonEnabled;
            initialized = true;
        }
    }

    private void ValidateData()
    {
        RollGroup data = editableDisplay.GetRollGroupCopy();

        bool inputValid = true;
        editableDisplay.ResetAllColors();

        if(data == null)
        {
            onInputValidityChecked?.Invoke(false);
            return;
        }

        if (data.name == null || data.name.Length == 0)
        {
            inputValid = false;

            ColorableDisplay nameColor = nameInputField.GetComponent<ColorableDisplay>();
            if (nameColor) nameColor.SetColor(warningColor);
        }
        else
        {
            ColorableDisplay nameColor = nameInputField.GetComponent<ColorableDisplay>();
            if (nameColor) nameColor.ResetColor();
        }


        foreach (Roll roll in data.rolls)
        {
            if(roll.name.Length == 0)
            {
                inputValid = false;
                editableDisplay.SetColor(roll, warningColor);
            }
        }

        foreach (Modifier modifier in data.modifiers)
        {
            if (modifier.name.Length == 0)
            {
                inputValid = false;
                editableDisplay.SetColor(modifier, warningColor);
            }
        }

        onInputValidityChecked?.Invoke(inputValid);
    }

    private void SetSaveButtonEnabled(bool state)
    {
        saveDataButton.interactable = state;
    }
}
