using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct RollOutcome
{
    [SerializeField] public Roll roll;
    [SerializeField] public int[] diceOutcomes;

    public RollOutcome(Roll roll)
    {
        this.roll = roll;
        diceOutcomes = new int[roll.numberOfDice];
    }

    public RollOutcome(Roll roll, int[] diceOutcomes)
    {
        this.roll = roll;
        this.diceOutcomes = diceOutcomes;
    }

    public int EvaluateAndRecord()
    {
        int total = 0;
        for (int i = 0; i < roll.numberOfDice; i++)
        {
            diceOutcomes[i] = Random.Range(0, roll.diceValue) + 1;
            total += diceOutcomes[i];
        }

        return total;
    }

    public int EvaluateAndRecord(int[] diceExclusion)
    {
        int total = 0;
        for (int i = 0; i < roll.numberOfDice; i++)
        {
            if (diceExclusion == null || diceExclusion.Contains(i) == false)
                diceOutcomes[i] = Random.Range(0, roll.diceValue) + 1;
            total += diceOutcomes[i];
        }

        return total;
    }

    public int Total
    {
        get => diceOutcomes.Sum();
    }

    public RollOutcome Clone()
    {
        int[] diceOutcomesCopy = new int[diceOutcomes.Length];
        diceOutcomes.CopyTo(diceOutcomesCopy,0);
        return new RollOutcome(roll, diceOutcomesCopy);
    }
}
