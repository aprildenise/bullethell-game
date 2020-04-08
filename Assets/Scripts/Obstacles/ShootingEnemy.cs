using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{

    public Shooter shooter;

    private void Start()
    {
        RunStart();
        if (shooter == null)
        {
            shooter = GetComponent<Shooter>();
        }
        shooter.enabled = false;
    }

    private void Update()
    {
        if (followCurve.IsFinished())
        {
            shooter.enabled = true;
        }
    }

}
