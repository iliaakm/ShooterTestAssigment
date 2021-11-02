using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField]
    protected float gunAccuracy;
    [SerializeField]
    protected float gunFireRate;
    [SerializeField]
    protected float gunDamage;
    [SerializeField]
    protected float gunProjectileSpeed;
    [SerializeField]
    protected float gunReloadTime;

    [SerializeField]
    protected int gunClipSize;

    [SerializeField]
    protected AudioSource gunSoundSource;
    [SerializeField]
    protected AudioClip gunShootSound;
    [SerializeField]
    protected AudioClip gunReloadSound;

    private int gunAmmoInClip;

    public virtual void Shoot()
    {

    }

    public virtual void Reload()
    {

    }
}
