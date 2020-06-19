using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Obstacle, IActivator, IStateTracker
{

    #region Variables


    // Components
    private StateMachine states; // Optional
    protected ActivateByProximity activate;

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
    /// Run this object's Start() function. Called by children of this class.
    /// </summary>
    protected virtual void RunStart()
    {
        this.Start();
    }

    /// <summary>
    /// Called on startup to define this Enemy's components.
    /// </summary>
    protected void Start()
    {
        enabled = false;
        hasActivated = false;

        // Get this Enemy's Components.
        hitBox = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        activate = GetComponent<ActivateByProximity>();
        states = GetComponent<StateMachine>();
        activate.SetTarget(PlayerController.GetInstance().gameObject);
    }

    /// <summary>
    /// Called upon activation in Activate(). Meant to be overridden by children of this class.
    /// </summary>
    protected virtual void DoSomething()
    {
        BecomePhysical();
        states.Next();
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

    public void ReportFinishedState()
    {
        states.Next();
    }
}
