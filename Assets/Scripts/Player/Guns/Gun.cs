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
    protected DamageGiver gunProjectilePref;
    [SerializeField]
    protected Transform gunShootPoint;

    [SerializeField]
    protected AudioSource gunSoundSource;
    [SerializeField]
    protected AudioClip gunShootSound;
    [SerializeField]
    protected AudioClip gunReloadSound;

    private int gunAmmoInClip;
    float timeBetweenShots, timeLastShot;
    bool reloading;

    private void Start()
    {
        timeBetweenShots = 1f / gunFireRate;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
    }

    public virtual void TryShoot()
    {
        if (CanShoot()) Shoot();
    }

    bool CanShoot()
    {
        if (Time.time - timeLastShot > timeBetweenShots)
        {
            timeLastShot = Time.time;
            return true;
        }
        return false;
    }

    public virtual void Shoot()
    {
        DamageGiver projectile = CreateProjectile();        
    }

    public virtual void Reload()
    {

    }
    public virtual DamageGiver CreateProjectile()
    {
        //TODO Add Object pooling
        GunProjectile projectile = Instantiate(gunProjectilePref.gameObject,
            gunShootPoint).GetComponent<GunProjectile>();   //TODO mess with parent object

        projectile.InitProjectile(gunDamage, gunProjectileSpeed, gunShootPoint.position);

        return projectile;
    }
}
