using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RollHistoryDisplay : Singleton<RollHistoryDisplay>
{
    [SerializeField] GameObject rollOutcomeGroupDisplayPrefab;
    [SerializeField] RectTransform contentHolder;
    [SerializeField] SelectableUISet selectableUISet;

    private List<RollOutcomesDisplay> groupDisplayBoxes;

    private void Awake()
    {
        groupDisplayBoxes = new List<RollOutcomesDisplay>();
        RollGroupStorage.onLoadedDataDirty += RefreshMembers;
    }

    public void RefreshMembers()
    {
        foreach (RollOutcomesDisplay rollOutcomeBox in groupDisplayBoxes)
        {
            selectableUISet.Remove(rollOutcomeBox.transform);

            rollOutcomeBox.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollOutcomeBox.gameObject);
        }
        groupDisplayBoxes.Clear();

        foreach (RollOutcomeGroup rollOutcome in RollGroupStorage.LoadedHistoryOutcomes)
        {
            GameObject go = PoolManager.SpawnObject(rollOutcomeGroupDisplayPrefab);
            RollOutcomesDisplay rollOutcomeDisplay = go.GetComponent<RollOutcomesDisplay>();

            selectableUISet.Add(rollOutcomeDisplay.transform);


            groupDisplayBoxes.Add(rollOutcomeDisplay);
            rollOutcomeDisplay.SetRollOutcomeGroup(rollOutcome);

            go.transform.SetParent(contentHolder);
        }

        //selectableUISet.SelectDefault();
        selectableUISet.OnUIUpdateFinished();

        //ensures that the vertical layout group arranges the spawned children correctly
        //don't ask me why this works properly only if you call it twice??
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
    }
}
