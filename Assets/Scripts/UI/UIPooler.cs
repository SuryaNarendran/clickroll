using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPooler : Singleton<UIPooler>
{
    [SerializeField] GameObject rollFieldPrefab;
    [SerializeField] GameObject modifierFieldPrefab;
    [SerializeField] GameObject rollGroupUIPrefab;
    [SerializeField] GameObject rollEntryFieldPrefab;
    [SerializeField] GameObject modifierEntryFieldPrefab;
    [SerializeField] GameObject rollGroupDisplayPrefab;


    private void Awake()
    {
        PoolManager.SetRootTransform(transform);

        PoolManager.WarmPool(rollFieldPrefab, 10);
        PoolManager.WarmPool(modifierFieldPrefab, 10);
        PoolManager.WarmPool(rollEntryFieldPrefab, 5);
        PoolManager.WarmPool(modifierEntryFieldPrefab, 5);
        PoolManager.WarmPool(rollGroupDisplayPrefab, 20);
    }

    public static Transform releasedObjectParent { get => Instance.transform; }
}
