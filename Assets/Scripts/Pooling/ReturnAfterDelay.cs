using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnAfterDelay : MonoBehaviour, IObjectPoolNotifier
{
    public void OnCreatedOrDequeuedFromPool(bool created)
    {
        print("Dequeued from object pool!");
        StartCoroutine(DoReturnAfterDelay());
    }

    public void OnEnqueuedToPool()
    {
        print("Enqueued to object pool!");
    }

    IEnumerator DoReturnAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        gameObject.ReturnToPool();
    }
}
