using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Roll: IRollGroupMember
{
    [SerializeField] public int numberOfDice;
    [SerializeField] public int diceValue;
    [SerializeField] public string name;
    public string Name { get => name; set => name = value; }

    public Roll(int numberOfDice, int diceValue, string name)
    {
        this.numberOfDice = numberOfDice;
        this.diceValue = diceValue;
        this.name = name;
    }

    public int Evaluate()
    {
        int total = 0;
        for(int i = 0; i < numberOfDice; i++)
        {
            total += Random.Range(0, diceValue) + 1;
        }

        return total;
    }

    public override bool Equals(object obj)
    {
        return
            obj is Roll &&
            ((Roll)obj).diceValue == diceValue &&
            ((Roll)obj).numberOfDice == numberOfDice;
    }
}
