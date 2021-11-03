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

    float enemyRadius;

    private void Start()
    {
        enemyRadius = GetComponent<CapsuleCollider>().radius;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        RaycastHit hit;
        Vector3 posDelta = Vector3.zero;
        Debug.DrawRay(transform.position, transform.TransformDirection(moveDirection), Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(moveDirection), out hit, enemyRadius + movementSpeed * Time.deltaTime))
        {
            moveDirection = -moveDirection;
            Debug.DrawLine(transform.position, hit.point, Color.blue);
            print(hit.collider.name);
        }
        else
        {
            posDelta = movementSpeed * Time.deltaTime * moveDirection;
             
        }
       
        transform.position += posDelta;
    }
}
