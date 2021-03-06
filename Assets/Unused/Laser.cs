﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract for Lasers. Lasers require a LineRenderer for graphics and will use raycasting to find 
/// collisions/triggers. 
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public abstract class Laser : Weapon
{
    
    [Header("Laser Properties")]
    public ChargeUp charge; // optional
    public float maxLength;
    public float minLength;
    public float maxWidth;
    public float minWidth;

    [Header("Collide With")]
    public LayerMask collideWith;

    [HideInInspector] public Vector3 origin;
    [HideInInspector] public Vector3 laserSize;

    private LineRenderer laserBeam;
    private float currentLength;
    private float currentWidth;
    private bool isReflecting = false;

    #region Laser Functions

    /// <summary>
    /// Run Start(). To be called by children of this class.
    /// </summary>
    protected void RunStart()
    {
        this.Start();
    }

    /// <summary>
    /// Setup this Laser by defining variables and components.
    /// </summary>
    private void Start()
    {
        currentLength = minLength;
        currentWidth = minWidth;
        laserBeam = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Run's the base FixedUpdate(). Used for children of the class.
    /// </summary>
    protected void RunFixedUpdate()
    {
        this.FixedUpdate();
    }

    /// <summary>
    /// Check the laser for collisions and make it grow/shrink accordingly.
    /// </summary>
    private void FixedUpdate()
    {
        if (!canUseWeapon) return;

        // Raycast to find what has hit this laser and stop the laser from growing.
        RaycastHit[] hit = Physics.SphereCastAll(transform.position,
            maxWidth / 2, transform.right, maxLength, collideWith);

        if (hit.Length > 0)
        {
            OnRaycastHit(hit);
        }
        else
        {
            OnRaycastMiss();
        }


        origin = transform.position;
        laserSize = laserBeam.GetPosition(0);

    }

    /// <summary>
    /// Controls what happens when the laser raycasts and hits something. 
    /// This will shorten the length of the LineRenderer so that it meets the
    /// closest object in the given hits.
    /// </summary>
    /// <param name="hit">List of hits</param>
    protected virtual void OnRaycastHit(RaycastHit[] hits)
    {
        // Change the points of the laser so that it only stretches until the hit.
        currentLength = Mathf.Abs(hits[0].point.x);
        Vector3 point = transform.InverseTransformPoint(hits[0].point);
        laserBeam.SetPosition(0, point);
        laserBeam.SetPosition(1, point);

        // Attempt to interact with these objects.
        RaycastHit firstHit = hits[0];
        GameObject other = firstHit.collider.gameObject;
        if (((1 << other.gameObject.layer) & collideWith) == 0) return;

        IWeaponSpawn spawn = other.gameObject.GetComponent<IWeaponSpawn>();
        if (spawn != null)
        {
            // Make sure this bullet isn't interacting with a bullet from the same Shooter.
            if (gameObject.Equals(spawn.GetOrigin())) return;
        }

        // Everything checks out!
        TypeSizeController.Interact(gameObject, other);


        // On default, attempt to create a laser reflection, if allowed to.
        if (!isReflecting)
        {
            laserBeam.SetPosition(2, Vector3.zero);
        }
        else
        {
            StartCoroutine(Reflect(firstHit));
        }
    }


    protected IEnumerator Reflect(RaycastHit firstHit)
    {
        //ParticleController.GetInstance().InstantiateParticle(ParticleController.ProjectileBounce, firstHit.transform.position);
        Vector3 pos = Vector3.Reflect(firstHit.point - this.transform.position, firstHit.normal);
        laserBeam.SetPosition(0, Vector3.zero);
        laserBeam.SetPosition(2, pos);
        yield return 0; 
    }

    /// <summary>
    /// Controls what happens when the laser raycasts and misses.
    /// </summary>
    protected virtual void OnRaycastMiss()
    {
        // Make the laser stretch to max length.
        currentLength = maxLength;
        laserBeam.SetPosition(0, new Vector3(currentLength, 0f, 0f));
        laserBeam.SetPosition(1, new Vector3(currentLength, 0f, 0f));
        laserBeam.SetPosition(2, Vector3.zero);
    }

    /// <summary>
    /// Turn on this Laser's laser beam.
    /// </summary>
    public void EnableLaserBeam()
    {
        canUseWeapon = true;
        laserBeam.enabled = true;
    }

    /// <summary>
    /// Turn off this Laser's laser beam.
    /// </summary>
    public void DisableLaserBeam()
    {
        laserBeam.enabled = false;
        canUseWeapon = false;
    }

    #endregion

    #region TypeSize

    public override void OnAdvantage(GameObject collider, GameObject other)
    {

        Debug.Log("LASER ADVANTAGE");

        isReflecting = false;

        // If this is a weapon spawn, destroy it.
        if (other.GetComponent<IWeaponSpawn>() != null) Destroy(other);

        // If this is a destructible, attempt to inflict damage.
        IDestructable destructable = other.GetComponent<IDestructable>();
        if (destructable != null)
        {
            destructable.ReceiveDamage(DamageCalculator.CalculateByDistance(collider.transform.position,
                other.transform.position, damageMultiplier));
            ParticleController.GetInstance().InstantiateParticle(ParticleController.PlayerLaserCollision, other.transform.position, transform.position);
            return;
        }

        // Finally, jam the laser if applicable.
        if (charge != null)
        {
            charge.Jam();
            Debug.Log("LASER JAMMED");
        }

    }

    public override void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("LASER DISADVANTAGE");

        isReflecting = false;

        // Jam the laser.
        if (charge != null)
        {
            charge.Jam();
            Debug.Log("LASER JAMMED");
        }
    }

    public override void OnNeutral(GameObject collider, GameObject other)
    {
        Debug.Log("LASER NEUTRAL");

        isReflecting = true;

    }


    #endregion

}
