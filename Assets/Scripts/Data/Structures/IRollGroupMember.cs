using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IRollGroupMember
{
    string Name { get; set; }
    int Evaluate();
}
