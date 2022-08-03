using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDiceSelectorButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject rootGameObject;
    public void OnClicked()
    {
        //assuming the sibling index corresponds with the index of the
        //roll outcome within the roll outcome group.
        int index = rootGameObject.transform.GetSiblingIndex();
        RollOutcome rollOutcome = SelectionManager.SelectedHistoryGroup.rollOutcomes[index];
        RollDiceSelection rollDiceSelection = SelectionManager.RerollSelectionUI.GetDiceSelectionAtIndex(index);

        SelectionManager.DiceSelectorUI.Setup(rollOutcome, rollDiceSelection);
    }
}
