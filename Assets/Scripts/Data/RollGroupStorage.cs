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
    [SerializeField] List<HistoryEntry> loadedHistory;

    bool loadedDataDirty = false;

    public static event System.Action onLoadedDataDirty;

    public static void LoadFromFile()
    {

        var saveData = JSONHandler.LoadData();
        Instance.loadedGroups = saveData.rollGroups.ToList();
        Instance.loadedHistory = saveData.history.ToList();
        //makes sure that at least one group exists.
        if (Instance.loadedGroups.Count == 0) Instance.loadedGroups.Add(new RollGroup());

        Instance.loadedDataDirty = true;
    }

    public static void SaveToFile()
    {
        JSONHandler.SaveData(Instance.loadedGroups, Instance.loadedHistory);
    }

    public static int LoadedGroupCount => Instance.loadedGroups.Count;
    public static int LoadedHistoryCount => Instance.loadedHistory.Count;

    public static IReadOnlyList<RollGroup> LoadedRollGroups { get => Instance.loadedGroups; } 
    public static IReadOnlyList<HistoryEntry> LoadedHistory { get => Instance.loadedHistory; }
    public static IReadOnlyList<RollOutcomeGroup> LoadedHistoryOutcomes 
    { 
        get => Instance.loadedHistory.Select(x => x.rollOutcomeGroup).ToList(); 
    }

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
            onLoadedDataDirty?.Invoke();
            SelectionManager.SelectedRollGroup = newData;
        }

        Instance.loadedDataDirty = true;
    }

    public static void AddGroup(RollGroup group)
    {
        if (Instance.loadedGroups.Contains(group) == false)
            Instance.loadedGroups.Add(group);

        Instance.loadedDataDirty = true;
    }

    public static void RemoveGroup(RollGroup group)
    {
        if(Instance.loadedGroups.Contains(group))
            Instance.loadedGroups.Remove(group);

        //makes sure that at least one group exists.
        if (Instance.loadedGroups.Count == 0) Instance.loadedGroups.Add(new RollGroup());

        Instance.loadedDataDirty = true;
    }

    public static RollOutcomeGroup GetHistoryOutcomeAtIndex(int index)
    {
        return Instance.loadedHistory.ElementAtOrDefault(index).rollOutcomeGroup;
    }

    public static string GetHistoryNotesAtIndex(int index)
    {
        return Instance.loadedHistory.ElementAtOrDefault(index).notes;
    }

    public static int IndexOf(RollOutcomeGroup group)
    {
        HistoryEntry entry = Instance.loadedHistory.FirstOrDefault(x => x.rollOutcomeGroup == group);
        if (entry == null) return -1;
        return Instance.loadedHistory.IndexOf(entry);
    }

    public static void AddHistory(RollOutcomeGroup group, int positionFromLast = 0)
    {
        HistoryEntry entry = new HistoryEntry(group);
        Instance.loadedHistory.Insert(positionFromLast, entry);
        Instance.loadedDataDirty = true;
    }

    public static void SetNotes(string notes, int index)
    {
        var history = Instance.loadedHistory.ElementAtOrDefault(index);
        if (history == null) return;
        history.notes = notes;
    }
    public static void SetNotes(string notes, RollOutcomeGroup group)
    {
        SetNotes(notes, IndexOf(group));
    }

    public static void ClearAllHistory()
    {
        Instance.loadedHistory.Clear();
        Instance.loadedDataDirty = true;
    }

    public static void TriggerRefresh()
    {
        onLoadedDataDirty?.Invoke();
    }

    private void Start()
    {
        LoadFromFile();
    }

    private void Update()
    {
        if (loadedDataDirty)
        {
            onLoadedDataDirty?.Invoke();
            loadedDataDirty = false;
        }
    }
}
