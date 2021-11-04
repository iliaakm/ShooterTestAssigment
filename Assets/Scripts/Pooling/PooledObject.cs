using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public ObjectPool owner;
}

public static class PooledGameObjectExtentions
{
    public static void ReturnToPool(this GameObject gameObject)
    {
        var pooledObject = gameObject.GetComponent<PooledObject>();

        if(pooledObject == null)
        {
            Debug.LogError($"Cannot return {gameObject} to object pool, because it was not created from one");
            return;
        }

        pooledObject.owner.ReturnObject(gameObject);
    }
}
