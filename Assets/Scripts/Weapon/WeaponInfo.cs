using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponInfo : ScriptableObject
{

    public string weaponName;
    public float damageMultiplier;
    public Type weaponType;
    public Size weaponSize;

}
