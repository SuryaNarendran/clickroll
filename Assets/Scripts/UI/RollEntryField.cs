using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollEntryField : MonoBehaviour, IRollDisplay
{
    [SerializeField] Roll roll;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField numberOfDiceInput;
    [SerializeField] TMP_InputField diceValueInput;
    [SerializeField] Button deleteButton;

    private IRollGroupDisplay rollGroupDisplay;

    private void Awake()
    {
        nameInput.onEndEdit.AddListener(UpdateName);
        numberOfDiceInput.onEndEdit.AddListener(UpdateNumberOfDice);
        diceValueInput.onEndEdit.AddListener(UpdateDiceValue);
        deleteButton.onClick.AddListener(Remove);
    }

    public void Refresh()
    {
        nameInput.text = roll.name;
        numberOfDiceInput.text = roll.numberOfDice.ToString();
        diceValueInput.text = roll.diceValue.ToString();
    }

    public void SetData(Roll roll, IRollGroupDisplay rollGroupDisplay = null)
    {
        this.roll = roll;
        this.rollGroupDisplay = rollGroupDisplay;
        Refresh();
    }

    public void UpdateName(string value)
    {
        roll.name = value;
        UpdateData();
    }

    public void UpdateNumberOfDice(string value)
    {
        if(int.TryParse(value, out int intValue))
        {
            roll.numberOfDice = intValue;
            UpdateData();
        }
    }

    public void UpdateDiceValue(string value)
    {
        if (int.TryParse(value, out int intValue))
        {
            roll.diceValue = intValue;
            UpdateData();
        }
    }

    public void Remove()
    {
        if(rollGroupDisplay is RollGroupDisplay && rollGroupDisplay != null)
        {
            (rollGroupDisplay as RollGroupDisplay).RemoveRoll(this);
        }

    }

    private void UpdateData()
    {
        if (rollGroupDisplay is RollGroupDisplay && rollGroupDisplay != null)
        {
            (rollGroupDisplay as RollGroupDisplay).UpdateRollGroupData(this, roll);
        }
    }
}
