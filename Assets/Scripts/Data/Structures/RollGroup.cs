using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RollGroup
{
    [SerializeField] public List<Roll> rolls;
    [SerializeField] public List<Modifier> modifiers;
    [SerializeField] public string name;

    public RollGroup()
    {
        rolls = new List<Roll>();
        modifiers = new List<Modifier>();
    }

    public RollGroup(List<Roll> rolls, List<Modifier> modifiers, string name)
    {
        this.rolls = rolls;
        this.modifiers = modifiers;
        this.name = name;
    }


    public int Evaluate()
    {
        int total = 0;
        foreach(Roll roll in rolls)
        {
            total += roll.Evaluate();
        }
        
        foreach(Modifier modifier in modifiers)
        {
            total += modifier.Evaluate();
        }

        return total;
    }

    public RollGroup Clone()
    {
        return new RollGroup(new List<Roll>(rolls), new List<Modifier>(modifiers), name);
    }
}
