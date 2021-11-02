using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    void TakeDamage(int damageAmount);
}

public class DamageReceiver : MonoBehaviour, IDamageable
{
    [SerializeField]
    int maxHealthPoints;
    int currentHealthPoints;

    public UnityEvent onDeath;

    private void Awake()
    {
        currentHealthPoints = maxHealthPoints;
    }

    void IDamageable.TakeDamage(int damageAmount)
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
