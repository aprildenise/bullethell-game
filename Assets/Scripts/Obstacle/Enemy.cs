using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Obstacle, IActivator
{

    #region Variables

    // Components
    /// <summary>
    /// Controls this Enemy to follow a curve.
    /// </summary>
    [SerializeField]
    protected FollowCurve followCurve; // Enemy does not have to have a FollowCurve Component.
    protected ActivateByProximity activate;

    // For Enemy AI.
    protected bool hasActivated;

    #endregion 

    #region Activator
    /// <summary>
    /// Calls the DoSomething() function, iff this Enemy has not already activated.
    /// </summary>
    public void Activate()
    {
        if (HasActivated()) return;
        Debug.Log(this.gameObject.name + " activated.");
        hasActivated = true;
        DoSomething();
    }

    public bool HasActivated()
    {
        return hasActivated;
    }

    #endregion

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
    /// The object stays in place if there is no FollowCurve Component attached.
    /// </summary>
    protected void FollowCurve()
    {
        if (followCurve != null) followCurve.Follow(true);
    }

    #region Enemy Functions

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
        hasActivated = false;
        // Get this Enemy's Components.
        //followCurve = GetComponent<FollowCurve>();
        hitBox = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        activate = GetComponent<ActivateByProximity>();
        activate.SetTarget(PlayerController.GetPlayerController().gameObject);

        // If this has an ObstacleInfo, apply it's fields.
        if (obstacleInfo != null)
        {
            SetObstacleInfo(obstacleInfo);
        }
    }

    /// <summary>
    /// For testing only.
    /// </summary>
    private void OnValidate()
    {
        if (obstacleInfo!= null)
        {
            SetObstacleInfo(this.obstacleInfo);
        }
    }

    #endregion
}
