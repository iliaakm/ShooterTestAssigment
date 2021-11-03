using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    [Inject(Id = "PlayerTransform")]
    readonly Transform playerTransform;

    [Header("Movement")]
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    Vector2 moveDirection;
    [SerializeField]
    float moveDistance;

    [Header("Shooting")]
    [SerializeField]
    float shootRange;
    [SerializeField]
    float shootFireRatePerMin;
    [SerializeField]
    float shootDamage;
    [SerializeField, Range(0f, 1f)]
    float shootAccuracy;

    Vector3 startPos;
    float enemyRadius;
    float timeBetweenShots;

    private void Start()
    {
        enemyRadius = GetComponent<CapsuleCollider>().radius;
        startPos = transform.position;
        timeBetweenShots = 60f / shootFireRatePerMin;
        StartCoroutine(ShootLoopCor());
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
            ChangeDirection();
            Debug.DrawLine(transform.position, hit.point, Color.blue);
        }
        else
        {
            posDelta = movementSpeed * Time.deltaTime * moveDirection;
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

    private void ShootToPlayer()
    {
        float hitChance = Random.Range(0f, 1f);
        if (hitChance > shootAccuracy)
        {
            Ray ray = new Ray(this.transform.position, playerTransform.position - this.transform.position);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray, out raycastHit, shootRange))
            {
                if(raycastHit.collider.transform == playerTransform)
                {
                    HitPlayer(ray.direction);
                }
            }
        }
    }

    IEnumerator ShootLoopCor()
    {
        while (true)
        {
            ShootToPlayer();

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    void HitPlayer(Vector3 direction)
    {
        playerTransform.GetComponent<ImpactReceiver>().AddImpact(direction, shootDamage);
    }

    private void OnDestroy()
    {
        StopCoroutine(ShootLoopCor());
    }
}
