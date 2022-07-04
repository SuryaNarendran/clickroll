using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SelectableUISet : MonoBehaviour
{
    [SerializeField] private bool allowMultiSelect = false;
    [SerializeField] private bool forceAtLeastOneSelection = true;

    private List<SelectableDisplay> selectables = new List<SelectableDisplay>();
    private List<int> currentSelectionIndices = new List<int>();

    public List<int> SelectionIndices => currentSelectionIndices.ToList(); //returns copy

    public IReadOnlyList<SelectableDisplay> Selectables => selectables;
    public IReadOnlyList<SelectableDisplay> CurrentSelections 
    {
        get
        {
            List<SelectableDisplay> retval = new List<SelectableDisplay>();
            foreach(int index in currentSelectionIndices)
            {
                if (index < selectables.Count) retval.Add(selectables[index]);
            }
            return retval;
        }
    }

    public event System.Action<SelectableDisplay> onSelect;
    public event System.Action<SelectableDisplay> onDeselect;
    public event System.Action<SelectableDisplay> onUserSelect;
    public event System.Action<SelectableDisplay> onUserDeselect;
    public event System.Action onUIUpdateFinished;
    public event System.Action onAnySelectionChange;

    bool selectionDataDirty = false;

    public void Add(SelectableDisplay selectable)
    {
        selectables.Add(selectable);
        selectable.onUserSelect += OnUserSelected;
        selectable.onUserDeselect += OnUserDeselected;
        selectable.SetSelected(false);
        selectionDataDirty = true;
    }

    public void Add(Transform target) => Add(target.GetComponent<SelectableDisplay>());

    public void Remove(SelectableDisplay selectable)
    {
        selectables.Remove(selectable);
        selectable.onUserSelect -= OnUserSelected;
        selectable.onUserDeselect -= OnUserDeselected;
        selectionDataDirty = true;
    }

    public void Remove(Transform target) => Remove(target.GetComponent<SelectableDisplay>());

    public T GetAtIndex<T>(int index) where T : Component
    {
        return selectables[index].GetComponent<T>();
    }

    public T GetLastSelection<T>() where T : Component
    {
        return CurrentSelections.LastOrDefault()?.GetComponent<T>();
    }

    public int LastSelectionIndex
    {
        get
        {
            if (currentSelectionIndices.Count > 0 && Selectables.Count > 0)
            {
                return currentSelectionIndices.Last();
            }
            else return -1;
        }
    }


    public void OnUserSelected(Transform box)
    {
        SelectableDisplay selectable = box.GetComponent<SelectableDisplay>();
        Select(selectable);
        onUserSelect?.Invoke(selectable);
    }

    public void OnUserDeselected(Transform box)
    {
        SelectableDisplay selectable = box.GetComponent<SelectableDisplay>();
        Deselect(selectable);
        onUserDeselect?.Invoke(selectable);
    }

    public void Deselect(SelectableDisplay target)
    {
        if (CurrentSelections.Contains(target))
        {
            int index = Selectables.IndexOf(target);
            currentSelectionIndices.Remove(index);
            target.SetSelected(false);
            onDeselect?.Invoke(target);
            selectionDataDirty = true;

            if (CurrentSelections.Count == 0 && forceAtLeastOneSelection) SelectDefault();
        }
    }

    public void DeselectAll()
    {
        foreach (SelectableDisplay selectable in CurrentSelections)
        {
            selectable.SetSelected(false);
            selectionDataDirty = true;
        }
        currentSelectionIndices.Clear();
    }

    public void Select(SelectableDisplay target)
    {
        if (allowMultiSelect == false) DeselectAll();

        if (Selectables.Contains(target) && CurrentSelections.Contains(target) == false)
        {
            int index = Selectables.IndexOf(target);
            currentSelectionIndices.Add(index);
            target.SetSelected(true);
            onSelect?.Invoke(target);
            selectionDataDirty = true;
        }
    }

    public void Select(int index)
    {
        if (index >= 0 && index < selectables.Count)
        {
            Select(selectables[index]);
        }
            
        else Debug.LogError("index out of range!");
    }

    public void SelectDefault()
    {
        if (selectables.Count > 0)
        {
            DeselectAll();
            Select(selectables[0]);
        }
    }

    public void OnUIUpdateFinished()
    {
        var updatedList = new List<int>();
        foreach(int index in currentSelectionIndices)
        {
            if (index < Selectables.Count) updatedList.Add(index);
        }
        currentSelectionIndices = updatedList;

        onUIUpdateFinished?.Invoke();
        selectionDataDirty = true;
    }

    private void LateUpdate()
    {
        if (selectionDataDirty)
        {
            selectionDataDirty = false;
            onAnySelectionChange?.Invoke();
        }
    }
}
