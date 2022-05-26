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
    [SerializeField] Canvas editingCanvas;

    int selectedRollGroupIndex;
    int selectedHistoryGroupIndex;

    public static RollGroupDisplay ActiveGroupDisplay => Instance.activeGroupDisplay;
    public static RollOutcomesDisplay LastRolledDisplay => Instance.lastRolledDisplay;
    public static RollOutcomesDisplay SelectedHistoryDisplay => Instance.selectedHistoryDisplay;
    public static RollGroupDisplay EditableDisplay => Instance.editableDisplay;
    public static Canvas EditingOverlay => Instance.editingCanvas;

    public static RollGroup SelectedRollGroup
    {
        get => RollGroupStorage.GetAtIndex(Instance.selectedRollGroupIndex);
        set
        {
            if (RollGroupStorage.LoadedRollGroups.Contains(value))
            {
                Instance.selectedRollGroupIndex = RollGroupStorage.LoadedRollGroups.IndexOf(value);
                onGroupSelected?.Invoke();
            } 
        }
    }

    public static int SelectedRollGroupIndex
    {
        get => Instance.selectedRollGroupIndex;
        set
        {
            Instance.selectedRollGroupIndex = value;
            onGroupSelected?.Invoke();
        }
    }

    public static RollOutcomeGroup SelectedHistoryGroup
    {
        get => RollGroupStorage.GetHistoryAtIndex(Instance.selectedHistoryGroupIndex);
        set
        {
            if (RollGroupStorage.LoadedHistory.Contains(value))
            {
                Instance.selectedHistoryGroupIndex = RollGroupStorage.LoadedHistory.IndexOf(value);
                onHistoryGroupSelected?.Invoke();
            }
        }
    }

    public static int SelectedHistoryGroupIndex
    {
        get => Instance.selectedHistoryGroupIndex;
        set
        {
            Instance.selectedHistoryGroupIndex = value;
            onHistoryGroupSelected?.Invoke();
        }
    }

    public static event System.Action onGroupSelected;
    public static event System.Action onHistoryGroupSelected;

    private void Awake()
    {
        RollGroupStorage.onLoadedDataDirty += UpdateSelectedGroupIndex;
        RollGroupStorage.onLoadedDataDirty += UpdateSelectedOutcomeIndex;
        onGroupSelected += UpdateActiveSelectedDisplay;
        onHistoryGroupSelected += UpdateSelectedOutcomeDisplay;
    }

    private void UpdateSelectedGroupIndex()
    {
        selectedRollGroupIndex = Mathf.Clamp(selectedRollGroupIndex, 0, RollGroupStorage.LoadedGroupCount-1);
        UpdateActiveSelectedDisplay();
    }

    private void UpdateActiveSelectedDisplay()
    {
        ActiveGroupDisplay.SetRollGroup(SelectedRollGroup);
    }

    private void UpdateSelectedOutcomeIndex()
    {
        selectedHistoryGroupIndex = Mathf.Clamp(selectedHistoryGroupIndex, 0, RollGroupStorage.LoadedHistoryCount - 1);
        UpdateSelectedOutcomeDisplay();
    }

    private void UpdateSelectedOutcomeDisplay()
    {
        SelectedHistoryDisplay.SetRollOutcomeGroup(SelectedHistoryGroup);
    }
}
