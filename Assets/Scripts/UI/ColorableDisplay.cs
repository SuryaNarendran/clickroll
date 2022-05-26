using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorableDisplay : MonoBehaviour
{
    [SerializeField] Graphic targetGraphic;

    Color baseColor;

    private void Awake()
    {
        baseColor = targetGraphic.color;
    }

    public void SetColor(Color color, bool additive=false)
    {
        if (additive) targetGraphic.color += color;
        else targetGraphic.color = color;
    }

    public void ResetColor()
    {
        targetGraphic.color = baseColor;
    }
}
