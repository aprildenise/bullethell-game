using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLaser : Laser
{

    public GameObject laserFragmentPrefab;

    public override bool UseWeapon(bool useWeapon)
    {
        if (useWeapon)
        {
            charge.Charge();
        }
        else
        {
            charge.Cancel();
        }

        return useWeapon;
    }


    private void Start()
    {
        base.RunStart();
        charge = GetComponent<ChargeUp>();
    }

    private void FixedUpdate()
    {
        base.RunFixedUpdate();
    }
}
