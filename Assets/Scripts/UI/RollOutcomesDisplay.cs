using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RollOutcomesDisplay : MonoBehaviour, IRollGroupDisplay
{
    [SerializeField] RollOutcomeGroup rollOutcomeGroup;
    [SerializeField] GameObject rollOutcomeDisplayPrefab;
    [SerializeField] GameObject modifierDisplayPrefab;
    [SerializeField] RectTransform contentHolder;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] TMP_Text totalValueLabel;

    private List<IRollOutcomeDisplay> rollOutcomeDisplayFields;
    private List<IModifierDisplay> modifierDisplayFields;

    private void Awake()
    {
        rollOutcomeDisplayFields = new List<IRollOutcomeDisplay>();
        modifierDisplayFields = new List<IModifierDisplay>();

        RefreshMembers();
        DisplayName();
    }

    public void EvaluateAndRecord()
    {
        rollOutcomeGroup.EvaluateAndRecord();
        RefreshMembers();
        DisplayName();
        DisplayTotal();
    }

    public void EvaluateAndRecord(RollGroup rollGroup)
    {
        rollOutcomeGroup = new RollOutcomeGroup(rollGroup);
        EvaluateAndRecord();
    }

    public void RefreshMembers()
    {
        foreach (IRollOutcomeDisplay rollDisplay in rollOutcomeDisplayFields)
        {
            rollDisplay.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollDisplay.gameObject);
        }
        rollOutcomeDisplayFields.Clear();

        foreach (RollOutcome rollOutcome in rollOutcomeGroup.rollOutcomes)
        {
            GameObject go = PoolManager.SpawnObject(rollOutcomeDisplayPrefab);
            IRollOutcomeDisplay rollDisplay = go.GetComponent<IRollOutcomeDisplay>();
            rollOutcomeDisplayFields.Add(rollDisplay);
            rollDisplay.SetData(rollOutcome, this);

            go.transform.SetParent(contentHolder);
        }

        foreach (IModifierDisplay modifierDisplay in modifierDisplayFields)
        {
            modifierDisplay.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(modifierDisplay.gameObject);
        }
        modifierDisplayFields.Clear();

        foreach (Modifier modifier in rollOutcomeGroup.rollGroup.modifiers)
        {
            GameObject go = PoolManager.SpawnObject(modifierDisplayPrefab);
            IModifierDisplay modifierDisplay = go.GetComponent<IModifierDisplay>();
            modifierDisplayFields.Add(modifierDisplay);
            modifierDisplay.SetData(modifier, this);

            go.transform.SetParent(contentHolder);
        }

        //ensures that the vertical layout group arranges the spawned children correctly
        //don't ask me why this works properly only if you call it twice??
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
    }

    public RollOutcomeGroup GetRollGroupCopy()
    {
        return rollOutcomeGroup.Clone();
    }

    private void DisplayName()
    {
        if (textLabel != null)
        {
            textLabel.text = rollOutcomeGroup.rollGroup.name;
        }
    }

    private void DisplayTotal()
    {
        if (totalValueLabel != null)
        {
            totalValueLabel.text = rollOutcomeGroup.Total.ToString();
        }
    }
}
