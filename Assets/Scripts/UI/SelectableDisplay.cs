using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableDisplay : MonoBehaviour
{
    [SerializeField] Image highlightImage;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color normalColor;
    [SerializeField] bool allowUserSelect;
    [SerializeField] bool allowUserDeselect;

    public event System.Action<Transform> onSelect;
    public event System.Action<Transform> onDeselect;

    public void UserSelect()
    {
        if (allowUserSelect) Select();
    }

    public void UserDeselect()
    {
        if (allowUserDeselect) Deselect();
    }

    public void Select()
    {
        highlightImage.color = highlightedColor;
        onSelect?.Invoke(transform);
    }

    public void Deselect()
    {
        highlightImage.color = normalColor;
        onDeselect?.Invoke(transform);
    }

    public void SetHighlight(bool state)
    {
        highlightImage.color = state ? highlightedColor : normalColor;
    }
}
