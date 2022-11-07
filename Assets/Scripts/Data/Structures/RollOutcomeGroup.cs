using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RollOutcomeGroup
{
    [SerializeField] public RollGroup rollGroup;
    [SerializeField] public RollOutcome[] rollOutcomes;
    [SerializeField] public RollDiceSelection[] rerollSelections;

    public RollOutcomeGroup(RollGroup rollGroup)
    {
        this.rollGroup = rollGroup;
        this.rerollSelections = new RollDiceSelection[0];
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
        this.rerollSelections = new RollDiceSelection[0];
    }

    public RollOutcomeGroup(RollGroup rollGroup, RollOutcome[] rollOutcomes, RollDiceSelection[] rerollSelections)
    {
        this.rollGroup = rollGroup;
        this.rollOutcomes = rollOutcomes;
        this.rerollSelections = rerollSelections;
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

    public int EvaluateAndRecord(List<RollDiceSelection> rollDiceSelections)
    {
        rerollSelections = rollDiceSelections.ToArray();

        int total = 0;
        for (int i = 0; i < rollOutcomes.Length; i++)
        {
            Roll roll = rollOutcomes[i].roll;
            if (rerollSelections.Any(x => x.rollIndex == i))
            {
                RollDiceSelection selection = rerollSelections.First(x => x.rollIndex == i);
                total += rollOutcomes[i].EvaluateAndRecord(selection.diceExclusions);
            }
            else total += rollOutcomes[i].Total;
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
        RollOutcome[] copyOfRollOutcomes = new RollOutcome[rollOutcomes.Length];
        
        RollDiceSelection[] copyOfRerollSelections;
        if (rerollSelections != null) copyOfRerollSelections = new RollDiceSelection[rerollSelections.Length];
        else copyOfRerollSelections = new RollDiceSelection[0];

        for (int i = 0; i < rollOutcomes.Length; i++)
        {
            copyOfRollOutcomes[i] = rollOutcomes[i].Clone();
        }

        if (rerollSelections != null)
        {
            for (int i = 0; i < rerollSelections.Length; i++)
            {
                copyOfRerollSelections[i] = rerollSelections[i].Clone();
            }
        }

        return new RollOutcomeGroup(rollGroup.Clone(), copyOfRollOutcomes, copyOfRerollSelections);
    }
}
