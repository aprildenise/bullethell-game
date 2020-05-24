using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    /// <summary>
    /// BulletInfo that alreadu holds the fields that this Bullet object should have.
    /// </summary>
    [SerializeField]
    private BulletInfo bulletInfo;

    // From bulletInfo. fields are public for now
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

    private Vector3 origin;

    protected void Start()
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

        // CHANGE THIS
        GameObject parent = transform.parent.gameObject;
        Shooter shooter = parent.GetComponent<Shooter>();


        velocity = shooter.speed;
        rigidBody = GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
        deceleration = -1 * (maxSpeed / timeToMin);

        origin = this.transform.position;
    }

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
    /// If this bullet collides into another object that has the IDestructable interface,
    /// damage the object by calling Damage().
    /// </summary>
    /// <param name="other">Collider of the object this encountered.</param>
    protected void OnTriggerEnter(Collider other)
    {
        try
        {
            IDestructable destructable = other.gameObject.GetComponent<IDestructable>();
            float damage = CalculateDamage(other.gameObject);
            destructable.Damage(damage);
        } catch(System.NullReferenceException e)
        {
            Debug.LogError(e);
            return;
        }
    }

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



}
