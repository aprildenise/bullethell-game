using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enemy that begins shooting once it finishes following its curve.
/// </summary>
public class ShootingEnemy : Enemy
{
    /// <summary>
    /// Shooter that belongs to this enemy.
    /// </summary>
    private AutoShooter shooter;

    /// <summary>
    /// Set up this Enemy.
    /// </summary>
    private void Start()
    {
        RunStart();
        if (shooter == null)
        {
            shooter = GetComponent<AutoShooter>();
        }
        shooter.enabled = false;
    }

    /// <summary>
    /// Check if this enemy has finished following its curve.
    /// If it has, then turn on its shooter.
    /// </summary>
    private void Update()
    {
        if (followCurve.IsFinished())
        {
            shooter.enabled = true;
            //shooter.BeginShooting();
        }
    }

}
