using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] List<Tab> tabs;
    [SerializeField] Color tabFadedColor;
    [SerializeField] Color tabActiveColor;

    public Tab activeTab { get; private set; }

    private void Start()
    {
        SelectTab(0);
    }

    public void SelectTab(int index)
    {
        activeTab = tabs[index];
        foreach(Tab tab in tabs)
        {
            tab.tabLabelGraphic.color = tabFadedColor;
            tab.tabUI.gameObject.SetActive(false);
        }

        activeTab.tabUI.gameObject.SetActive(true);
        activeTab.tabLabelGraphic.color = tabActiveColor;
    }
}

[System.Serializable]
public struct Tab
{
    [SerializeField] public string name;
    [SerializeField] public Transform tabUI;
    [SerializeField] public Image tabLabelGraphic;
}
