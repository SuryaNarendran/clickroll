using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStoredDataUpdate : MonoBehaviour
{
    RollGroupDisplay target;

    private void Awake()
    {
        target = GetComponent<RollGroupDisplay>();
    }
    private void OnEnable()
    {
        target.onDataUpdated += UpdateStoredData;
    }

    private void OnDisable()
    {
        target.onDataUpdated -= UpdateStoredData;
    }

    private void UpdateStoredData()
    {
        RollGroupStorage.UpdateSelectedRollGroup(target.GetRollGroupCopy());
    }
}
