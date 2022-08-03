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
    [SerializeField] DiceSelector diceSelectorUI;
    [SerializeField] RerollSelection rerollSelection;


    int selectedRollGroupIndex;
    int selectedHistoryGroupIndex;

    public static RollGroupDisplay ActiveGroupDisplay => Instance.activeGroupDisplay;
    public static RollOutcomesDisplay LastRolledDisplay => Instance.lastRolledDisplay;
    public static RollOutcomesDisplay SelectedHistoryDisplay => Instance.selectedHistoryDisplay;
    public static RollGroupDisplay EditableDisplay => Instance.editableDisplay;
    public static DiceSelector DiceSelectorUI => Instance.diceSelectorUI;
    public static RerollSelection RerollSelectionUI => Instance.rerollSelection;
    public static Canvas EditingOverlay => Instance.editingCanvas;

    public static System.Action onSelectedHistoryIndexUpdated;
    public static System.Action onSelectedRollGroupUpdated;

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
                onSelectedRollGroupUpdated?.Invoke();
            }
        }
    }

    public static RollOutcomeGroup SelectedHistoryGroup
    {
        get => RollGroupStorage.GetHistoryOutcomeAtIndex(SelectedHistoryGroupIndex);
        set
        {
            if (RollGroupStorage.LoadedHistoryOutcomes.Contains(value))
            {
                SelectedHistoryGroupIndex = RollGroupStorage.IndexOf(value);
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
                Instance.selectedHistoryGroupIndex = value;
                onSelectedHistoryIndexUpdated?.Invoke();
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
        SelectedHistoryGroupIndex = historyScroller.LastSelectionIndex;
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
