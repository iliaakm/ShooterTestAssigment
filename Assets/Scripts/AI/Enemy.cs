using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    Vector2 moveDirection;

    [Header("Shooting")]
    [SerializeField]
    float shootRange;
    [SerializeField]
    float shootFirePerMin;
    [SerializeField]
    float shootDamage;
    [SerializeField]
    float shootAccuracy;

    RaycastHit RayCastDir(Vector3 direction, float maxLength)
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, direction, out hit, maxLength);
        return hit;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Mathf.Repeat()
    }

    private void Move()
    {
        if(RayCastDir(moveDirection))
    }
}
