using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ITypeSize
{
    #region Variables

    // From bulletInfo. fields are public for now

    private BulletInfo bulletInfo;
    [Header("Homing")]
    public float baseSpeed; // Speed as given by the Shooter this came from.
    public float homingRate;

    [Header("Acceleration/Decceleration")]
    public bool accelerating;
    public bool decelerating; 
    public float timeToMax;
    public float timeToMin;
    public float maxSpeed;
    public float minSpeed;

    [Header("Deccelerate, then accelerate")]
    public bool deaccelerating;
    public float timeInDecceleration;

    // For class computations
    private Transform target;
    private Rigidbody rigidBody;
    private float velocity;
    private float acceleration;
    private float deceleration;
    private float timer = 0;

    // Type and size of the weapon that shot this bullet.
    private Type shooterType;
    private Size shooterSize;
    private Vector3 origin;
    /// <summary>
    /// The Shooter that init this Bullet.
    /// </summary>
    private Shooter shooter;

    #endregion

    #region Bullet Operations

    public void SetBulletInfo(BulletInfo bulletInfo)
    {
        if (bulletInfo != null)
        {
            this.homingRate = bulletInfo.homingRate;
            this.accelerating = bulletInfo.accelerating;
            this.decelerating = bulletInfo.decelerating;
            this.timeToMax = bulletInfo.timeToMax;
            this.timeToMin = bulletInfo.timeToMin;
            this.maxSpeed = bulletInfo.maxSpeed;
            this.minSpeed = bulletInfo.minSpeed;
            this.deaccelerating = bulletInfo.deaccelerating;
            this.timeInDecceleration = bulletInfo.timeInDecceleration;
        }
    }

    public void SetShooter(Shooter shooter)
    {
        this.shooter = shooter;
        this.shooterType = shooter.shooterType;
        this.shooterSize = shooter.shooterSize;
        this.baseSpeed = shooter.speed;
    }

    public Shooter GetShooter()
    {
        return shooter;
    }

    /// <summary>
    /// Set up this bullet.
    /// </summary>
    protected void Start()
    {
        velocity = baseSpeed;
        rigidBody = GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
        deceleration = -1 * (maxSpeed / timeToMin);

        origin = this.transform.position;
    }

    /// <summary>
    /// Find the acceleration and deceleration of the bullet.
    /// </summary>
    protected void Update()
    {
        if (deaccelerating)
        {
            if (velocity == minSpeed && timer > timeInDecceleration)
            {
                deaccelerating = false;
                accelerating = true;
                return;
            }
            velocity += deceleration * Time.deltaTime;
            velocity = Mathf.Max(velocity, minSpeed);
            timer += Time.deltaTime;
        }
        else
        {
            if (accelerating)
            {
                velocity += acceleration * Time.deltaTime;
                velocity = Mathf.Min(velocity, maxSpeed);
            }

            if (decelerating)
            {
                velocity += deceleration * Time.deltaTime;
                velocity = Mathf.Max(velocity, minSpeed);
            }
        }
    }


    /// <summary>
    /// If homing, alter the path of this bullet.
    /// Also, apply the acceleration and deceleration found in Update.
    /// </summary>
    protected void FixedUpdate()
    {
        // If this bullet has a homingRate, then it will home and move towards the player.
        if (homingRate > 0)
        {
            // Take the cross product of the normalized vectors for the direction to the target,
            // and the direction the bullet is currently moving in, in order to find the rotation needed
            // to reach the target.
            Vector3 targetDirection = target.position - rigidBody.position;
            targetDirection.Normalize();
            Vector3 currentDirection = rigidBody.velocity;
            currentDirection.Normalize();
            Vector3 rotate = Vector3.Cross(targetDirection, currentDirection);

            // Apply the rotation.
            rigidBody.angularVelocity = -rotate * homingRate;
        }
        // Continue moving the bullet (if needed)
        rigidBody.velocity = transform.forward * velocity;
    }


    /// <summary>
    /// Interact with what was just collided based on Typing and Size rules.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {
        // Make sure bullets from the same weapon aren't colliding with each other
        try
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet.GetShooter().Equals(shooter))
            {
                return;
            }
        } catch (System.NullReferenceException)
        {
            // Interact with what was collided.
            TypeSizeController.Interact(this.gameObject, other.gameObject);
        }
        
    }

    #endregion

    #region TypeSize

    public Type GetGameType()
    {
        return this.shooterType;
    }

    public Size GetSize()
    {
        return this.shooterSize;
    }

    public void SetType(Type type)
    {
        this.shooterType = type;
    }

    public void SetSize(Size size)
    {
        this.shooterSize = size;
    }

    /// <summary>
    /// Damage the other GameObject, only if they are a Destructible.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public void OnAdvantage(GameObject collider, GameObject other)
    {
        try
        {
            // Inflict damage and destroy this Bullet.
            IDestructable destructable = other.gameObject.GetComponent<IDestructable>();
            float damage = CalculateDamage(other.gameObject);
            destructable.ReceiveDamage(damage);
            Destroy(this.gameObject);
        }
        catch (System.NullReferenceException)
        {
            // They are not a Destructible.
        }
    }

    /// <summary> 
    /// Calculate the damage this bullet will inflict onto a Destructable, based on
    /// the distance from the target and this bullet's shooter.
    /// </summary>
    /// <returns>Damage calculated.</returns>
    protected virtual float CalculateDamage(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position, origin);
        return distance * shooter.damageMultiplier;
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// For bullets, this is the same as OnAdvantage().
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public void OnNeutral(GameObject collider, GameObject other)
    {
        OnAdvantage(collider, other);
    }


    #endregion
}
