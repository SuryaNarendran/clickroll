using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentlyLoadedLabel : Singleton<CurrentlyLoadedLabel>
{
    [SerializeField] TMP_Text currentlyLoadedText;
    public static void UpdateText(string text)
    {
        Instance.currentlyLoadedText.text = text;
    }
}
