using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    void TakeDamage(float damageAmount);
}

public class DamageReceiver : MonoBehaviour, IDamageable
{
    [SerializeField]
    float maxHealthPoints;
    float currentHealthPoints;

    public UnityEvent onDeath { get; set; }

    private void Awake()
    {
        currentHealthPoints = maxHealthPoints;
    }

    void IDamageable.TakeDamage(float damageAmount)
    {
        currentHealthPoints -= damageAmount;

        if (currentHealthPoints <= 0)
        {
            if (onDeath != null)
            {
                onDeath.Invoke();
            }
            Destroy(gameObject);
        }
    }
}
