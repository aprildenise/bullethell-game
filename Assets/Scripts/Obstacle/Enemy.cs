using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Obstacle, IActivator
{

    /// <summary>
    /// Controls this Enemy to follow a curve.
    /// </summary>
    protected FollowCurve followCurve;


    /// <summary>
    /// Calls the DoSomething() function.
    /// </summary>
    public void Activate()
    {
        Debug.Log(this.gameObject.name + " activated.");
        DoSomething();
    }


    /// <summary>
    /// Called upon activation in Activate(). Meant to be overridden by children of this class.
    /// </summary>
    protected virtual void DoSomething()
    {
        BecomePhysical();
        FollowCurve();
    }

    /// <summary>
    /// Allows this object to become "physical" which just means that it becomes active, collidable
    /// and rendered.
    /// </summary>
    protected void BecomePhysical()
    {
        enabled = true;
        hitBox.enabled = true;
        mesh.enabled = true;
    }

    /// <summary>
    /// Allow this object to begin following a curve by using its FollowCurve Follow().
    /// </summary>
    protected void FollowCurve()
    {
        followCurve.Follow(true);
    }

    /// <summary>
    /// Run this object's Start() function. Called by children of this class.
    /// </summary>
    protected override void RunStart()
    {
        this.Start();
    }

    /// <summary>
    /// Called on startup to define this Enemy's components.
    /// </summary>
    private void Start()
    {
        enabled = false;
        if (followCurve == false)
        {
            followCurve = GetComponent<FollowCurve>();
        }
        hitBox = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
    }

}
