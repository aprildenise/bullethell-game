using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteShooter : AutoShooter
{



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
                return;
            }
            else if (shots >= shotsPerSet)
            {
                shots = 0; // Reset individual shot counter.
                //StopShooting();
                BeginShooting();
                sets++; // Count this new set.
            }
        }
        else
        {
            // Check if this shot the required number of shots.
            if (shots >= requiredShots)
            {
                return;
            }
            else
            {
                BeginShooting();
            }
        }
        base.RunFixedUpdate();
    }
}
