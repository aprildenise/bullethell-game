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
    /// AI of the Enemy, which will only run if the Enemy has been activated.
    /// </summary>
    private void Update()
    {
        //if (!hasActivated) return;

        ////if (followCurve != null)
        ////{
        ////    if (followCurve.IsFinished())
        ////    {
        ////        shooter.enabled = true;
        ////        //shooter.BeginShooting();
        ////    }
        ////}
        //else
        //{
        //    shooter.enabled = true;
        //}
    }

}
