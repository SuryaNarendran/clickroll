using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RerollSelection : MonoBehaviour
{
    [SerializeField] SelectableUISet rollSelectionUI;
    [SerializeField] Button rerollSelectedButton;

    private void OnEnable()
    {
        rollSelectionUI.onAnySelectionChange += SetRerollButtonState;
        SetRerollButtonState();
    }

    private void OnDisable()
    {
        rollSelectionUI.onAnySelectionChange -= SetRerollButtonState;
    }

    public List<RollDiceSelection> GetSelection()
    {
        List<RollDiceSelection> retval = new List<RollDiceSelection>();

        foreach(int index in rollSelectionUI.SelectionIndices)
        {
            RollDiceSelection selection = new RollDiceSelection();
            selection.rollIndex = index;
            retval.Add(selection);
        }

        return retval;
    }

    public void Reroll(bool onlySelected)
    {
        int historyIndex = SelectionManager.SelectedHistoryGroupIndex;
        List<RollDiceSelection> selection = GetSelection();

        RollOutcomeGroup groupToReroll = SelectionManager.SelectedHistoryDisplay.GetRollOutcomeGroupCopy();
        if (groupToReroll.rollGroup.name.EndsWith("(Reroll)") == false)
            groupToReroll.rollGroup.name = groupToReroll.rollGroup.name + " (Reroll)";
        //NOTE: adjust this to output more helpful names for multiple consecutive rerolls

        if(onlySelected) SelectionManager.LastRolledDisplay.EvaluateAndRecord(groupToReroll, selection);
        else SelectionManager.LastRolledDisplay.EvaluateAndRecord(groupToReroll);

        RollOutcomeGroup rerolledOutcome = SelectionManager.LastRolledDisplay.GetRollOutcomeGroupCopy();
        RollGroupStorage.AddHistory(rerolledOutcome, historyIndex);
        RollGroupStorage.SaveToFile();
    }

    private void SetRerollButtonState()
    {
        if (rollSelectionUI.SelectionIndices.Count == 0)
            rerollSelectedButton.interactable = false;
        else rerollSelectedButton.interactable = true;
    }
}
