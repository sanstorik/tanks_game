using UnityEngine;
using System.Collections.Generic;

interface IPoolableComponent
{
    void Spawned();
    void Despawned();
}

public static class PoolingSystem 
{
    static Dictionary<GameObject, PrefabPool> mainPool;
    static Dictionary<GameObject, PrefabPool> _goToMainPool;
    public static Transform poolHolder;

    static PoolingSystem()
    {
        mainPool = new Dictionary<GameObject, PrefabPool>();
        _goToMainPool = new Dictionary<GameObject, PrefabPool>();
    }
    static public void ClearPools()
    {
        mainPool.Clear();
        _goToMainPool.Clear();
    }


    static public GameObject Spawn(GameObject objPrefab, Vector3 position)
    {
        if (!mainPool.ContainsKey(objPrefab))
        {
            mainPool.Add(objPrefab, new PrefabPool());
        }
        GameObject createdObj = mainPool[objPrefab].Spawn(objPrefab, position);
        _goToMainPool.Add(createdObj, mainPool[objPrefab]);

        return createdObj;
    }

    static public bool Despawn(GameObject obj)
    {
        if (!_goToMainPool.ContainsKey(obj))
        {
            Debug.LogError("POOL ERROR");
            return false;
        }
        PrefabPool pool = _goToMainPool[obj];
        if (pool.Despawn(obj))
        {
            _goToMainPool.Remove(obj);
        }

        return false;
    }

    static public void Prespawn(GameObject obj, int size, Transform poolHolder)
    {
        List<GameObject> spawned = new List<GameObject>();
        PoolingSystem.poolHolder = poolHolder;

        for (int i = 0; i < size; i++)
            spawned.Add(Spawn(obj, Vector3.up));

        for (int i = 0; i < size; i++)
            Despawn(spawned[i]);

        spawned.Clear();
    }
}

struct PoolablePrefabData
{
    public GameObject gameObject;
    public IPoolableComponent[] poolableComponents;
}

class PrefabPool
{
    Dictionary<GameObject, PoolablePrefabData> activeList;
    Queue<PoolablePrefabData> inActiveList;

    public PrefabPool()
    {
        activeList = new Dictionary<GameObject, PoolablePrefabData>();
        inActiveList = new Queue<PoolablePrefabData>();
    }

    public GameObject Spawn(GameObject obj, Vector3 position)
    {
        PoolablePrefabData data;
        if (inActiveList.Count >= 1)
        {
            data = inActiveList.Dequeue();
        }
        else
        {
            GameObject go = GameObject.Instantiate(obj);
            go.transform.SetParent(PoolingSystem.poolHolder, false);
            data = new PoolablePrefabData();
            data.gameObject = go;
            data.poolableComponents = go.GetComponents<IPoolableComponent>();
        }

        if (data.gameObject != null)
        {
            data.gameObject.SetActive(true);
            data.gameObject.transform.position = position;
        }


            foreach (var x in data.poolableComponents)
                x.Spawned();


        activeList.Add(data.gameObject, data);

        return data.gameObject;
    }

    public bool Despawn(GameObject obj)
    {
        PoolablePrefabData data;
        if (!activeList.ContainsKey(obj))
        {
            Debug.LogError("pool error");
            return false;
        }

        data = activeList[obj];

           foreach (var x in data.poolableComponents)
                x.Despawned();
           

        if (data.gameObject != null)
           data.gameObject.SetActive(false);

        activeList.Remove(data.gameObject);
        inActiveList.Enqueue(data);
        return true;
    }
}
