using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Auto Shooter", menuName = "AutoShooter Info")]
public class AutoShooterInfo : ScriptableObject
{

    public ShooterInfo shooterInfo;

    [Header("Main Enemy Shooter")]
    public float startDelay;

    [Header("Shoot in sets")]
    public bool shootInSets;
    public int shotsPerSet;
    public float setDelay;

    public int requiredShots;
    public int requiredSets;

    [Header("Rotation")]
    public bool constantRotation;
    public float spinAngle;

    public int sets;
}
