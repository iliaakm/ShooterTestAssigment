using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageGiver : MonoBehaviour
{
    [SerializeField]
    int damageAmount;
    UnityEvent onHit;

    private void OnCollisionEnter(Collision collision)
    {
        HitCheck(collision.gameObject);
    }

    void HitCheck(GameObject hitSubject)
    {
        DamageReceiver damageReceiver = hitSubject.GetComponent<DamageReceiver>();
        {
            if (damageReceiver != null)
            {
                Hit(damageReceiver);
            }
        }
        
    }

    void Hit(DamageReceiver damageReceiver)
    {
        damageReceiver.TakeDamage(damageAmount);
        if(onHit != null)
        {
            onHit.Invoke();
        }
    }
}
