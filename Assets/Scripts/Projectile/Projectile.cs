using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, ITypeSize, IWeaponSpawn
{

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

    [Header("Optional")]
    // Optional components to have.
    public bool allowInteraction;
    public Homing homing;


    // For computating speed, velocity, acceleration, and spawn.
    private Transform target;
    private Rigidbody rigidBody;
    [HideInInspector]
    public Vector3 currentVelocity;
    private float currentSpeed;
    private float acceleration;
    private float deceleration;
    private float timer = 0;
    [HideInInspector]
    public Weapon origin;

    /// <summary>
    /// Set up this projectile.
    /// </summary>
    public void Start()
    {
        rigidBody = this.gameObject.GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
        deceleration = -1 * (maxSpeed / timeToMin);
        Setup();
    }

    protected virtual void Setup()
    {
        return;
    }

    /// <summary>
    /// Find the acceleration and deceleration of the bullet.
    /// </summary>
    protected void Update()
    {

        if (deaccelerating)
        {
            if (currentSpeed == minSpeed && timer > timeInDecceleration)
            {
                deaccelerating = false;
                accelerating = true;
                return;
            }
            currentSpeed += deceleration * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, minSpeed);
            timer += Time.deltaTime;
        }
        else
        {
            if (accelerating)
            {
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }

            if (decelerating)
            {
                currentSpeed += deceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, minSpeed);
            }
        }
    }


    protected abstract void OnTrigger();

    /// <summary>
    /// Interact with what was just collided based on Typing and Size rules.
    /// If this is an exploding bullet, the exploding action takes priority.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {

        // Check if we're colliding with someting on the same sorting layer, or in the Environment layer
        // then don't interact with it.
        if (other.gameObject.layer == this.gameObject.layer 
            || other.gameObject.layer == LayerMask.NameToLayer("Environment")) return;

        OnTrigger();
        if (allowInteraction) Interact(other);
    }

    protected void Interact(Collider other)
    {

        // Attempt to interact with this by checking if its an appropriate object.
        IWeaponSpawn otherObject = other.gameObject.GetComponent<IWeaponSpawn>();
        if (otherObject != null)
        {
            // Make sure this bullet isn't interacting with a bullet from the same Shooter.
            if (GetOrigin().Equals(otherObject.GetOrigin())) return;
        }

        // Everything checks out!
        TypeSizeController.Interact(this.gameObject, other.gameObject);
    }


    public Rigidbody GetRigidbody()
    {
        return rigidBody;
    }


    public Type GetGameType()
    {
        return origin.GetGameType();
    }

    public Size GetSize()
    {
        return origin.GetSize();
    }

    public void SetType(Type type)
    {
        throw new System.NotImplementedException();
    }

    public void SetSize(Size size)
    {
        throw new System.NotImplementedException();
    }

    public void OnAdvantage(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET ADVANTAGE");
        if (other.GetComponent<Laser>() != null)
        {
            Destroy(gameObject);
        }
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET NEUTRAL:" + this.gameObject);

        if (other.GetComponent<Bullet>() == null) return;

        Vector3 colliderCenter = collider.GetComponent<Collider>().bounds.center;
        Vector3 otherCenter = other.GetComponent<Collider>().bounds.center;
        Vector3 colliderVelocity1 = collider.GetComponent<Rigidbody>().velocity;
        Vector3 otherVelocity1 = other.GetComponent<Rigidbody>().velocity;

        float colliderMass = 1f;
        float otherMass = 1f;

        // Calculate the elastic collision.
        Vector3 diffVelocity = colliderVelocity1 - otherVelocity1;
        Vector3 diffCenter = colliderCenter - otherCenter;
        float dividend = Vector3.Dot(diffVelocity, diffCenter);
        float divisor = Mathf.Pow(Vector3.Dot(diffCenter, diffCenter), 2);
        Vector3 colliderVelocity2 = colliderVelocity1
            - (2f * otherMass / (colliderMass + otherMass))
            * (dividend / divisor) * diffCenter;
        colliderVelocity2.y = 0f;

        // Apply the new vector.
        float multiplier = -.8f;


        currentVelocity = colliderVelocity2 * multiplier;
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET DISADVANTAGE:" + this.gameObject);

    }

    public Weapon GetOrigin()
    {
        return origin;
    }
}
