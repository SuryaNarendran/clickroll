using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// manager class which holds currently loaded rollgroups
/// </summary>
public class RollGroupStorage : Singleton<RollGroupStorage>
{
    [SerializeField] List<RollGroup> loadedGroups;

    public static event System.Action onLoadedGroupsUpdated;

    public static void LoadFromFile()
    {
        Instance.loadedGroups = JSONHandler.LoadRollGroups().ToList();
        //makes sure that at least one group exists.
        if (Instance.loadedGroups.Count == 0) Instance.loadedGroups.Add(new RollGroup());

        onLoadedGroupsUpdated?.Invoke();
    }

    public static void SaveToFile()
    {
        JSONHandler.SaveRollGroups(Instance.loadedGroups);
    }

    public static int LoadedGroupCount => Instance.loadedGroups.Count;

    public static IReadOnlyList<RollGroup> LoadedRollGroups { get => Instance.loadedGroups; } 

    public static RollGroup GetAtIndex(int index)
    {
        return Instance.loadedGroups.ElementAtOrDefault(index);
    }

    public static int IndexOf(RollGroup group)
    {
        return Instance.loadedGroups.IndexOf(group);
    }

    public static void UpdateSelectedRollGroup(RollGroup newData)
    {
        int index = Instance.loadedGroups.IndexOf(SelectionManager.SelectedRollGroup);
        if (index >= 0)
        {
            Instance.loadedGroups[index] = newData;
            SelectionManager.SelectedRollGroup = newData;
            onLoadedGroupsUpdated?.Invoke();
        }
    }

    public static void AddGroup(RollGroup group)
    {
        if (Instance.loadedGroups.Contains(group) == false)
            Instance.loadedGroups.Add(group);

        onLoadedGroupsUpdated?.Invoke();
    }

    public static void RemoveGroup(RollGroup group)
    {
        if(Instance.loadedGroups.Contains(group))
            Instance.loadedGroups.Remove(group);

        //makes sure that at least one group exists.
        if (Instance.loadedGroups.Count == 0) Instance.loadedGroups.Add(new RollGroup());

        onLoadedGroupsUpdated?.Invoke();
    }

    public static void TriggerRefresh()
    {
        onLoadedGroupsUpdated?.Invoke();
    }

    private void Start()
    {
        LoadFromFile();
    }
}
