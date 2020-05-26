using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ITypeSize
{
    #region Variables

    // From bulletInfo. fields are public for now

    private BulletInfo b;
    [Header("Homing")]
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
    private Type type;
    private Size size;

    private Vector3 origin;
    private string shooterName;

    #endregion

    #region Bullet Operations

    public void SetBullet(BulletInfo bulletInfo, Type type, Size size, string shooterName)
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

        SetBullet(type, size, shooterName);
    }

    public void SetBullet(Type type, Size size, string shooterName)
    {
        this.type = type;
        this.size = size;
        this.shooterName = shooterName;
    }

    /// <summary>
    /// Set up this bullet.
    /// </summary>
    protected void Start()
    {

        // CHANGE THIS
        GameObject parent = transform.parent.gameObject;
        Shooter shooter = parent.GetComponent<Shooter>();


        velocity = shooter.speed;
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
            Bullet b = other.gameObject.GetComponent<Bullet>();
            if (b.shooterName == this.shooterName)
            {
                return;
            }
        } catch (System.NullReferenceException)
        {
            TypeSizeController.Interact(this.gameObject, other.gameObject);
        }
        
    }

    #endregion

    /// <summary>
    /// Calculate the damage this bullet will inflict onto a Destructable, based on
    /// the distance from the target and this bullet's shooter.
    /// </summary>
    /// <returns>Damage calculated</returns>
    protected virtual float CalculateDamage(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position, origin);
        return distance;
    }

    #region TypeSize

    public Type GetGameType()
    {
        return this.type;
    }

    public Size GetSize()
    {
        return this.size;
    }

    public void SetType(Type type)
    {
        this.type = type;
    }

    public void SetSize(Size size)
    {
        this.size = size;
    }

    /// <summary>
    /// Damage the other gameObject if they are a Destructible.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public void OnAdvantage(GameObject collider, GameObject other)
    {
        try
        {
            IDestructable destructable = other.gameObject.GetComponent<IDestructable>();
            float damage = CalculateDamage(other.gameObject);
            destructable.ReceiveDamage(damage);
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError(e);
            return;
        }
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
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
