using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFragment : Projectile
{




    protected override void Setup()
    {
        allowInteraction = false;
    }

    

    protected override void OnTrigger()
    {
        throw new System.NotImplementedException();
    }
}
