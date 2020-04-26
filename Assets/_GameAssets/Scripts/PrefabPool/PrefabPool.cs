using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPool : MonoBehaviour
{
    [Range(0, 250)]
    public int maxItems = 0;

    private Dictionary<string, LinkedList<PooledObject>> objectPools = new Dictionary<string, LinkedList<PooledObject>>();

    private const string DefaultKey = "DefaultKeyForNotAssignedPooledPrefabs";

    public void Recycle(PooledObject pooledObject)
    {
        if (pooledObject == null)
        {
            return;
        }
        var key = pooledObject.prefabKey ?? DefaultKey;
        LinkedList<PooledObject> pool = null;
        if (objectPools.ContainsKey(key))
        {
            pool = objectPools[key];
        } else
        {
            pool = new LinkedList<PooledObject>();
            objectPools.Add(key, pool);
        }
        if (maxItems > 0 && pool.Count >= maxItems)
        {
            Destroy(pooledObject.gameObject);
        } else
        {
            pooledObject.gameObject.SetActive(false);
            pool.AddLast(pooledObject);
        }
    }

    public PooledObject Dequeue(PooledObject poolablePrefab)
    {
        return Dequeue(poolablePrefab.prefabKey, () => { return Instantiate(poolablePrefab); });
    }

    public PooledObject Dequeue(GameObject prefab)
    {
        var poolablePrefab = prefab.GetComponent<PooledObject>();
        if (poolablePrefab != null)
        {
            return Dequeue(poolablePrefab);
        } else
        {
            return Dequeue(prefab.name, () =>
            {
                var clone = Instantiate(prefab);
                var poolable = clone.AddComponent<PooledObject>();
                poolable.prefabKey = prefab.name;
                return poolable;
            });
        }
    }

    private delegate PooledObject PoolableMaker();

    private PooledObject Dequeue(string srcKey, PoolableMaker poolableMaker)
    {
        var key = srcKey ?? DefaultKey;
        PooledObject result = null;
        if (objectPools.ContainsKey(key))
        {
            var pool = objectPools[key];
            while (pool.Count > 0 && result == null)
            {
                result = pool.Last.Value;
                pool.RemoveLast();
            }
        }
        if (result == null)
        {
            result = poolableMaker();
        }
        result.gameObject.SetActive(true);
        return result;
    }
}
