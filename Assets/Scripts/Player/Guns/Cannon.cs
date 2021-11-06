using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Gun
{
    [SerializeField]
    Animator cannon_Animator;

    protected override void Reload()
    {
        base.Reload();
        cannon_Animator.SetTrigger(GameConfig.Animation.cannonAnimatorReloading);  
    }
}
