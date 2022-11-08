using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButtonHandler : MonoBehaviour
{
    public void OnClicked()
    {
        ConfirmationPopup.Raise("Save and Quit Application?", SaveAndQuit);
    }

    public void SaveAndQuit()
    {
        RollGroupStorage.SaveToFile();
        Application.Quit();
    }
}
