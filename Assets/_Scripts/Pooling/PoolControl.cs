using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolControl : MonoBehaviour
{
    [SerializeField] private List<PoolAmount> pool;

    private void Awake()
    {
        foreach(var item in pool)
        {
            SimplePool.Preload(item.Unit,item.Amount,item.Parent);
        }
    }
}

[Serializable]
public class PoolAmount
{
    [field:SerializeField] public Transform Parent { get; private set; }
    [field:SerializeField] public GameUnit Unit { get; private set;}
    [field:SerializeField] public int Amount { get; private set; }
}