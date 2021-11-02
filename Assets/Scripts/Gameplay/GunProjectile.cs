using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : DamageGiver        //В настоящем шутере классы пули и нанесения урона наверняка были бы связаны по-другому, здесь для упрощения сделано про принцину KISS
{
    public float MoveSpeed { get; set; }
    public Vector3 MoveDirection { get; set; }

    private void Update()
    {
        transform.position += Time.deltaTime * MoveSpeed * MoveDirection;
    }

    public void InitProjectile(float damageAmount, float moveSpeed, Vector3? moveDirection = null)
    {
        this.DamageAmount = damageAmount;
        this.MoveSpeed = moveSpeed;
        moveDirection ??= Vector3.forward;      //default direction
        this.MoveDirection = moveDirection.Value;
    }
}
