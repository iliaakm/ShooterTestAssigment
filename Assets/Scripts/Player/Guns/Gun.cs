using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    enum GunState
    {
        Loaded,
        Reloading
    }

    [SerializeField]
    protected float gunAccuracy;
    [SerializeField]
    protected float gunFireRatePerMin;
    [SerializeField]
    protected float gunDamage;
    [SerializeField]
    protected float gunProjectileSpeed;
    [SerializeField]
    protected float gunReloadTime;

    [SerializeField]
    protected int gunAmmoClipSize;

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
    [SerializeField]
    protected AudioClip gunEmptySound;

    GunState gunState { get; set; }
    Coroutine reloadCor;    

    private int gunAmmoInClip;
    private float timeBetweenShots = 0f;
    private float nextFire = 0f;

    private void Start()
    {
        timeBetweenShots = 60f / gunFireRatePerMin;
        gunAmmoInClip = gunAmmoClipSize;
        FieldCheck();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            TryShoot();
        }
    }

    protected virtual void FieldCheck()
    {
        if (gunAccuracy == 0) Debug.LogWarning("Accuracy not setted");
        if (gunFireRatePerMin == 0) Debug.LogWarning("FireRatePerMin not setted");
        if (gunDamage == 0) Debug.LogWarning("Damage not setted");
        if (gunProjectileSpeed == 0) Debug.LogWarning("ProjectileSpeed not setted");
        if (gunReloadTime == 0) Debug.LogWarning("ReloadTime not setted");
        if (gunAmmoClipSize == 0) Debug.LogWarning("ClipSize not setted");
        if (gunProjectilePref == null) Debug.LogWarning("ProjectilePref not setted");
        if (gunShootPoint == null) Debug.LogWarning("ShootPoint not setted");
        if (gunSoundSource == null) Debug.LogWarning("SoundSource not setted");
        if (gunShootSound == null) Debug.LogWarning("ShootSound not setted");
        if (gunReloadSound == null) Debug.LogWarning("ReloadSound not setted");
    }

    protected virtual void TryShoot()
    {
        if (CanShoot()) Shoot();
    }

    protected virtual bool CanShoot()
    {
        if (gunState ==  GunState.Reloading)
        {
            //PlayEmptySound();
            return false;
        }

        if (!CheckAmmo())
        {
            return false;
        }

        if (Time.time > nextFire)
        {
            nextFire = Time.time + timeBetweenShots;
            return true;
        }
        return false;
    }

    protected virtual void Shoot()
    {
        DamageGiver projectile = CreateProjectile();
        PlayShotSound();

        gunAmmoInClip--;
        CheckAmmo();
    }

    protected virtual void Reload()
    {
        if (reloadCor != null)
            return;
        reloadCor = StartCoroutine(ReloadCor());
    }

    protected  virtual IEnumerator ReloadCor()
    {
        PlayReloadSound();
        gunState = GunState.Reloading;
        yield return new WaitForSeconds(gunReloadTime);
        gunState = GunState.Loaded;
        gunAmmoInClip = gunAmmoClipSize;
        reloadCor = null;
    }

    protected virtual bool CheckAmmo()
    {
        if (gunAmmoInClip == 0)
        {
            Reload();
            return false;
        }
        return true;
    }

    protected virtual void PlayShotSound()
    {
        PlaySound(gunShootSound);
    }

    protected virtual void PlayEmptySound()
    {
        PlaySound(gunEmptySound);
    }

    protected virtual void PlayReloadSound()
    {
        PlaySound(gunReloadSound);
    }

    protected void PlaySound(AudioClip soundClip)
    {
        if (gunSoundSource && soundClip)
        {
            gunSoundSource.clip = soundClip;
            gunSoundSource.Play();
        }
    }

    protected virtual DamageGiver CreateProjectile()
    {
        //TODO Add Object pooling
        GunProjectile projectile = Instantiate(gunProjectilePref.gameObject, null,
            true).GetComponent<GunProjectile>();   //TODO mess with parent object

        projectile.InitProjectile(gunDamage, gunProjectileSpeed, gunShootPoint.position, gunShootPoint.forward);

        return projectile;
    }
}
