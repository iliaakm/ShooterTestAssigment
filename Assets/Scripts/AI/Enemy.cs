using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

enum DirectPlayerVisibility
{
    Visible,
    Invisible
}

enum ShootRange
{
    InRange,
    TooFar
}

public class Enemy : MonoBehaviour
{
    [Inject(Id = GameConfig.ZenjectConfig.playerTransform)]
    readonly Transform playerTransform;
    [Inject]
    ImpactReceiver playerImpactReceiver;

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
    float shootImpact;
    [SerializeField, Range(0f, 1f)]
    float shootAccuracy;
    [SerializeField]
    float shotTracerTime;

    Vector3 startPos;
    float enemyRadius;
    float timeBetweenShots;
    LineRenderer shotTracer;
    DamageReceiver damageReceiver;

    DirectPlayerVisibility _directPlayerVisibilityState;
    DirectPlayerVisibility DirectPlayerVisibilityState
    {
        get { return _directPlayerVisibilityState; }
        set
        {
            _directPlayerVisibilityState = value;
            if (_directPlayerVisibilityState == DirectPlayerVisibility.Invisible)
            {
                gameObject.layer = GameConfig.Layers.layerOutline;       //Outline ON
            }
            if (_directPlayerVisibilityState == DirectPlayerVisibility.Visible)
            {
                gameObject.layer = GameConfig.Layers.layerDefault;       //Outline Off
            }
        }
    }

    ShootRange ShootRangeState { get; set; }

    private void Start()
    {
        DirectPlayerVisibilityState = DirectPlayerVisibility.Invisible;
        ShootRangeState = ShootRange.TooFar;

        enemyRadius = GetComponent<CapsuleCollider>().radius;
        shotTracer = GetComponent<LineRenderer>();
        startPos = transform.position;
        timeBetweenShots = 60f / shootFireRatePerMin;

        StartCoroutine(ShootLoopCor());
    }

    private void FixedUpdate()
    {
        Move();
        CheckVisible();
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
        if (hitChance < shootAccuracy)
        {
            if (DirectPlayerVisibilityState == DirectPlayerVisibility.Visible)
                if (ShootRangeState == ShootRange.InRange)
                {
                    Vector3 pushDirection = playerTransform.position - transform.position;
                    HitPlayer(pushDirection);
                    StartCoroutine(DrawShotTrailCor());
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
        playerImpactReceiver.AddImpact(direction, shootImpact);
        Debug.Log("Hit player", this);
    }

    private void OnDestroy()
    {
        StopCoroutine(ShootLoopCor());
    }

    void CheckVisible()
    {
        DirectPlayerVisibilityState = DirectPlayerVisibility.Invisible;
        ShootRangeState = ShootRange.TooFar;

        Ray ray = new Ray(this.transform.position, playerTransform.position - this.transform.position);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.transform == playerTransform)
            {
                DirectPlayerVisibilityState = DirectPlayerVisibility.Visible;
                if (raycastHit.distance < shootRange)
                {
                    ShootRangeState = ShootRange.InRange;
                }
            }
        }
    }

    IEnumerator DrawShotTrailCor()
    {
        shotTracer.SetPosition(0, transform.position);
        shotTracer.SetPosition(1, playerTransform.position);
        shotTracer.enabled = true;

        yield return new WaitForSeconds(shotTracerTime);

        shotTracer.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        Gizmos.color = Color.blue;
        if (Application.isPlaying)
            Gizmos.DrawRay(startPos, transform.TransformDirection(moveDirection) * moveDistance);
        else
            Gizmos.DrawRay(transform.position, transform.TransformDirection(moveDirection) * moveDistance);
    }

    void Death()
    {

    }
}
