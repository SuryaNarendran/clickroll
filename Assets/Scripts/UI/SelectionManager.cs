using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] RollGroupDisplay activeGroupDisplay;
    [SerializeField] RollOutcomesDisplay lastRolledDisplay;
    [SerializeField] RollOutcomesDisplay selectedHistoryDisplay;
    [SerializeField] RollGroupDisplay editableDisplay;
    [SerializeField] CheckInputValidity editingInputValidityCheck;
    [SerializeField] Canvas editingCanvas;

    [SerializeField] SelectableUISet rollSets;
    [SerializeField] SelectableUISet historyScroller;


    int selectedRollGroupIndex;
    int selectedHistoryGroupIndex;

    public static RollGroupDisplay ActiveGroupDisplay => Instance.activeGroupDisplay;
    public static RollOutcomesDisplay LastRolledDisplay => Instance.lastRolledDisplay;
    public static RollOutcomesDisplay SelectedHistoryDisplay => Instance.selectedHistoryDisplay;
    public static RollGroupDisplay EditableDisplay => Instance.editableDisplay;
    public static Canvas EditingOverlay => Instance.editingCanvas;

    public enum EditingMode { EditGroup, AddGroup}
    public static EditingMode CurrentEditingMode;

    public static RollGroup SelectedRollGroup
    {
        get => RollGroupStorage.GetAtIndex(SelectedRollGroupIndex);
        set
        {
            if (RollGroupStorage.LoadedRollGroups.Contains(value))
            {
                SelectedRollGroupIndex = RollGroupStorage.LoadedRollGroups.IndexOf(value);
                Instance.rollSets.Select(RollGroupStorage.LoadedRollGroups.IndexOf(value));
            } 
        }
    }

    public static int SelectedRollGroupIndex 
    {
        get => Instance.selectedRollGroupIndex;
        set
        {
            if(value >= 0 && value < RollGroupStorage.LoadedGroupCount)
            {
                Instance.selectedRollGroupIndex = value;
                Instance.rollSets.Select(value);
            }
        }
    }

    public static RollOutcomeGroup SelectedHistoryGroup
    {
        get => RollGroupStorage.GetHistoryAtIndex(SelectedHistoryGroupIndex);
        set
        {
            if (RollGroupStorage.LoadedHistory.Contains(value))
            {
                SelectedHistoryGroupIndex = RollGroupStorage.LoadedHistory.IndexOf(value);
                Instance.historyScroller.Select(RollGroupStorage.LoadedHistory.IndexOf(value));
            }
        }
    }

    public static int SelectedHistoryGroupIndex
    {
        get => Instance.selectedHistoryGroupIndex;
        set
        {
            if (value >= 0 && value < RollGroupStorage.LoadedHistoryCount)
            {
                SelectedHistoryGroupIndex = value;
                Instance.historyScroller.Select(value);
            }
        }
    }

    private void Awake()
    {
        rollSets.onSelect += UpdateActiveSelectedDisplay;
        historyScroller.onSelect += UpdateSelectedOutcomeDisplay;
        rollSets.onUIUpdateFinished += UpdateRollSetsUISelection;
        historyScroller.onUIUpdateFinished += UpdateHistoryUISelection;

        editingInputValidityCheck.FirstTimeSetup();
    }

    private void UpdateActiveSelectedDisplay(SelectableDisplay selectableDisplay)
    {
        selectedRollGroupIndex = rollSets.LastSelectionIndex;
        ActiveGroupDisplay.SetRollGroup(SelectedRollGroup);
    }

    private void UpdateSelectedOutcomeDisplay(SelectableDisplay selectableDisplay)
    {
        selectedHistoryGroupIndex = historyScroller.LastSelectionIndex;
        SelectedHistoryDisplay.SetRollOutcomeGroup(SelectedHistoryGroup);
    }

    private void UpdateRollSetsUISelection()
    {
        if (rollSets.Selectables.Count > 0) rollSets.Select(SelectedRollGroupIndex);
    }

    private void UpdateHistoryUISelection()
    {
        if(historyScroller.Selectables.Count > 0) historyScroller.Select(SelectedHistoryGroupIndex);
    }
}
