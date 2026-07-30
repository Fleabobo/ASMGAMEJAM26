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

    [Header("Hip Position (normal carry position)")]
    public Vector3 modelPositionOffset;
    public Vector3 modelRotationOffset;

    [Header("Aim Position (while holding right click)")]
    public Vector3 aimPositionOffset;
    public Vector3 aimRotationOffset;
    public float aimSpeed = 8f; // how fast it blends into aim, higher = snappier
}

public enum WeaponType
{
    Melee,
    Pistol,
    Rifle,
    Shank
}