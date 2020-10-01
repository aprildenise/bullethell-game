using UnityEngine;

public class ContinuousShooter : AutoShooter
{

    private void FixedUpdate()
    {
        // Shoot in a set
        if (shootInSets)
        {
            if (shots >= shotsPerSet)
            {
                shots = 0; // Reset individual shot counter.
                UseWeapon(true);
                sets++; // Count this new round.
            }
        }
        else
        {
            // Just shoot!
            UseWeapon(true);
        }
        base.RunFixedUpdate();
    }

}
