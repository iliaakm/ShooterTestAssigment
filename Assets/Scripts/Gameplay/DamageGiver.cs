using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    [SerializeField]
    int damageAmount = 1;

    private void OnCollisionEnter(Collision collision)
    {
        var otherDamageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if(otherDamageReceiver != null)
        {
            otherDamageReceiver.TakeDamage(damageAmount);
        }
        Destroy(gameObject);
    }
}
