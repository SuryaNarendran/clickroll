using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataDisplayBase<T>
{
    GameObject gameObject { get; }
    Transform transform { get; }
    void Refresh();
    void SetData(T data, IRollGroupDisplay display);
    T currentData { get; }
}

