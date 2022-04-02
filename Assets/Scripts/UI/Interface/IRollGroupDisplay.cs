using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRollGroupDisplay 
{
    Transform transform { get; }
    GameObject gameObject { get; }
}
