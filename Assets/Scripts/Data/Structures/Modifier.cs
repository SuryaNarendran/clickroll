using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Modifier: IRollGroupMember
{
    [SerializeField] public int value;
    [SerializeField] public string name;
    public string Name { get => name; set => name = value; }

    public Modifier(int value, string name)
    {
        this.value = value;
        this.name = name;
    }

    public int Evaluate()
    {
        return value;
    }
}
