using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollButtonHandler : MonoBehaviour
{
    public void OnPressed()
    {
        int historyIndex = SelectionManager.SelectedHistoryGroupIndex;

        RollGroup groupToReroll = SelectionManager.SelectedHistoryDisplay.GetRollOutcomeGroupCopy().rollGroup;
        if(groupToReroll.name.EndsWith("(Reroll)") == false)
            groupToReroll.name = groupToReroll.name + " (Reroll)"; 
        //NOTE: adjust this to output more helpful names for multiple consecutive rerolls

        SelectionManager.LastRolledDisplay.EvaluateAndRecord(groupToReroll);
        RollOutcomeGroup rerolledOutcome = SelectionManager.LastRolledDisplay.GetRollOutcomeGroupCopy();
        RollGroupStorage.AddHistory(rerolledOutcome, historyIndex);
        RollGroupStorage.SaveToFile();
    }
}
