using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButtonHandler : MonoBehaviour
{
    [SerializeField] TMP_Text savedConfirmationText;
    [SerializeField] Color color;

    public void Save()
    {
        RollGroupStorage.SaveToFile();

        if(savedConfirmationText.color.a < 0.1f)
        {
            savedConfirmationText.color = color;
            Timer.Register(4f, () => savedConfirmationText.color = new Color(0, 0, 0, 0));
        }
    }
}
