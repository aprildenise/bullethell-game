using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, ITypeSize
{

    [Header("Weapon Properites")]
    public string weaponName;
    public float damageMultiplier;
    public Type weaponType;
    public Size weaponSize;
    protected bool canUseWeapon;


    //protected void SetWeaponInfo(WeaponInfo info)
    //{
    //    weaponName = info.weaponName;
    //    damageMultiplier = info.damageMultiplier;
    //    weaponType = info.weaponType;
    //    weaponSize = info.weaponSize;
    //}

    public Weapon GetWeapon()
    {
        return this;
    }

    public abstract bool UseWeapon(bool useWeapon);

    public Type GetGameType()
    {
        return weaponType;
    }

    public Size GetSize()
    {
        return weaponSize;
    }

    public abstract void OnAdvantage(GameObject collider, GameObject other);

    public abstract void OnDisadvantage(GameObject collider, GameObject other);

    public abstract void OnNeutral(GameObject collider, GameObject other);

    public void SetSize(Size size)
    {
        weaponSize = size;
    }

    public void SetType(Type type)
    {
        weaponType = type;
    }
}
