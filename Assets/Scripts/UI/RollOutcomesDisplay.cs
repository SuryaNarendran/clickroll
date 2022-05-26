using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class RollOutcomesDisplay : MonoBehaviour, IRollGroupDisplay
{
    [SerializeField] RollOutcomeGroup rollOutcomeGroup;
    [SerializeField] GameObject rollOutcomeDisplayPrefab;
    [SerializeField] GameObject modifierDisplayPrefab;
    [SerializeField] RectTransform contentHolder;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] TMP_Text totalValueLabel;
    [SerializeField] TMP_Text summationLabel;
    [SerializeField] bool abbreviatedDisplay = false;

    private List<IRollOutcomeDisplay> rollOutcomeDisplayFields;
    private List<IModifierDisplay> modifierDisplayFields;

    private void Awake()
    {
        rollOutcomeDisplayFields = new List<IRollOutcomeDisplay>();
        modifierDisplayFields = new List<IModifierDisplay>();

        RefreshMembers();
        DisplayName();
        DisplayTotal();
    }

    public void SetRollOutcomeGroup(RollOutcomeGroup newData)
    {
        rollOutcomeGroup = newData;
        RefreshMembers();
        DisplayName();
        DisplayTotal();
    }

    public void EvaluateAndRecord()
    {
        if (rollOutcomeGroup != null)
        {
            rollOutcomeGroup.EvaluateAndRecord();
            RefreshMembers();
            DisplayName();
            DisplayTotal();
        }
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

        foreach (IModifierDisplay modifierDisplay in modifierDisplayFields)
        {
            modifierDisplay.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(modifierDisplay.gameObject);
        }
        modifierDisplayFields.Clear();

        //if in abbreviated display mode, don't spawn any more sub-fields
        if (abbreviatedDisplay) return;
        if (rollOutcomeGroup == null) return;

        foreach (RollOutcome rollOutcome in rollOutcomeGroup.rollOutcomes)
        {
            GameObject go = PoolManager.SpawnObject(rollOutcomeDisplayPrefab);
            IRollOutcomeDisplay rollDisplay = go.GetComponent<IRollOutcomeDisplay>();
            rollOutcomeDisplayFields.Add(rollDisplay);
            rollDisplay.SetData(rollOutcome, this);

            go.transform.SetParent(contentHolder);
        }

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

    public RollOutcomeGroup GetRollOutcomeGroupCopy()
    {
        return rollOutcomeGroup.Clone();
    }

    public bool HoldsOutcomeGroup(RollOutcomeGroup group)
    {
        return rollOutcomeGroup == group;
    }

    private void DisplayName()
    {
        if (textLabel != null)
        {
            if (rollOutcomeGroup != null)
                textLabel.text = rollOutcomeGroup.rollGroup.name;
            else textLabel.text = "";
        }
    }

    private void DisplayTotal()
    {
        if (totalValueLabel != null)
        {
            if (rollOutcomeGroup != null)
                totalValueLabel.text = rollOutcomeGroup.Total.ToString();
            else totalValueLabel.text = "";
        }
        if(summationLabel != null)
        {
            if (rollOutcomeGroup != null)
            {
                string output = "";
                foreach (RollOutcome outcome in rollOutcomeGroup.rollOutcomes)
                {
                    output += outcome.diceOutcomes.Sum().ToString() + " + ";
                }
                foreach (Modifier modifier in rollOutcomeGroup.rollGroup.modifiers)
                {
                    output += modifier.value.ToString() + " + ";
                }

                if (output.Length > 3)
                    output = output.Substring(0, output.Length - 3); //trim the last " + "

                summationLabel.text = output;
            }
            else summationLabel.text = "";           
        }
    }
}
