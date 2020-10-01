using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shooter", menuName = "Shooter Info")]
public class ShooterInfo : ScriptableObject
{

    public string shooterName;
    public float damageMultiplier;
    public GameObject prefab;
    public float speed;
    [Range(0, 360)]
    public float aimDegree;
    public float shotDelay;
    public bool homing;
    [Header("Shoot in arrays and groups")]
    public bool equalArraySpread;
    public int arrays;
    [Range(0, 360)]
    public float arraySpread;
    public int arrayGroups;
    [Range(0, 360)]
    public float arrayGroupSpread;

    public Type shooterType;
    public Size shooterSize;

}
