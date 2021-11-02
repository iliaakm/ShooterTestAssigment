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
        FieldCheck();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
    }

    public virtual void FieldCheck()
    {
        if (gunAccuracy == 0) Debug.LogWarning("Accuracy not setted");
        if (gunFireRate == 0) Debug.LogWarning("FireRate not setted");
        if (gunDamage == 0) Debug.LogWarning("Damage not setted");
        if (gunProjectileSpeed == 0) Debug.LogWarning("ProjectileSpeed not setted");
        if (gunReloadTime == 0) Debug.LogWarning("ReloadTime not setted");
        if (gunClipSize == 0) Debug.LogWarning("ClipSize not setted");
        if (gunProjectilePref == null) Debug.LogWarning("ProjectilePref not setted");
        if (gunShootPoint == null) Debug.LogWarning("ShootPoint not setted");
        if (gunSoundSource == null) Debug.LogWarning("SoundSource not setted");
        if (gunShootSound == null) Debug.LogWarning("ShootSound not setted");
        if (gunReloadSound == null) Debug.LogWarning("ReloadSound not setted");
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

    public virtual void PlayShotSound()
    {
        if (gunSoundSource && gunShootSound)
        {
            gunSoundSource.clip = gunShootSound;
            gunSoundSource.Play();
        }
    }

    public virtual DamageGiver CreateProjectile()
    {
        //TODO Add Object pooling
        GunProjectile projectile = Instantiate(gunProjectilePref.gameObject, null, 
            true).GetComponent<GunProjectile>();   //TODO mess with parent object

        projectile.InitProjectile(gunDamage, gunProjectileSpeed, gunShootPoint.position);

        return projectile;
    }
}
