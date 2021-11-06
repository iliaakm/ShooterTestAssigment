using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : DamageGiver, IObjectPoolNotifier
{
    [SerializeField]
    Rigidbody projectileRigidbody;
    [SerializeField]
    ParticleSystem hitParticle;

    public float MoveSpeed { get; set; }
    public Vector3 MoveDirection { get; set; }

    float poolReturnDelay;

    private void Awake()
    {
        if (OnHit == null)
            OnHit = new UnityEngine.Events.UnityEvent();
        OnHit.AddListener(ShellStop);
    }

    private void FixedUpdate()
    {
        transform.position += Time.deltaTime * MoveSpeed * MoveDirection;
    }

    public void InitProjectile(float damageAmount, float moveSpeed, Vector3 startWorldPosition, Vector3 moveDirection)
    {
        this.DamageAmount = damageAmount;
        this.MoveSpeed = moveSpeed;
        this.transform.position = startWorldPosition;
        this.MoveDirection = moveDirection;
    }

    void ShellStop()
    {
        this.MoveSpeed = 0;
        projectileRigidbody.velocity = Vector3.zero;
        projectileRigidbody.angularVelocity = Vector3.zero;
        hitParticle.Play();
        StartCoroutine(PoolReturnDelayCor());
    }

    void IObjectPoolNotifier.OnEnqueuedToPool()
    {
        hitParticle.Stop();
    }

    void IObjectPoolNotifier.OnCreatedOrDequeuedFromPool(bool created)
    {
        poolReturnDelay = hitParticle.main.duration;
    }

    IEnumerator PoolReturnDelayCor()
    {
        
        yield return new WaitForSeconds(poolReturnDelay);
        gameObject.ReturnToPool();
    }
}
