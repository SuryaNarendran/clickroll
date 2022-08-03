using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RerollSelection : MonoBehaviour
{
    [SerializeField] SelectableUISet rollSelectionUI;
    [SerializeField] Button rerollSelectedButton;

    RollDiceSelection[] allSelections;

    public RollDiceSelection GetDiceSelectionAtIndex(int index)
    {
        return allSelections[index].Clone();
    } 

    private void OnEnable()
    {
        SelectionManager.onSelectedHistoryIndexUpdated += SetupDiceSelections;

        rollSelectionUI.onAnySelectionChange += SetRerollButtonState;
        SetupDiceSelections();
        SetRerollButtonState();
    }

    private void OnDisable()
    {
        SelectionManager.onSelectedHistoryIndexUpdated -= SetupDiceSelections;
        rollSelectionUI.onAnySelectionChange -= SetRerollButtonState;
    }

    public List<RollDiceSelection> GetSelection()
    {
        List<RollDiceSelection> retval = new List<RollDiceSelection>();

        foreach(int index in rollSelectionUI.SelectionIndices)
        {
            retval.Add(allSelections[index]);
        }

        return retval;
    }

    public List<RollDiceSelection> GetSelectionOfAll()
    {
        return new List<RollDiceSelection>(allSelections);
    }

    public void SetupDiceSelections(RollOutcomeGroup outcomeGroup)
    {
        allSelections = new RollDiceSelection[outcomeGroup.rollOutcomes.Length];
        for(int i=0; i<outcomeGroup.rollOutcomes.Length; i++)
        {
            allSelections[i] = new RollDiceSelection(i);
        }
    }

    public void SetupDiceSelections()
    {
        SetupDiceSelections(SelectionManager.SelectedHistoryGroup);
    }

    public void SetDiceExclusions(RollDiceSelection newSelection)
    {
        if(newSelection.rollIndex < 0 || newSelection.rollIndex > allSelections.Length)
        {
            Debug.LogError("index out of range");
            return;
        }

        allSelections[newSelection.rollIndex] = newSelection;
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
        else SelectionManager.LastRolledDisplay.EvaluateAndRecord(groupToReroll, GetSelectionOfAll());

        RollOutcomeGroup rerolledOutcome = SelectionManager.LastRolledDisplay.GetRollOutcomeGroupCopy();

        //marks rerolls
        for(int i = 0; i < rerolledOutcome.rollOutcomes.Length; i++)
        {
            if(selection.Any(x => x.rollIndex == i) || onlySelected == false)
            {
                if (rerolledOutcome.rollOutcomes[i].roll.name.EndsWith("(*)") == false)
                    rerolledOutcome.rollOutcomes[i].roll.name += "(*)";
            }
        }

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
