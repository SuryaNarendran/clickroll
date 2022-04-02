using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollOutcomeLabel : MonoBehaviour,IRollOutcomeDisplay
{
    [SerializeField] RollOutcome rollOutcome;
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] TMP_Text valueLabel;
    [SerializeField] TMP_Text outcomeLabel;


    private RollOutcomesDisplay rollOutcomesDisplay;

    public void Refresh()
    {
        nameLabel.text = rollOutcome.roll.name;
        valueLabel.text = string.Format("{0} d{1}", rollOutcome.roll.numberOfDice, rollOutcome.roll.diceValue);
        outcomeLabel.text = string.Join(" + ", rollOutcome.diceOutcomes) + " = " + rollOutcome.Total;
    }

    public void SetData(RollOutcome rollOutcome, RollOutcomesDisplay rollOutcomesDisplay = null)
    {
        this.rollOutcome = rollOutcome;
        this.rollOutcomesDisplay = rollOutcomesDisplay;
        Refresh();
    }
}
