using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DiceSelector : MonoBehaviour
{
    [SerializeField] GameObject diceSelectablePrefab;
    [SerializeField] RerollSelection rerollSelection;
    [SerializeField] RectTransform myPanel; //not working for now
    [SerializeField] RectTransform panelAboveMe; //not working for now
    [SerializeField] int panelVerticalSize; //not working for now
    [SerializeField] SelectableUISet currentSelectionSet;
    [SerializeField] Transform contentHolder;
    [SerializeField] SelectableUISet rollOutcomesSelectableUISet;

    List<DiceSelectable> diceSelectables;
    RollOutcome rollOutcome;
    RollDiceSelection diceSelection;

    bool isOpen = false;

    private void OnEnable()
    {
        currentSelectionSet.onAnySelectionChange += ApplyChangesToRollSelection;
        SelectionManager.onSelectedHistoryIndexUpdated += Close;
        rollOutcomesSelectableUISet.onAnySelectionChange += OnRollOutcomesSelectionChange;
    }

    private void OnDisable()
    {
        if(currentSelectionSet != null)
            currentSelectionSet.onAnySelectionChange -= ApplyChangesToRollSelection;

        if (rollOutcomesSelectableUISet != null)
            rollOutcomesSelectableUISet.onAnySelectionChange -= OnRollOutcomesSelectionChange;

        SelectionManager.onSelectedHistoryIndexUpdated -= Close;
    }


    public void Setup(RollOutcome newRollOutcome, RollDiceSelection newSelection)
    {
        rollOutcome = newRollOutcome;
        diceSelection = newSelection;

        if (isOpen == false) Open();

        RefreshMembers();
    }

    public void Open()
    {
        if (isOpen) return;

        //myPanel.sizeDelta = new Vector2(myPanel.sizeDelta.x, panelVerticalSize);
        //panelAboveMe.sizeDelta = new Vector2(panelAboveMe.sizeDelta.x, panelAboveMe.sizeDelta.y - panelVerticalSize);
        isOpen = true;
    }

    public void Close()
    {
        if (isOpen == false) return;

        //myPanel.sizeDelta = new Vector2(myPanel.sizeDelta.x, 0);
        //panelAboveMe.sizeDelta = new Vector2(panelAboveMe.sizeDelta.x, panelAboveMe.sizeDelta.y + panelVerticalSize)

        ClearLayout();
        isOpen = false;
    }

    private void ApplyChangesToRollSelection()
    {
        List<int> updatedDiceExclusions = new List<int>();
        for(int index = 0; index < rollOutcome.diceOutcomes.Length; index++)
        {
            if(currentSelectionSet.SelectionIndices.Contains(index) == false)
            {
                updatedDiceExclusions.Add(index);
            }
        }
        diceSelection.diceExclusions = updatedDiceExclusions.ToArray();

        rerollSelection.SetDiceExclusions(diceSelection.Clone());

        if(currentSelectionSet.SelectionIndices.Count > 0)
        {
            rollOutcomesSelectableUISet.Select(diceSelection.rollIndex);
        }
        else if(currentSelectionSet.SelectionIndices.Count == 0)
        {
            rollOutcomesSelectableUISet.Deselect(diceSelection.rollIndex);
        }
    }

    private void OnRollOutcomesSelectionChange()
    {
        if (rollOutcomesSelectableUISet.SelectionIndices.Contains(diceSelection.rollIndex))
        {
            if(currentSelectionSet.SelectionIndices.Count == 0)
            {
                currentSelectionSet.SelectAll();
            }
        }

        if (rollOutcomesSelectableUISet.SelectionIndices.Contains(diceSelection.rollIndex) == false)
        {
            currentSelectionSet.DeselectAll();
        }
    }


    public void RefreshMembers()
    {
        currentSelectionSet.DeselectAll();

        foreach(DiceSelectable diceSelectable in diceSelectables)
        {
            diceSelectable.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(diceSelectable.gameObject);
            currentSelectionSet?.Remove(diceSelectable.transform);
        }

        foreach (int value in rollOutcome.diceOutcomes)
        {
            GameObject go = PoolManager.SpawnObject(diceSelectablePrefab);
            DiceSelectable diceSelectable = go.GetComponent<DiceSelectable>();
            diceSelectables.Add(diceSelectable);
            diceSelectable.LabelValue = value;

            go.transform.SetParent(contentHolder);
            currentSelectionSet?.Add(go.transform);
        }

        if (rollOutcomesSelectableUISet.SelectionIndices.Contains(diceSelection.rollIndex))
        {
            for (int index = 0; index < rollOutcome.diceOutcomes.Length; index++)
            {
                if (diceSelection.diceExclusions.Contains(index) == false)
                {
                    currentSelectionSet.Select(index);
                }
            }
        }
        else
        {
            currentSelectionSet.DeselectAll();
        }
    }

    private void ClearLayout()
    {
        currentSelectionSet.DeselectAll();

        foreach (DiceSelectable diceSelectable in diceSelectables)
        {
            diceSelectable.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(diceSelectable.gameObject);
            currentSelectionSet?.Remove(diceSelectable.transform);
        }
    }

    private void Awake()
    {
        diceSelectables = new List<DiceSelectable>();
    }
}
