using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RollStorageDisplay : MonoBehaviour
{
    [SerializeField] GameObject rollGroupDisplayPrefab;
    [SerializeField] RectTransform contentHolder;
    [SerializeField] SelectableUISet selectableUISet;

    private List<RollGroupDisplay> groupDisplayBoxes;

    private void Awake()
    {
        groupDisplayBoxes = new List<RollGroupDisplay>();
        RollGroupStorage.onLoadedDataDirty += RefreshMembers;
    }

    public void RefreshMembers()
    {
        foreach (RollGroupDisplay rollGroupBox in groupDisplayBoxes)
        {
            selectableUISet.Remove(rollGroupBox.transform);

            rollGroupBox.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollGroupBox.gameObject);
        }
        groupDisplayBoxes.Clear();

        foreach (RollGroup rollGroup in RollGroupStorage.LoadedRollGroups)
        {
            GameObject go = PoolManager.SpawnObject(rollGroupDisplayPrefab);
            RollGroupDisplay rollGroupDisplay = go.GetComponent<RollGroupDisplay>();
            selectableUISet.Add(go.transform);

            groupDisplayBoxes.Add(rollGroupDisplay);
            rollGroupDisplay.SetRollGroup(rollGroup);

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
