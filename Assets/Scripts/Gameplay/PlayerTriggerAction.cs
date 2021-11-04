using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class PlayerTriggerAction : MonoBehaviour
{
    [Inject(Id = GameConfig.ZenjectConfig.playerTransform)]
    readonly Transform playerTransform;

    public UnityEvent TriggerEnterAction;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == playerTransform)
        {
            if(TriggerEnterAction != null)
            {
                TriggerEnterAction.Invoke();
            }
        }
    }
}
