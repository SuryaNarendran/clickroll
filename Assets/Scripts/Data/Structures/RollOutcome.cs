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

    public int EvaluateAndRecord()
    {
        diceOutcomes = new int[roll.numberOfDice];

        int total = 0;
        for (int i = 0; i < roll.numberOfDice; i++)
        {
            int diceValue = Random.Range(0, roll.diceValue) + 1;
            diceOutcomes[i] = diceValue;
            total += diceValue;
        }

        return total;
    }

    public int Total
    {
        get => diceOutcomes.Sum();
    }
}
