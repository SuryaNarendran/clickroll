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
    [SerializeField] SelectableUISet selectableUISet;
    [SerializeField] bool abbreviatedDisplay = false;
    [SerializeField] bool highlightRerolls = false;
    [SerializeField] bool delayResult = false;
    [SerializeField] float countsPerSecond;

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
        selectableUISet?.DeselectAll();
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

    public void EvaluateAndRecord(RollOutcomeGroup rollOutcomeGroup)
    {
        this.rollOutcomeGroup = rollOutcomeGroup;
        EvaluateAndRecord();
    }

    public void EvaluateAndRecord(RollOutcomeGroup rollOutcomeGroup, List<RollDiceSelection> rollDiceSelections)
    {
        this.rollOutcomeGroup = rollOutcomeGroup;
        EvaluateAndRecord(rollDiceSelections);
    }

    public void EvaluateAndRecord(List<RollDiceSelection> rollDiceSelections)
    {
        if (rollOutcomeGroup != null)
        {
            rollOutcomeGroup.EvaluateAndRecord(rollDiceSelections);
            RefreshMembers();
            DisplayName();
            DisplayTotal();
        }
    }

    public void RefreshMembers()
    {
        foreach (IRollOutcomeDisplay rollDisplay in rollOutcomeDisplayFields)
        {
            rollDisplay.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollDisplay.gameObject);
            selectableUISet?.Remove(rollDisplay.transform);
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

            if (highlightRerolls)
            {
                int index = rollOutcomeGroup.rollOutcomes.IndexOf(rollOutcome);
                if (rollOutcomeGroup.rerollSelections != null &&
                    rollOutcomeGroup.rerollSelections.Any(x => x.rollIndex == index))
                    rollDisplay.HighlightRerolls(rollOutcomeGroup.rerollSelections.First(x => x.rollIndex == index));
            }


            go.transform.SetParent(contentHolder);
            selectableUISet?.Add(go.transform);
        }

        foreach (Modifier modifier in rollOutcomeGroup.rollGroup.modifiers)
        {
            GameObject go = PoolManager.SpawnObject(modifierDisplayPrefab);
            IModifierDisplay modifierDisplay = go.GetComponent<IModifierDisplay>();
            modifierDisplayFields.Add(modifierDisplay);
            modifierDisplay.SetData(modifier, this);

            go.transform.SetParent(contentHolder);
        }

        selectableUISet?.OnUIUpdateFinished();
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
        if (delayResult)
        {
            DisplayTotalWithCoroutine();
            return;
        }

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

    private void DisplayTotalWithCoroutine()
    {
        if (totalValueLabel != null)
        {
            if (rollOutcomeGroup != null)
                totalValueLabel.text = "0";
            else totalValueLabel.text = "";
        }
        if (summationLabel != null)
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

        StopAllCoroutines();
        StartCoroutine(DisplayTotalCoroutine());
    }

    private IEnumerator DisplayTotalCoroutine()
    {
        float resultDelayTime = 1f; //default value
        if(rollOutcomeGroup != null)
        {
            resultDelayTime = rollOutcomeGroup.Total / countsPerSecond;
        }

        if (summationLabel != null)
        {
            summationLabel.maxVisibleCharacters = 0;
        }

        float t = 0;
        while(t < resultDelayTime)
        {
            if (totalValueLabel != null)
            {
                if (rollOutcomeGroup != null)
                {
                    int finalValue = rollOutcomeGroup.Total;
                    int interpolatedValue = (int)(finalValue * t / resultDelayTime);
                    totalValueLabel.text = interpolatedValue.ToString();
                }
                else totalValueLabel.text = "";
            }

            if (summationLabel != null)
            {
                int interpolatedLetters = (int)(summationLabel.text.Length * t / resultDelayTime);
                summationLabel.maxVisibleCharacters = interpolatedLetters;
            }

            t += Time.deltaTime;

            //slows down as it approaches the limit
            if (t / resultDelayTime > 0.9f) yield return Frames(5);
            else if (t / resultDelayTime > 0.8f) yield return Frames(3);
            else if (t / resultDelayTime > 0.7f) yield return Frames(2);
            else yield return Frames(1);
        }

        if (summationLabel != null)
        {
            summationLabel.maxVisibleCharacters = summationLabel.text.Length;
        }

        if (totalValueLabel != null)
        {
            if (rollOutcomeGroup != null)
            {
                totalValueLabel.text = rollOutcomeGroup.Total.ToString();
            }
        }
    }

    private IEnumerator Frames(int frames)
    {
        if (frames <= 0) yield break;

        while(frames != 0)
        {
            frames--;
            yield return null;
        }
    }
}
