using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceSelectable : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    private int labelValue = 0;
    public int LabelValue
    {
        get => labelValue;
        set
        {
            labelValue = Mathf.Clamp(value, 0, 999);
            label.text = labelValue.ToString();
        }
    }
}
