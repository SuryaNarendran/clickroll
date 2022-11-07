using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButtonHandler : MonoBehaviour
{
    public void Load()
    {
        string path = FileBrowserHandler.OpenFileDialog();
        if(path != string.Empty)
        {
            RollGroupStorage.LoadFromFile(path);
        }
    }
}
