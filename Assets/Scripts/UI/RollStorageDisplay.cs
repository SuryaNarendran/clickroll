using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RollStorageDisplay : MonoBehaviour
{
    [SerializeField] GameObject rollGroupDisplayPrefab;
    [SerializeField] RectTransform contentHolder;

    private List<RollGroupDisplay> groupDisplayBoxes;

    public SelectableDisplay currentlySelected { get; private set; }

    private void Awake()
    {
        groupDisplayBoxes = new List<RollGroupDisplay>();

        RollGroupStorage.onLoadedGroupsUpdated += RefreshMembers;
    }

    private void OnEnable()
    {
        RefreshMembers();

        SelectGroup(groupDisplayBoxes[0]);
        SelectableDisplay defaultBox = groupDisplayBoxes[0].GetComponent<SelectableDisplay>();
        currentlySelected = defaultBox;
        currentlySelected.SetHighlight(true);
        
    }

    public void RefreshMembers()
    {
        foreach (RollGroupDisplay rollGroupBox in groupDisplayBoxes)
        {
            rollGroupBox.GetComponent<SelectableDisplay>().onSelect -= OnBoxSelected;

            rollGroupBox.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollGroupBox.gameObject);
        }
        groupDisplayBoxes.Clear();

        foreach (RollGroup rollGroup in RollGroupStorage.LoadedRollGroups)
        {
            GameObject go = PoolManager.SpawnObject(rollGroupDisplayPrefab);
            RollGroupDisplay rollGroupDisplay = go.GetComponent<RollGroupDisplay>();
            var selectable = go.GetComponent<SelectableDisplay>();
            selectable.onSelect += OnBoxSelected;
            selectable.SetHighlight(false);


            groupDisplayBoxes.Add(rollGroupDisplay);
            rollGroupDisplay.SetRollGroup(rollGroup);

            go.transform.SetParent(contentHolder);
        }

        //ensures that the vertical layout group arranges the spawned children correctly
        //don't ask me why this works properly only if you call it twice??
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);

        RollGroupDisplay selectedGroupDisplay = groupDisplayBoxes.FirstOrDefault(x => x.HoldsGroup(SelectionManager.SelectedRollGroup));
        if (selectedGroupDisplay)
        {
            currentlySelected = selectedGroupDisplay.GetComponent<SelectableDisplay>();
            currentlySelected.SetHighlight(true);
            //NOTE: selection does not happen properly at start up
        }
    }

    public void SelectGroup(RollGroupDisplay rollGroupDisplay)
    {
        SelectionManager.SelectedRollGroup = RollGroupStorage.GetAtIndex(groupDisplayBoxes.IndexOf(rollGroupDisplay));
    }

    private void OnBoxSelected(Transform box)
    {
        currentlySelected?.Deselect();
        currentlySelected = box.GetComponent<SelectableDisplay>();
        RollGroupDisplay rollGroupDisplay = box.GetComponent<RollGroupDisplay>();
        SelectGroup(rollGroupDisplay);
    }
}
