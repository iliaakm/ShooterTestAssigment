using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    Vector2 moveDirection;
    [SerializeField]
    float moveDistance;
    [SerializeField]
    CapsuleCollider capsuleCollider;
    [SerializeField]
    Enemy enemy;

    float enemyRadius;
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        moveSpeed = GameConfig.Agent.moveSpeed;
        enemyRadius = capsuleCollider.radius;
    }

    private void FixedUpdate()
    {
        if (enemy.AliveStatusState == AliveStatus.Dead)
            return;

        Move();
    }
    private void Move()
    {
        RaycastHit hit;
        Vector3 posDelta = Vector3.zero;

        Debug.DrawRay(transform.position, transform.TransformDirection(moveDirection), Color.yellow);

        if (Physics.Raycast(transform.position, transform.TransformDirection(moveDirection), out hit, enemyRadius + moveSpeed * Time.deltaTime))
        {
            ChangeDirection();
            Debug.DrawLine(transform.position, hit.point, Color.blue);
        }
        else
        {
            posDelta = moveSpeed * Time.deltaTime * moveDirection;
        }

        Vector3 newPos = transform.position + posDelta;
        if (Vector3.Distance(startPos, newPos) > moveDistance)
        {
            ChangeDirection();
            newPos = transform.position;
        }

        transform.position = newPos;
    }

    void ChangeDirection()
    {
        moveDirection = -moveDirection;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (Application.isPlaying)
            Gizmos.DrawRay(startPos, transform.TransformDirection(moveDirection) * moveDistance);
        else
            Gizmos.DrawRay(transform.position, transform.TransformDirection(moveDirection) * moveDistance);
    }
}
