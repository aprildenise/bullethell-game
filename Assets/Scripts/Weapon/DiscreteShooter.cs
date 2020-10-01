using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteShooter : AutoShooter
{

    [Header("Shooting requirements")]
    public int requiredShots;
    public int requiredSets;

    /// <summary>
    /// Checks if this shooter shot all its required sets/shots.
    /// </summary>
    private void FixedUpdate()
    {
        if (shootInSets)
        {
            // Check if this shot the required number of sets.
            if (sets >= requiredSets)
            {
                Destroy(this); // Done. Destroy this Shooter.
                return;
            }
            else if (shots >= shotsPerSet)
            {
                shots = 0; // Reset individual shot counter.
                //StopShooting();
                UseWeapon(true);
                sets++; // Count this new set.
            }
        }
        else
        {
            // Check if this shot the required number of shots.
            if (shots >= requiredShots)
            {
                Destroy(this); // Done. Destroy this Shooter.
                return;
            }
            else
            {
                UseWeapon(true);
            }
        }
        base.RunFixedUpdate();
    }
}
