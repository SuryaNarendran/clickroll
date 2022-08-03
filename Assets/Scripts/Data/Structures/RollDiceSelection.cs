using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RollDiceSelection
{
    [SerializeField] public int rollIndex;
    [SerializeField] public int[] diceExclusions; //NOTE:make sure to deep copy this

    public RollDiceSelection(int rollIndex, int[] diceExclusions)
    {
        this.rollIndex = rollIndex;
        this.diceExclusions = diceExclusions;
    }

    public RollDiceSelection(int rollIndex) : this()
    {
        this.rollIndex = rollIndex;
        diceExclusions = new int[0];
    }

    public RollDiceSelection Clone()
    {
        int[] clonedDiceExclusions = diceExclusions.Clone() as int[];
        return new RollDiceSelection(rollIndex, clonedDiceExclusions);
    }
}
