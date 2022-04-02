using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RollOutcomeGroup
{
    [SerializeField] public RollGroup rollGroup;
    [SerializeField] public RollOutcome[] rollOutcomes;

    public RollOutcomeGroup(RollGroup rollGroup)
    {
        this.rollGroup = rollGroup;
        rollOutcomes = new RollOutcome[rollGroup.rolls.Count];

        for(int i = 0; i < rollGroup.rolls.Count; i++)
        {
            rollOutcomes[i] = new RollOutcome(rollGroup.rolls[i]);
        }
    }

    public RollOutcomeGroup(RollGroup rollGroup, RollOutcome[] rollOutcomes)
    {
        this.rollGroup = rollGroup;
        this.rollOutcomes = rollOutcomes;
    }

    public int EvaluateAndRecord()
    {
        int total = 0;
        for(int i=0;i < rollOutcomes.Length; i++)
        {
            total += rollOutcomes[i].EvaluateAndRecord();
        }

        foreach (Modifier modifier in rollGroup.modifiers)
        {
            total += modifier.Evaluate();
        }

        return total;
    }

    public int Total
    {
        get
        {
            int totalValue = 0;
            foreach (RollOutcome outcome in rollOutcomes) totalValue += outcome.Total;
            foreach (Modifier modifier in rollGroup.modifiers) totalValue += modifier.value;
            return totalValue;
        }
    }

    public RollOutcomeGroup Clone()
    {
        RollOutcome[] copyOfRollOutcomes = new RollOutcome[rollGroup.rolls.Count];
        rollOutcomes.CopyTo(copyOfRollOutcomes, 0);
        return new RollOutcomeGroup(rollGroup.Clone(), copyOfRollOutcomes);
    }
}
