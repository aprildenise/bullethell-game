using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ManualSword : Sword
{

    public float cooldown;
    private Timer cooldownTimer;

    private void Start()
    {
        base.RunStart();
        //rotator = gameObject.transform.parent;
        cooldownTimer = gameObject.AddComponent<Timer>();
        cooldownTimer.SetTimer(cooldown);
        cooldownTimer.StartTimer();

    }

    public override bool UseWeapon(bool useWeapon)
    {
        if (cooldownTimer.GetStatus() != Timer.Status.FINISHED) return false;
        if (!canUseWeapon || !useWeapon || isSwinging) return false;

        EnableBlade(true);
        cooldownTimer.StartTimer();
        isSwinging = true;
        canUseWeapon = false;
        return true;
    }


    private void FixedUpdate()
    {
        base.RunFixedUpdate();
    }


    public override void OnAdvantage(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnDisadvantage(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
    }

    public override void OnNeutral(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
    }

}
