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

    public Vector3 origin;
    public Vector3 laserSize;

    private float currentLength;
    private float currentWidth;
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

        SetWeaponInfo(info);

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
        //laserBeam.SetPosition(0, PlayerController.GetInstance().transform.position);
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
        currentLength = Mathf.Abs(hits[0].point.x);
        Vector3 point = transform.InverseTransformPoint(hits[0].point);
        laserBeam.SetPosition(0, point);

        // Attempt to interact with these objects.
        // TODO decide if we should interact with all objects.
        GameObject other = hits[0].collider.gameObject;
        if (other.layer == this.gameObject.layer || other.layer == LayerMask.NameToLayer("Environment")) return;


        IWeaponSpawn spawn = other.gameObject.GetComponent<IWeaponSpawn>();
        if (spawn != null)
        {
            // Make sure this bullet isn't interacting with a bullet from the same Shooter.
            if (gameObject.Equals(spawn.GetOrigin())) return;
        }

        // Everything checks out!
        TypeSizeController.Interact(gameObject, other);

    }

    /// <summary>
    /// Controls what happens when the laser raycasts and misses.
    /// </summary>
    protected virtual void OnRaycastMiss()
    {
        currentLength = maxLength;
        laserBeam.SetPosition(0, new Vector3(currentLength, 0f, 0f));
    }

    //protected virtual void OnEnable()
    //{
    //    EnableLaserBeam();
        
    //}

    //protected virtual void OnDisable()
    //{
    //    DisableLaserBeam();
        
    //}

    public void EnableLaserBeam()
    {
        canUseWeapon = true;
        laserBeam.enabled = true;
    }

    public void DisableLaserBeam()
    {
        laserBeam.enabled = false;
        canUseWeapon = false;
    }

    #region TypeSize

    public override void OnAdvantage(GameObject collider, GameObject other)
    {

        Debug.Log("LASER ADVANTAGE");
        // If this is a weapon spawn, destroy it.
        if (other.GetComponent<IWeaponSpawn>() != null) Destroy(other);

    }

    public override void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("LASER DISADVANTAGE");
    }

    public override void OnNeutral(GameObject collider, GameObject other)
    {
        Debug.Log("LASER NEUTRAL");
    }

    #endregion

    private void OnValidate()
    {
        if (info != null) SetWeaponInfo(info);
    }
}
