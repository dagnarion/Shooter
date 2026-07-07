using System.Collections.Generic;
using UnityEngine;

public static class SimplePool
{
    private static Dictionary<PoolType, Pool> poolHolder = new Dictionary<PoolType, Pool>();

    public static void Preload(GameUnit unit,int amount, Transform parent)
    {
        if(unit == null) return;
        if (!poolHolder.ContainsKey(unit.PoolType) || poolHolder[unit.PoolType] == null)
        {
            Pool pool = new Pool();
            pool.Preload(unit,amount,parent);
            poolHolder.Add(unit.PoolType,pool);
        }
    }

    public static T Spawn<T>(PoolType poolType,Vector3 pos,Quaternion rot) where T : GameUnit
    {
        if (!poolHolder.ContainsKey(poolType))
        {
            Debug.Log($"{poolType.ToString()} isn't preload");
            return null;
        }
        
        return poolHolder[poolType].Spawn(pos,rot) as T;
    }

    public static void Despawn(GameUnit unit)
    {
        if (!poolHolder.ContainsKey(unit.PoolType))
        {
            Debug.Log($"{unit.PoolType.ToString()} isn't preload");
            return;
        }
        poolHolder[unit.PoolType].Despawn(unit);
    }

    public static void Collect(PoolType poolType)
    {
        if (!poolHolder.ContainsKey(poolType))
        {
            Debug.Log($"{poolType.ToString()} isn't preload");
            return;
        }
        poolHolder[poolType].Collect();
    }

    public static void CollectAll()
    {
        foreach (var pool in poolHolder.Values)
        {
           pool.Collect(); 
        }
    }

    public static void Release()
    {
        foreach (var pool in poolHolder.Values)
        {
            pool.Release(); 
        }
    }
    
    
}

public class Pool
{
    private Transform parent;
    private GameUnit prefab;
    private List<GameUnit> actives = new List<GameUnit>();
    private Queue<GameUnit> inActives = new Queue<GameUnit>();

    public void Preload(GameUnit unit,int amount,Transform parent)
    {
        this.prefab = unit;
        this.parent = parent;
        for (int i = 0; i < amount; i++)
        {
            Despawn(Spawn(Vector3.zero,Quaternion.identity));
        }
    }

    public GameUnit Spawn(Vector3 pos,Quaternion rot)
    {
        GameUnit temp;
        if (inActives.Count == 0)
            temp = GameObject.Instantiate(prefab, parent);
        else
            temp = inActives.Dequeue();
        
        temp.TF.SetPositionAndRotation(pos,rot);
        temp.gameObject.SetActive(true);
        actives.Add(temp);
        return temp;
    }

    public void Despawn(GameUnit unit)
    {
        if (unit == null || !unit.gameObject.activeSelf) return;
        actives.Remove(unit);
        inActives.Enqueue(unit);
        unit.gameObject.SetActive(false);
    }

    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    public void Release()
    {
        Collect();
        while (inActives.Count > 0)
        {
            GameObject.Destroy(inActives.Dequeue().gameObject);
        }
    }   
}
