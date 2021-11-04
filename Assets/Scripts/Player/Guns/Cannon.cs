using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Gun
{
    Animator cannon_Animator;

    protected override void Start()
    {
        base.Start();
        cannon_Animator = GetComponent<Animator>();
    }

    protected override void Reload()
    {
        base.Reload();
        cannon_Animator.SetTrigger(GameConfig.Animation.cannonAnimatorReloading);  
    }
}
