using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RollHistoryDisplay : Singleton<RollHistoryDisplay>
{
    [SerializeField] GameObject rollOutcomeGroupDisplayPrefab;
    [SerializeField] RectTransform contentHolder;

    private List<RollOutcomesDisplay> groupDisplayBoxes;

    public SelectableDisplay currentlySelected { get; private set; }

    private void Awake()
    {
        groupDisplayBoxes = new List<RollOutcomesDisplay>();

        RollGroupStorage.onLoadedDataDirty += RefreshMembers;

        SelectionManager.onHistoryGroupSelected += UpdateHighlighting;
    }

    public void RefreshMembers()
    {
        foreach (RollOutcomesDisplay rollOutcomeBox in groupDisplayBoxes)
        {
            rollOutcomeBox.GetComponent<SelectableDisplay>().onSelect -= OnBoxSelected;

            rollOutcomeBox.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollOutcomeBox.gameObject);
        }
        groupDisplayBoxes.Clear();

        foreach (RollOutcomeGroup rollOutcome in RollGroupStorage.LoadedHistory)
        {
            GameObject go = PoolManager.SpawnObject(rollOutcomeGroupDisplayPrefab);
            RollOutcomesDisplay rollOutcomeDisplay = go.GetComponent<RollOutcomesDisplay>();
            var selectable = go.GetComponent<SelectableDisplay>();
            selectable.onSelect += OnBoxSelected;
            selectable.SetHighlight(false);


            groupDisplayBoxes.Add(rollOutcomeDisplay);
            rollOutcomeDisplay.SetRollOutcomeGroup(rollOutcome);

            go.transform.SetParent(contentHolder);
        }

        //ensures that the vertical layout group arranges the spawned children correctly
        //don't ask me why this works properly only if you call it twice??
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);

        if (CurrentlySelectedDisplay)
        {
            currentlySelected = CurrentlySelectedDisplay.GetComponent<SelectableDisplay>();
            currentlySelected.SetHighlight(true);
            //NOTE: selection does not happen properly at start up
        }
    }

    public void SelectGroup(RollOutcomesDisplay rollOutcomesDisplay)
    {
        SelectionManager.SelectedHistoryGroup = 
            RollGroupStorage.GetHistoryAtIndex(groupDisplayBoxes.IndexOf(rollOutcomesDisplay));
    }

    private void OnBoxSelected(Transform box)
    {
        RollOutcomesDisplay rollOutcomesDisplay = box.GetComponent<RollOutcomesDisplay>();
        SelectGroup(rollOutcomesDisplay);
    }

    private void UpdateHighlighting()
    {
        currentlySelected?.SetHighlight(false);
        if (CurrentlySelectedDisplay)
        {
            currentlySelected = CurrentlySelectedDisplay.GetComponent<SelectableDisplay>();
            currentlySelected.SetHighlight(true);
        }
    }

    private RollOutcomesDisplay CurrentlySelectedDisplay
        => groupDisplayBoxes.FirstOrDefault(x => x.HoldsOutcomeGroup(SelectionManager.SelectedHistoryGroup));
}
