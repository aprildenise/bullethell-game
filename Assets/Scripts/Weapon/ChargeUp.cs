using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Template class for things that charge up before activating.
/// </summary>
public abstract class ChargeUp : MonoBehaviour
{
    public float timeToCharge;
    public float timeToCancel;

    protected static readonly string chargeState = "Charge";
    protected static readonly string cancelState = "Cancel";
    protected static readonly string inactivateState = "Inactive";
    protected static readonly string finishedChargeState = "FinishedCharge";
    public string currentState;


    protected void RunStart()
    {
        this.Start();
    }

    private void Start()
    {
        currentState = inactivateState;
    }

    protected void RunUpdate()
    {
        this.Update();
    }

    private void Update()
    {
        if (GetTime() >= timeToCharge && currentState.Equals(chargeState))
        {
            FinishedCharge();
            return;
        }

        if (GetTime() >= timeToCancel && currentState.Equals(cancelState))
        {
            Inactive();
            return;
        }
    }



    public void Charge()
    {
        if (currentState.Equals(chargeState) || currentState.Equals(cancelState)) return;

        currentState = chargeState;
        OnCharge();
    }

    public void FinishedCharge()
    {
        if (currentState.Equals(finishedChargeState)) return;
        currentState = finishedChargeState;
        OnFinishedCharge();
    }

    public void Cancel()
    {
        if (currentState.Equals(inactivateState) || currentState.Equals(cancelState)) return;
        currentState = cancelState;
        OnCancel();
    }

    public void Inactive()
    {
        if (currentState.Equals(inactivateState)) return;
        currentState = inactivateState;
        OnInactive();
    }


    protected abstract void OnCharge();
    protected abstract void OnFinishedCharge();
    protected abstract void OnCancel();
    protected abstract void OnInactive();
    protected abstract float GetTime();

}
