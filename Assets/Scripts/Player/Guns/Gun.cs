using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class Gun : MonoBehaviour
{
    enum GunState
    {
        Loaded,
        Reloading
    }

    [Inject]
    ObjectPool pool;

    [SerializeField]
    protected float gunSpread;
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
        if (Input.GetButton("Fire1"))       //TODO take magic string from confiig 
        {
            TryShoot();
        }
    }

    protected virtual void FieldCheck()
    {
        if (gunSpread == 0) Debug.LogWarning("Accuracy not setted");
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
        CreateProjectile();

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
        GunProjectile projectile = pool.GetObject()
            .GetComponent<GunProjectile>();

        Vector3 moveDirection = gunShootPoint.forward + GetSpread();
        projectile.InitProjectile(gunDamage, gunProjectileSpeed, gunShootPoint.position, moveDirection);

        return projectile;
    }

    protected virtual Vector3 GetSpread()
    {
        Vector3 spread = Vector3.zero;
        spread.x = Random.Range(-gunSpread, gunSpread);
        spread.y = Random.Range(-gunSpread, gunSpread);

        return spread;
    }
}
