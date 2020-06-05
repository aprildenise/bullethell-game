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
        //this.enabled = true;
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(shotDelay, Timer.Status.FINISHED);
    }

    /// <summary>
    /// Called in order to control the shooter.
    /// </summary>
    public override bool UseWeapon(bool useWeapon)
    {
        canUseWeapon = useWeapon;
        return useWeapon;
    }

    private void Update()
    {
        base.SetAim(PlayerController.GetInstance().transform.forward);
    }

    /// <summary>
    /// Used to shoot as long as isShooting is true and Timer is FINISHED.
    /// </summary>
    private void FixedUpdate()
    {
        if (canUseWeapon && timer.GetStatus() == Timer.Status.FINISHED)
        {
            Shoot();
            timer.StartTimer();
        }
    }

}
