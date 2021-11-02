using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageGiver : MonoBehaviour
{
    public float DamageAmount { get; set; }
    UnityEvent onHit;

    private void OnCollisionEnter(Collision collision)
    {
        HitCheck(collision.gameObject);
    }

    void HitCheck(GameObject hitSubject)
    {
        IDamageable damageReceiver = hitSubject.GetComponent<IDamageable>();
        {
            if (damageReceiver != null)
            {
                Hit(damageReceiver);
            }
        }
        
    }

    void Hit(IDamageable damageReceiver)
    {
        damageReceiver.TakeDamage(DamageAmount);
        if(onHit != null)
        {
            onHit.Invoke();
        }
    }
}
