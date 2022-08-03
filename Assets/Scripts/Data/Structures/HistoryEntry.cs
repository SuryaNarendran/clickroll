using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HistoryEntry
{
    [SerializeField] public RollOutcomeGroup rollOutcomeGroup;
    [SerializeField] public string notes;

    public HistoryEntry(RollOutcomeGroup rollOutcomeGroup)
    {
        this.rollOutcomeGroup = rollOutcomeGroup;
        this.notes = "";
    }
}
