using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : DamageGiver        //В настоящем шутере классы пули и нанесения урона были бы связаны по-другому, здесь для упрощения сделано про принцину KISS
{
    public float MoveSpeed { get; set; }
    public Vector3 MoveDirection { get; set; }

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
}
