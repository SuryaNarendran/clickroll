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

    int selectedRollGroupIndex;

    public static RollGroupDisplay ActiveGroupDisplay { get => Instance.activeGroupDisplay; }
    public static RollOutcomesDisplay LastRolledDisplay { get => Instance.lastRolledDisplay; }

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

    public static event System.Action onGroupSelected;

    private void Awake()
    {
        RollGroupStorage.onLoadedGroupsUpdated += UpdateSelectedGroupIndex;
        onGroupSelected += UpdateActiveSelectedDisplay;
    }

    private void UpdateSelectedGroupIndex()
    {
        selectedRollGroupIndex = Mathf.Clamp(selectedRollGroupIndex, 0, RollGroupStorage.LoadedGroupCount-1);
        UpdateActiveSelectedDisplay();
    }

    private void UpdateActiveSelectedDisplay()
    {
        ActiveGroupDisplay.SetRollGroup(SelectedRollGroup.Clone());
    }
}
