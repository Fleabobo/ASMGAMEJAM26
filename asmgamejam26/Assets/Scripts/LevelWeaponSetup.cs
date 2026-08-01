using UnityEngine;

public class LevelWeaponSetup : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    public WeaponData levelWeapon;

    void Start()
    {
        if (weaponHolder != null && levelWeapon != null)
        {
            weaponHolder.EquipWeapon(levelWeapon);
        }
    }
}