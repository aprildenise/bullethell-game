using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLaser : Laser
{
    public override bool UseWeapon(bool useWeapon)
    {
        if (useWeapon) EnableLaserBeam();
        else DisableLaserBeam();

        canUseWeapon = useWeapon;
        return canUseWeapon;
    }


    private void Start()
    {
        base.RunStart();
    }

    private void FixedUpdate()
    {
        base.RunFixedUpdate();
    }
}
