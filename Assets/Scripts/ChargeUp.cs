using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUp : MonoBehaviour
{


    public Laser chargingObject;
    public float timeToCharge;
    public float timeToCancel;
    public float size;

    [SerializeField]
    private ParticleSystem chargeBall;

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

    private void Update()
    {
        //Debug.Log(chargeBall.time);
        if (chargeBall.time >= timeToCharge && currentState.Equals(chargeState))
        {
            FinishedCharge();
            return;
        }

        if (chargeBall.time >= timeToCancel && currentState.Equals(cancelState))
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





    protected virtual void OnCharge()
    {
        ParticleSystem.MainModule main = chargeBall.main;
        main.duration = timeToCharge;
        main.startLifetime = timeToCharge;
        main.startSize = size;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 1f);
        ParticleSystem.SizeOverLifetimeModule mod = chargeBall.sizeOverLifetime;
        mod.enabled = true;
        mod.size = new ParticleSystem.MinMaxCurve(1f, curve);

        chargeBall.gameObject.SetActive(true);
        chargeBall.Play();
    }

    protected virtual void OnFinishedCharge()
    {
        ParticleSystem.SizeOverLifetimeModule mod = chargeBall.sizeOverLifetime;
        mod.enabled = false;
        ParticleSystem.MainModule main = chargeBall.main;
        chargeBall.Pause();

        chargingObject.EnableLaserBeam();

    }

    protected virtual void OnCancel()
    {
        chargingObject.DisableLaserBeam();

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
        int p = chargeBall.GetParticles(particles);

        chargeBall.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        Debug.Log(chargeBall.isPlaying);

        ParticleSystem.MainModule main = chargeBall.main;
        main.duration = timeToCancel;
        main.startLifetime = timeToCancel;
        main.startSize = particles[0].GetCurrentSize(chargeBall);

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);
        curve.AddKey(1f, 0f);
        ParticleSystem.SizeOverLifetimeModule mod = chargeBall.sizeOverLifetime;
        mod.enabled = true;
        mod.size = new ParticleSystem.MinMaxCurve(1f, curve);
        chargeBall.Play();
    }

    protected virtual void OnInactive()
    {
        chargingObject.DisableLaserBeam();
        chargeBall.Stop();
    }





}
