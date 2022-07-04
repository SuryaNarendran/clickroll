using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class SelectableDisplay : MonoBehaviour
{
    
    [SerializeField] Image highlightImage;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color normalColor;
    [SerializeField] public bool allowUserSelect;
    [SerializeField] public bool allowUserDeselect;

    [SerializeField] UnityEvent onSelected;
    [SerializeField] UnityEvent onDeselected;

    public event System.Action<Transform> onUserSelect;
    public event System.Action<Transform> onUserDeselect;

    private bool selected = false;
    public bool Selected => false;

    Selectable unitySelectableComponent;

    private void Awake()
    {
        unitySelectableComponent = GetComponent<Selectable>();
    }

    private void OnEnable()
    {
        //if (unitySelectableComponent) unitySelectableComponent.
    }

    public void UserSelect()
    {
        if (allowUserSelect)
        {
            onUserSelect?.Invoke(transform);
        }
    }

    public void UserDeselect()
    {
        if (allowUserDeselect)
        {
            onUserDeselect?.Invoke(transform);
        }
    }

    public void SetSelected(bool state)
    {
        selected = state;
        highlightImage.color = state ? highlightedColor : normalColor;

        if (selected) onSelected?.Invoke();
        else onDeselected?.Invoke();
    }

    public void UserToggleSelect()
    {
        if (selected) UserDeselect();
        else UserSelect();
    }
}
