using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public string prefabKey;

    private void OnValidate()
    {
        if (prefabKey == null || prefabKey == "")
        {
            prefabKey = gameObject.name;
        }
    }
}
