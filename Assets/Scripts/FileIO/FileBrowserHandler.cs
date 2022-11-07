using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
public static class FileBrowserHandler
{
    static string defaultDirectory => Application.persistentDataPath;
    public static string OpenFileDialog()
    {
        var extensionFilters = new[] { new ExtensionFilter("ClickRoll Save Files", "json") };
        var results = StandaloneFileBrowser.OpenFilePanel("Open File", defaultDirectory, extensionFilters, false);
        if (results.Length == 0) return string.Empty;
        else return results[0];
    }

    public static string SaveFileDialog()
    {
        var extensionFilters = new[] { new ExtensionFilter("ClickRoll Save Files", "json") };
        return StandaloneFileBrowser.SaveFilePanel("Save File", defaultDirectory, "Rolls Data", extensionFilters);
    }
}
