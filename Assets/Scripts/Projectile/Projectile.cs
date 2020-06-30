using UnityEngine;

public abstract class Projectile : MonoBehaviour, ITypeSize, IWeaponSpawn
{

    [Header("Collide With Layer Masks")]
    public LayerMask collideWith;

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

    // For computating speed, velocity, acceleration, and spawn.
    protected Rigidbody rigidBody;
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
        allowInteraction = true;
        OnStart();
    }

    protected virtual void OnStart()
    {
        return;
    }

    /// <summary>
    /// Find the acceleration and deceleration of the bullet.
    /// </summary>
    protected void FixedUpdate()
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

        // Continue moving the bullet in its current trajectory.
        rigidBody.velocity = transform.forward * currentVelocity.magnitude;
        //rigidBody.velocity = currentVelocity;

        OnFixedUpdate();

    }

    protected virtual void OnFixedUpdate()
    {
        return;
    }

    protected virtual void OnTrigger()
    {
        return;
    }

    /// <summary>
    /// Interact with what was just collided based on Typing and Size rules.
    /// If this is an exploding bullet, the exploding action takes priority.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
    {

        Debug.Log("On trigger collision detected");

        // Check if we're colliding with someting we're allowed to collide with.
        if (((1 << other.gameObject.layer) & collideWith) == 0) return;

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

        // If this is a destructible, attempt to inflict damage.
        IDestructable destructable = other.GetComponent<IDestructable>();
        if (destructable != null)
        {
            destructable.ReceiveDamage(DamageCalculator.CalculateByDistance(collider.transform.position, 
                other.transform.position, origin.damageMultiplier));
            OnTrigger();
            return;
        }
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET NEUTRAL:" + this.gameObject);

        // If this is a destructible, attempt to inflict damage.
        IDestructable destructable = other.GetComponent<IDestructable>();
        if (destructable != null)
        {
            destructable.ReceiveDamage(DamageCalculator.CalculateByDistance(collider.transform.position, 
                other.transform.position, origin.damageMultiplier));
            OnTrigger();
            return;
        }

        // TODO Handle laser.
        if (other.GetComponent<Laser>() != null) return;

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
        ParticleController.GetInstance().InstantiateParticle(ParticleController.ProjectileBounce, transform.position);
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET DISADVANTAGE:" + this.gameObject);

        // Destroy this projectile.
        ParticleController.GetInstance().InstantiateParticle(ParticleController.ObstacleDestroy, transform.position);
        Destroy(this.gameObject);

    }

    public Weapon GetOrigin()
    {
        return origin;
    }
}
