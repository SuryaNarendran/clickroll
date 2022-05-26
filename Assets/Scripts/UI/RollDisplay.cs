using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollDisplay : MonoBehaviour, IRollDisplay
{
    [SerializeField] Roll roll;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text valueText;

    private IRollGroupDisplay rollGroupDisplay;

    public Roll currentData => roll;

    public void Refresh()
    {
        nameText.text = roll.name;
        valueText.text = string.Format("{0} d{1}", roll.numberOfDice, roll.diceValue);
    }

    public void SetData(Roll roll, IRollGroupDisplay rollGroupDisplay = null)
    {
        this.roll = roll;
        this.rollGroupDisplay = rollGroupDisplay;
        Refresh();
    }
}
