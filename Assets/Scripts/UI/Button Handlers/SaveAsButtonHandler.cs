using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAsButtonHandler : MonoBehaviour
{
    public void Save()
    {
        string path = FileBrowserHandler.SaveFileDialog();
        if(path.Length > 0)
        {
            RollGroupStorage.SaveToFile(path);
            RollGroupStorage.LoadFromFile(path);
        }
    }
}
