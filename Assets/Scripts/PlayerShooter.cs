using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : Shooter
{


    public Player player;


    private void Start()
    {
        base.RunStart();
        base.SetAim(90f);
    }

    private void FixedUpdate()
    {
        //if (canShoot())
        //{
        //    Shoot(defaultAim);
        //}
    }

}
