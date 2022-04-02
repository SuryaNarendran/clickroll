using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierDisplay
{
    void Refresh();
    void SetData(Modifier modifier, IRollGroupDisplay rollGroupDisplay);
    Transform transform { get; }
    GameObject gameObject { get; }
}
