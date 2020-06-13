using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeUpLaser : ChargeUp
{

    public Laser chargingObject;

    public float chargeBallSize;
    [SerializeField]
    private ParticleSystem chargeBall;
    [SerializeField]
    private ParticleSystem[] otherParticles;

    private void Start()
    {
        base.RunStart();
    }

    private void Update()
    {
        base.RunUpdate();
    }

    protected override void OnCharge()
    {
        ParticleSystem.MainModule main = chargeBall.main;
        main.duration = timeToCharge;
        main.startLifetime = timeToCharge;
        main.startSize = chargeBallSize;

        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 1f);
        ParticleSystem.SizeOverLifetimeModule mod = chargeBall.sizeOverLifetime;
        mod.enabled = true;
        mod.size = new ParticleSystem.MinMaxCurve(1f, curve);

        chargeBall.gameObject.SetActive(true);
        chargeBall.Play(true);
    }

    protected override void OnFinishedCharge()
    {
        ParticleSystem.SizeOverLifetimeModule mod = chargeBall.sizeOverLifetime;
        mod.enabled = false;
        ParticleSystem.MainModule main = chargeBall.main;
        otherParticles[0].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        chargeBall.Pause();

        chargingObject.EnableLaserBeam();

    }

    protected override void OnCancel()
    {
        chargingObject.DisableLaserBeam();

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
        int p = chargeBall.GetParticles(particles);

        chargeBall.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
        chargeBall.Play(false);
    }

    protected override void OnInactive()
    {
        chargingObject.DisableLaserBeam();
        chargeBall.Stop(true);
    }

    protected override float GetTime()
    {
        return chargeBall.time;
    }



}
