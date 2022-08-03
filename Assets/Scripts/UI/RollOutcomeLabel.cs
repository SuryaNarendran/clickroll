using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class RollOutcomeLabel : MonoBehaviour,IRollOutcomeDisplay
{
    [SerializeField] RollOutcome rollOutcome;
    [SerializeField] TMP_Text nameLabel;
    [SerializeField] TMP_Text valueLabel;
    [SerializeField] TMP_Text outcomeLabel;

    private RollDiceSelection rollDiceSelection;
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

    public void HighlightRerolls(RollDiceSelection rollDiceSelection)
    {
        this.rollDiceSelection = rollDiceSelection;

        string outputString = "";
        for (int i = 0; i < rollOutcome.diceOutcomes.Length; i++)
        {
            if (rollDiceSelection.diceExclusions.Contains(i))
                outputString += rollOutcome.diceOutcomes[i].ToString();
            else
                outputString += "<color=blue>" + rollOutcome.diceOutcomes[i].ToString() + "</color>";

            outputString += " + ";
        }
        outputString = outputString.Substring(0, outputString.Length - 3); //removes the last " + "
        outcomeLabel.text = outputString + " = " + rollOutcome.Total;
    }
}
