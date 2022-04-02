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

        public JsonSave(RollGroup[] rollGroups)
        {
            this.rollGroups = rollGroups;
        }
    }

    public static RollGroup[] LoadRollGroups()
    {
        if (File.Exists(path) == false) return new RollGroup[0];

        string jsonValue = File.ReadAllText(path);
        JsonSave jsonSave = JsonUtility.FromJson<JsonSave>(jsonValue);

        return jsonSave.rollGroups;
    }

    public static void SaveRollGroups(List<RollGroup> rollGroups)
    {
        JsonSave jsonSave = new JsonSave(rollGroups.ToArray());
        string jsonValue = JsonUtility.ToJson(jsonSave);
        File.WriteAllText(path, jsonValue);
    }
}
