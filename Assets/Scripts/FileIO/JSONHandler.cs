using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JSONHandler
{
    static string path { get => Application.persistentDataPath + "/SaveData.json"; }

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

    public static JsonSave LoadData()
    {
        if (File.Exists(path) == false) return new JsonSave();

        string jsonValue = File.ReadAllText(path);
        JsonSave jsonSave = JsonUtility.FromJson<JsonSave>(jsonValue);

        if (jsonSave == null) return new JsonSave();

        return jsonSave;
    }

    public static void SaveData(List<RollGroup> rollGroups, List<HistoryEntry> history)
    {
        JsonSave jsonSave = new JsonSave(rollGroups.ToArray(), history.ToArray());
        string jsonValue = JsonUtility.ToJson(jsonSave);
        File.WriteAllText(path, jsonValue);
    }
}
