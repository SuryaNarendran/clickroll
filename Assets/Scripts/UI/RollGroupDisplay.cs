using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;

public class RollGroupDisplay : MonoBehaviour, IRollGroupDisplay
{
    [SerializeField] RollGroup rollGroup;
    [SerializeField] GameObject rollDisplayPrefab;
    [SerializeField] GameObject modifierDisplayPrefab;
    [SerializeField] RectTransform contentHolder;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] TMP_InputField inputFieldLabel;
    [SerializeField] int memberDisplayLimit = 20;

    private List<IRollDisplay> rollDisplayFields;
    private List<IModifierDisplay> modifierDisplayFields;

    private Dictionary<IRollGroupMember, Color> colorCodeLookup; //NOTE: extend this later to handle index changes

    public event System.Action onDataUpdated;

    private bool initialized = false;

    private void Awake()
    {
        if (!initialized) FirstTimeSetup();

        RefreshMembers();
        DisplayName();

        if(inputFieldLabel != null)
        {
            inputFieldLabel.onEndEdit.AddListener(UpdateName);
        }
    }

    private void FirstTimeSetup()
    {
        rollDisplayFields = new List<IRollDisplay>();
        modifierDisplayFields = new List<IModifierDisplay>();
        colorCodeLookup = new Dictionary<IRollGroupMember, Color>();

        initialized = true;
    }

    public void RefreshMembers()
    {
        if (!initialized) FirstTimeSetup();

        foreach (IRollDisplay rollDisplay in rollDisplayFields)
        {
            rollDisplay.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(rollDisplay.gameObject);
        }
        rollDisplayFields.Clear();


        foreach (IModifierDisplay modifierDisplay in modifierDisplayFields)
        {
            modifierDisplay.transform.SetParent(UIPooler.releasedObjectParent);
            PoolManager.ReleaseObject(modifierDisplay.gameObject);
        }
        modifierDisplayFields.Clear();

        if (rollGroup == null) return;

        int memberCounter = 0;

        foreach (Roll roll in rollGroup.rolls)
        {
            if (memberCounter >= memberDisplayLimit) break;
            memberCounter++;

            GameObject go = PoolManager.SpawnObject(rollDisplayPrefab);
            IRollDisplay rollDisplay = go.GetComponent<IRollDisplay>();
            rollDisplayFields.Add(rollDisplay);
            rollDisplay.SetData(roll, this);

            ColorableDisplay colorableDisplay = go.GetComponent<ColorableDisplay>();
            if (colorableDisplay)
            {
                if (colorCodeLookup.ContainsKey(roll))
                    colorableDisplay.SetColor(colorCodeLookup[roll]);
                else colorableDisplay.ResetColor();
            }

            go.transform.SetParent(contentHolder);
        }

        foreach (Modifier modifier in rollGroup.modifiers)
        {
            if (memberCounter >= memberDisplayLimit) break;
            memberCounter++;

            GameObject go = PoolManager.SpawnObject(modifierDisplayPrefab);
            IModifierDisplay modifierDisplay = go.GetComponent<IModifierDisplay>();
            modifierDisplayFields.Add(modifierDisplay);
            modifierDisplay.SetData(modifier, this);

            ColorableDisplay colorableDisplay = go.GetComponent<ColorableDisplay>();
            if (colorableDisplay)
            {
                if (colorCodeLookup.ContainsKey(modifier))
                    colorableDisplay.SetColor(colorCodeLookup[modifier]);
                else colorableDisplay.ResetColor();
            }

            go.transform.SetParent(contentHolder);
        }

        //ensures that the vertical layout group arranges the spawned children correctly
        //don't ask me why this works properly only if you call it twice??
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentHolder);
    }

    public RollGroup GetRollGroupCopy()
    {
        return rollGroup.Clone();
    }

    public void SetRollGroup(RollGroup newGroup)
    {
        rollGroup = newGroup;
        RefreshMembers();
        DisplayName();
        onDataUpdated?.Invoke();
    }

    public void UpdateRollGroupData(IRollDisplay rollDisplay, Roll roll)
    {
        int rollIndex = rollDisplayFields.IndexOf(rollDisplay);
        rollGroup.rolls[rollIndex] = roll;

        onDataUpdated?.Invoke();
        RefreshMembers();
    }

    public void UpdateRollGroupData(IModifierDisplay modifierDisplay, Modifier modifier)
    {
        int modifierIndex = modifierDisplayFields.IndexOf(modifierDisplay);
        rollGroup.modifiers[modifierIndex] = modifier;

        onDataUpdated?.Invoke();
        RefreshMembers();
    }

    public void RemoveModifier(IModifierDisplay modifierDisplay)
    {
        int modifierIndex = modifierDisplayFields.IndexOf(modifierDisplay);
        rollGroup.modifiers.RemoveAt(modifierIndex);

        onDataUpdated?.Invoke();
        RefreshMembers();
    }

    public void RemoveRoll(IRollDisplay rollDisplay)
    {
        int rollIndex = rollDisplayFields.IndexOf(rollDisplay);
        rollGroup.rolls.RemoveAt(rollIndex);

        onDataUpdated?.Invoke();
        RefreshMembers();
    }

    public void AddRoll(Roll roll)
    {
        rollGroup.rolls.Add(roll);
        onDataUpdated?.Invoke();
        RefreshMembers();
    }

    public void AddRoll()
    {
        AddRoll(new Roll(1,6,""));
    }

    public void AddModifier(Modifier modifier)
    {
        rollGroup.modifiers.Add(modifier);
        onDataUpdated?.Invoke();
        RefreshMembers();
    }

    public void AddModifier()
    {
        AddModifier(new Modifier(1, ""));
    }

    public void UpdateName(string newName)
    {
        rollGroup.name = newName;
        onDataUpdated?.Invoke();
        DisplayName();
    }

    public bool HoldsGroup(RollGroup testGroup)
    {
        return rollGroup == testGroup;
    }

    public void DeleteReferencedGroup()
    {
        RollGroupStorage.RemoveGroup(rollGroup);
    }

    public int GroupStorageIndex()
    {
        return RollGroupStorage.IndexOf(rollGroup);
    }

    public void SetColor(IRollGroupMember rollGroupMember, Color color)
    {
        if (colorCodeLookup.ContainsKey(rollGroupMember))
            colorCodeLookup[rollGroupMember] = color;
        else colorCodeLookup.Add(rollGroupMember, color);       
    }

    public void ResetColor(IRollGroupMember rollGroupMember)
    {
        if (colorCodeLookup.ContainsKey(rollGroupMember))
            colorCodeLookup.Remove(rollGroupMember);
    }

    public void ResetAllColors()
    {
        colorCodeLookup.Clear();
    }

    private void DisplayName()
    {

        if(textLabel != null)
        {
            if (rollGroup == null) textLabel.text = "";
            else textLabel.text = rollGroup.name;
        }

        else if(inputFieldLabel != null)
        {
            if (rollGroup == null) inputFieldLabel.text = "";
            else inputFieldLabel.text = rollGroup.name;
        }
    }
}
