using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNewButtonHandler : MonoBehaviour
{
    public void OpenNew()
    {
        string path = JSONHandler.GetLastAccessedPath();
        if(path == string.Empty)
        {
            path = FileBrowserHandler.SaveFileDialog();
        }
        RollGroupStorage.SaveToFile(path);
        RollGroupStorage.LoadNewFile(true);
    }
}
