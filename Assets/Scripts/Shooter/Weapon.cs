using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, ITypeSize
{


    public string weaponName;
    public float damageMultiplier;
    protected Type weaponType;
    protected Size weaponSize;


    public abstract bool UseWeapon();

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
