using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollButtonHandler : MonoBehaviour
{
    public void OnPressed()
    {
        RollGroupDisplay currentDisplay = SelectionManager.ActiveGroupDisplay;

        RollGroup currentRollGroup = currentDisplay.GetRollGroupCopy();

        SelectionManager.LastRolledDisplay.EvaluateAndRecord(currentRollGroup);
        RollGroupStorage.AddHistory(SelectionManager.LastRolledDisplay.GetRollOutcomeGroupCopy());
        RollGroupStorage.SaveToFile();
    }
}
