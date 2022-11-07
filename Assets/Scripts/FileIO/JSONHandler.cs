using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JSONHandler
{
    //static string path { get => Application.persistentDataPath + "/SaveData.json"; }
    static string lastAccessedDataPath => Application.persistentDataPath + "/LastLoadedPath.txt";

    [System.Serializable]
    public class JsonSave
    {
        public RollGroup[] rollGroups;
        public HistoryEntry[] history;

        public JsonSave(RollGroup[] rollGroups, HistoryEntry[] history)
        {
            this.rollGroups = rollGroups;
            this.history = history;
        }

        public JsonSave()
        {
            this.rollGroups = new RollGroup[0];
            this.history = new HistoryEntry[0];
        }
    }

    public static JsonSave LoadData(string path)
    {
        if (File.Exists(path) == false) return new JsonSave();

        string jsonValue = File.ReadAllText(path);
        JsonSave jsonSave = JsonUtility.FromJson<JsonSave>(jsonValue);

        if (jsonSave == null) return new JsonSave();
        RecordLastAccessedPath(path);

        return jsonSave;
    }

    public static void SaveData(List<RollGroup> rollGroups, List<HistoryEntry> history, string path)
    {
        JsonSave jsonSave = new JsonSave(rollGroups.ToArray(), history.ToArray());
        string jsonValue = JsonUtility.ToJson(jsonSave);
        File.WriteAllText(path, jsonValue);

        RecordLastAccessedPath(path);
    }

    public static string GetLastAccessedPath()
    {
        if (File.Exists(lastAccessedDataPath) == false) return string.Empty;

        string returnPath = File.ReadAllText(lastAccessedDataPath);
        return returnPath;
    }

    private static void RecordLastAccessedPath(string recordedPathData)
    {
        File.WriteAllText(lastAccessedDataPath, recordedPathData);
    }

    public static bool PathIsValid(string path)
    {
        return File.Exists(path);
    }
}
