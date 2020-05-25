using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualShooter : Shooter
{

    /// <summary>
    /// Timer to calculate shotDelay.
    /// </summary>
    Timer timer;

    /// <summary>
    /// Setups: set the aim, enable this component, set the timer.
    /// </summary>
    private void Start()
    {
        base.RunStart();
        base.SetAim(90f);
        this.enabled = true;
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(shotDelay, Timer.Status.FINISHED);
    }

    /// <summary>
    /// Called in order to control the shooter.
    /// </summary>
    /// <param name="allow">True to allow shoowing and make the shooter shoot, false elsewise. </param>
    public void AllowShooting(bool allow)
    {
        isShooting = allow;
    }

    /// <summary>
    /// Used to shoot as long as isShooting is true and Timer is FINISHED.
    /// </summary>
    private void FixedUpdate()
    {
        //Debug.Log("is shooting:" + isShooting);
        //Debug.Log("timer:" + timer.GetStatus());
        if (isShooting && timer.GetStatus() == Timer.Status.FINISHED)
        {
            Shoot();
            timer.StartTimer();
        }
    }

}
