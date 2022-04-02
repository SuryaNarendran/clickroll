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
                Instance.UpdateActiveSelectedDisplay();
            } 
        }
    }

    public static int SelectedRollGroupIndex
    {
        get => Instance.selectedRollGroupIndex;
        set
        {
            Instance.selectedRollGroupIndex = value;
            Instance.UpdateActiveSelectedDisplay();
        }
    }

    private void Awake()
    {
        RollGroupStorage.onLoadedGroupsUpdated += UpdateSelectedGroupIndex;
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
}
