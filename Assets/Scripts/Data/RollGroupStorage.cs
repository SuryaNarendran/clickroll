using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

/// <summary>
/// manager class which holds currently loaded rollgroups
/// </summary>
public class RollGroupStorage : Singleton<RollGroupStorage>
{
    [SerializeField] List<RollGroup> loadedGroups;
    [SerializeField] List<HistoryEntry> loadedHistory;

    bool loadedDataDirty = false;

    public static event System.Action onLoadedDataDirty;

    public static void LoadFromFile(string path)
    {
        //if(JSONHandler.PathIsValid(path) == false)

        var saveData = JSONHandler.LoadData(path);
        Instance.loadedGroups = saveData.rollGroups.ToList();
        Instance.loadedHistory = saveData.history.ToList();
        //makes sure that at least one group exists.
        //if (Instance.loadedGroups.Count == 0) Instance.loadedGroups.Add(new RollGroup());

        Instance.loadedDataDirty = true;

        CurrentlyLoadedLabel.UpdateText(Path.GetFileName(path));
        SelectionManager.ClearAllDisplays();
    }

    public static void LoadFromFile()
    {
        string path = JSONHandler.GetLastAccessedPath();
        if (path == string.Empty || JSONHandler.PathIsValid(path) == false)
        {
            LoadNewFile(false);
            CurrentlyLoadedLabel.UpdateText("New File (Unsaved)");
        }
        else
        {
            LoadFromFile(path);
        }
    }

    public static void LoadNewFile(bool saveToNewFilename)
    {
        Instance.loadedGroups.Clear();
        Instance.loadedHistory.Clear();

        if (saveToNewFilename)
        {
            string path = FileBrowserHandler.SaveFileDialog();
            if (path == string.Empty)
            {
                CurrentlyLoadedLabel.UpdateText("New File (Unsaved)");
            }
            else
            {
                SaveToFile(path);
                CurrentlyLoadedLabel.UpdateText(Path.GetFileName(path));
            }
        }

        Instance.loadedDataDirty = true;
        SelectionManager.ClearAllDisplays();
    }

    public static void SaveToFile(string path)
    {
        JSONHandler.SaveData(Instance.loadedGroups, Instance.loadedHistory, path);
    }

    public static void SaveToFile()
    {
        string path = JSONHandler.GetLastAccessedPath();
        if (path == string.Empty)
        {
            path = FileBrowserHandler.SaveFileDialog();
            //throw new System.IO.FileNotFoundException("last accessed path was not found!");
        }
        SaveToFile(path);
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
        //if (Instance.loadedGroups.Count == 0) Instance.loadedGroups.Add(new RollGroup());

        Instance.loadedDataDirty = true;
    }

    public static RollOutcomeGroup GetHistoryOutcomeAtIndex(int index)
    {
        if (Instance.loadedHistory.Count > index)
        {
            return Instance.loadedHistory.ElementAtOrDefault(index).rollOutcomeGroup;
        }
        else return null;
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

    public static void RemoveHistory(HistoryEntry entry)
    {
        if (Instance.loadedHistory.Contains(entry))
            Instance.loadedHistory.Remove(entry);

        Instance.loadedDataDirty = true;
    }

    public static void RemoveHistory(int index)
    {
        if(index >= 0 && index < Instance.loadedHistory.Count)
        {
            HistoryEntry entry = Instance.loadedHistory[index];
            RemoveHistory(entry);
        }
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
