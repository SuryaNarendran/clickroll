using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRollDisplay
{
    void Refresh();
    void SetData(Roll roll, IRollGroupDisplay rollGroupDisplay);
    Transform transform { get; }
    GameObject gameObject { get; }
}
