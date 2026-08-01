using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("References")]
    public Transform weaponSocket;
    public AudioSource weaponAudioSource;

    [Header("Default")]
    public WeaponData defaultWeapon;

    private GameObject currentModel;
    private Animator currentAnimator;
    private MuzzleFlash currentMuzzleFlash;
    private WeaponData currentWeaponData;
    private bool isAiming;
    private int currentAmmo;
    private bool isReloading;

    void Start()
    {
        if (defaultWeapon != null)
            EquipWeapon(defaultWeapon);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            TryManualReload();
        }

        isAiming = Mouse.current.rightButton.isPressed;
        UpdateAimBlend();
    }

    void UpdateAimBlend()
    {
        if (currentModel == null || currentWeaponData == null || isReloading) return;

        Vector3 targetPos = isAiming ? currentWeaponData.aimPositionOffset : currentWeaponData.modelPositionOffset;
        Vector3 targetRot = isAiming ? currentWeaponData.aimRotationOffset : currentWeaponData.modelRotationOffset;

        float t = Time.deltaTime * currentWeaponData.aimSpeed;

        currentModel.transform.localPosition = Vector3.Lerp(currentModel.transform.localPosition, targetPos, t);
        currentModel.transform.localRotation = Quaternion.Slerp(currentModel.transform.localRotation, Quaternion.Euler(targetRot), t);
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null) return;

        if (currentModel != null)
            Destroy(currentModel);

        currentModel = Instantiate(newWeapon.modelPrefab, weaponSocket);
        currentModel.transform.localPosition = newWeapon.modelPositionOffset;
        currentModel.transform.localRotation = Quaternion.Euler(newWeapon.modelRotationOffset);

        currentAnimator = currentModel.GetComponentInChildren<Animator>();
        if (currentAnimator == null)
        {
            Debug.LogWarning(newWeapon.weaponName + " has no Animator component on its prefab!");
        }
        else if (newWeapon.animatorOverride != null)
        {
            currentAnimator.runtimeAnimatorController = newWeapon.animatorOverride;
        }

        currentMuzzleFlash = currentModel.GetComponentInChildren<MuzzleFlash>();

        currentWeaponData = newWeapon;
        currentAmmo = newWeapon.maxAmmo;
        isReloading = false;
    }

    public void Attack()
    {
        if (currentWeaponData == null || isReloading) return;

        if (currentWeaponData.usesAmmo && currentAmmo <= 0)
        {
            StartCoroutine(ReloadRoutine());
            return;
        }

        if (currentAnimator != null)
            currentAnimator.SetTrigger("Attack");

        PlaySound(currentWeaponData.swingSound);

        if (currentMuzzleFlash != null)
            currentMuzzleFlash.Flash();

        if (currentWeaponData.usesAmmo)
        {
            currentAmmo--;
        }

        if (currentWeaponData.firesProjectile && currentWeaponData.projectilePrefab != null)
        {
            FireProjectile();
        }
        else
        {
            FireRaycast();
        }

        if (currentWeaponData.usesAmmo && currentAmmo <= 0)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    void FireRaycast()
    {
        RaycastHit hit;
        Vector3 origin = weaponSocket.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, currentWeaponData.range))
        {
            MonsterHealth monster = hit.collider.GetComponent<MonsterHealth>();
            if (monster != null)
            {
                monster.TakeDamage(currentWeaponData.damage);
                PlaySound(currentWeaponData.hitSound);
            }
        }
    }

    void FireProjectile()
    {
        Vector3 spawnPos = currentMuzzleFlash != null ? currentMuzzleFlash.transform.position : weaponSocket.position;
        Quaternion spawnRot = Quaternion.LookRotation(transform.forward);

        GameObject proj = Instantiate(currentWeaponData.projectilePrefab, spawnPos, spawnRot);

        Projectile projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Init(currentWeaponData.damage, currentWeaponData.projectileSpeed, currentWeaponData.projectileLifetime);
        }
        else
        {
            Debug.LogWarning(currentWeaponData.weaponName + "'s projectile prefab has no Projectile script!");
        }
    }

    void TryManualReload()
    {
        if (currentWeaponData == null || isReloading) return;

        if (currentWeaponData.usesAmmo && currentAmmo < currentWeaponData.maxAmmo)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        Vector3 startPos = currentModel.transform.localPosition;
        Vector3 downPos = currentWeaponData.reloadPositionOffset;

        PlaySound(currentWeaponData.reloadSound);

        float dropTime = 0.3f;
        float elapsed = 0f;
        while (elapsed < dropTime)
        {
            elapsed += Time.deltaTime;
            currentModel.transform.localPosition = Vector3.Lerp(startPos, downPos, elapsed / dropTime);
            yield return null;
        }
        currentModel.transform.localPosition = downPos;

        yield return new WaitForSeconds(currentWeaponData.reloadDuration);

        Vector3 upStart = currentModel.transform.localPosition;
        Vector3 upTarget = currentWeaponData.modelPositionOffset;
        elapsed = 0f;
        while (elapsed < dropTime)
        {
            elapsed += Time.deltaTime;
            currentModel.transform.localPosition = Vector3.Lerp(upStart, upTarget, elapsed / dropTime);
            yield return null;
        }
        currentModel.transform.localPosition = upTarget;

        currentAmmo = currentWeaponData.maxAmmo;
        isReloading = false;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && weaponAudioSource != null)
        {
            weaponAudioSource.PlayOneShot(clip);
        }
    }

    public WeaponData CurrentWeapon => currentWeaponData;
}