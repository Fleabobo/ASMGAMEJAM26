using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("References")]
    public Transform weaponSocket;
    public Animator weaponAnimator;

    [Header("Default")]
    public WeaponData defaultWeapon;

    [Header("TEST ONLY - remove later")]
    public WeaponData testWeapon;

    private GameObject currentModel;
    private WeaponData currentWeaponData;
    private bool isAiming;

    void Start()
    {
        if (defaultWeapon != null)
            EquipWeapon(defaultWeapon);
    }

    void Update()
    {
        // TEMPORARY: press P to test-equip the weapon in testWeapon slot
        if (Keyboard.current.pKey.wasPressedThisFrame && testWeapon != null)
        {
            EquipWeapon(testWeapon);
        }

        // Check if right mouse button is held
        isAiming = Mouse.current.rightButton.isPressed;

        UpdateAimBlend();
    }

    void UpdateAimBlend()
    {
        if (currentModel == null || currentWeaponData == null) return;

        // Pick target offset based on whether we're aiming or not
        Vector3 targetPos = isAiming ? currentWeaponData.aimPositionOffset : currentWeaponData.modelPositionOffset;
        Vector3 targetRot = isAiming ? currentWeaponData.aimRotationOffset : currentWeaponData.modelRotationOffset;

        // Smoothly blend toward that target every frame
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

        if (newWeapon.animatorOverride != null)
            weaponAnimator.runtimeAnimatorController = newWeapon.animatorOverride;

        currentWeaponData = newWeapon;
    }

    public void Attack()
    {
        if (currentWeaponData == null) return;
        weaponAnimator.SetTrigger("Attack");
    }

    public WeaponData CurrentWeapon => currentWeaponData;
}