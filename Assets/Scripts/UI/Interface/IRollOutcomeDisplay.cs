using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRollOutcomeDisplay
{
    void SetData(RollOutcome rollOutcome, RollOutcomesDisplay rollOutcomesDisplay = null);
    void Refresh();
    Transform transform { get; }
    GameObject gameObject { get; }
}
