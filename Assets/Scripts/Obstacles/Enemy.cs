using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Obstacle, IActivator
{

    public FollowCurve followCurve;


    public void Activate()
    {
        BecomePhysical();
        FollowCurve();
    }

    protected void BecomePhysical()
    {
        enabled = true;
        hitBox.enabled = true;
        mesh.enabled = true;
    }

    protected void FollowCurve()
    {
        followCurve.Follow(true);
    }

    protected override void RunStart()
    {
        this.Start();
    }

    // Start is called before the first frame update
    private void Start()
    {
        enabled = false;
        if (followCurve == false)
        {
            gameObject.GetComponent<FollowCurve>();
        }
    }

}
