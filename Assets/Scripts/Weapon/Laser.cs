using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract for Lasers. Lasers require a LineRenderer for graphics and will use raycasting to find 
/// collisions/triggers. 
/// </summary>
public abstract class Laser : Weapon
{

    [SerializeField]
    private WeaponInfo info;
    private LineRenderer laserBeam;
    //public float growSpeed;
    public float maxLength;
    public float minLength;
    public float maxWidth;
    public float minWidth;

    private float currentLength;
    private float currentWidth;
    //private bool hasReachedMaxLength;
    //private bool hasReachedMaxWidth;
    private int layerMask;

    protected void RunStart()
    {
        this.Start();
    }
    
    /// <summary>
    /// Setup this Laser by defining variables and components.
    /// </summary>
    private void Start()
    {
        //hasReachedMaxLength = false;
        //hasReachedMaxWidth = false;
        currentLength = minLength;
        currentWidth = minWidth;
        layerMask |= (1 << LayerMask.NameToLayer("Environment"));
        layerMask |= (1 << gameObject.layer);
        layerMask = ~(layerMask);
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
        //Vector3 direction = new Vector3(PlayerController.GetInstance().lookingAt.x, 0f, PlayerController.GetInstance().lookingAt.z);
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, maxWidth / 2, transform.right, maxLength, layerMask);
        if (hit.Length > 0)
        {
            OnRaycastHit(hit);
        }
        else
        {
            OnRaycastMiss();
        }

        //// Continue to grow the laser beam if we haven't hit anything or if we're still growing.
        //if (!hasReachedMaxLength)
        //{
        //    currentLength += Time.fixedDeltaTime * growSpeed;

        //    if (currentLength > maxLength)
        //    {
        //        currentLength = maxLength;
        //        hasReachedMaxLength = true;
        //    }
        //}
        //if (!hasReachedMaxWidth)
        //{
        //    currentWidth += Time.fixedDeltaTime * growSpeed;
        //    if (currentWidth > maxWidth)
        //    {
        //        currentWidth = maxWidth;
        //        hasReachedMaxWidth = true;
        //    }
        //}

        //UpdateBeamSize();
    }

    /// <summary>
    /// Controls what happens when the laser raycasts and hits something. 
    /// This will shorten the length of the LineRenderer so that it meets the
    /// closest object in the given hits.
    /// </summary>
    /// <param name="hit">List of hits</param>
    protected virtual void OnRaycastHit(RaycastHit[] hits)
    {
        // Find the closest hit.
        RaycastHit closest = hits[0];
        foreach (RaycastHit other in hits)
        {
            if (other.distance < closest.distance) closest = other;
        }
        // Set the position of the LineRenderer to meet the hit.
        currentLength = Mathf.Abs(hits[0].point.x - 2f); // 2f for offset.
        laserBeam.SetPosition(0, new Vector3(currentLength, 0f, 0f));
    }

    /// <summary>
    /// Controls what happens when the laser raycasts and misses.
    /// </summary>
    protected virtual void OnRaycastMiss()
    {
        currentLength = maxLength;
        laserBeam.SetPosition(0, new Vector3(currentLength, 0f, 0f));
    }
    
    //private void UpdateBeamSize()
    //{
    //    AnimationCurve curve = new AnimationCurve();
    //    curve.AddKey(0f, currentWidth);
    //    laserBeam.widthCurve = curve;
    //    laserBeam.SetPosition(0, new Vector3(currentLength, 0f, 0f));
    //}

    //private void ResetLaserBeam()
    //{
    //    laserBeam.enabled = false;
    //    hasReachedMaxLength = false;
    //    hasReachedMaxWidth = false;
    //    currentWidth = minWidth;
    //    currentLength = minLength;
    //}

    protected void EnableLaserBeam()
    {
        //UpdateBeamSize();
        laserBeam.enabled = true;
    }

    protected void DisableLaserBeam()
    {
        //ResetLaserBeam();
        laserBeam.enabled = false;
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
