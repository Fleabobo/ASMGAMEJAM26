using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType type;
    public GameObject modelPrefab;
    public AnimatorOverrideController animatorOverride;
    public float damage;
    public float attackSpeed;
    public float range = 1.5f; // only used for melee raycast

    [Header("Hip Position (normal carry position)")]
    public Vector3 modelPositionOffset;
    public Vector3 modelRotationOffset;

    [Header("Aim Position (while holding right click)")]
    public Vector3 aimPositionOffset;
    public Vector3 aimRotationOffset;
    public float aimSpeed = 8f;

    [Header("Sounds")]
    public AudioClip swingSound;
    public AudioClip hitSound;

    [Header("Ammo (only used if Uses Ammo is checked)")]
    public bool usesAmmo = false;
    public int maxAmmo = 6;
    public float reloadDuration = 2f;
    public AudioClip reloadSound;
    public Vector3 reloadPositionOffset;

    [Header("Projectile (for firearms that shoot physical bullets)")]
    public bool firesProjectile = false;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public float projectileLifetime = 5f;
}

public enum WeaponType
{
    Melee,
    Pistol,
    Rifle,
    Shank
}