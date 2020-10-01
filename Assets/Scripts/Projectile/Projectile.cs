using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour, ITypeSize, IPooledObject
{

    [Header("Collide With Layer Masks")]
    public LayerMask collideWith;

    [Header("Acceleration/Decceleration over time")]
    public bool accelerating;
    public bool decelerating;
    public float timeToMax;
    public float timeToMin;
    public float maxSpeed;
    public float minSpeed;

    [Header("Deccelerate, then accelerate over time")]
    public bool deaccelerating;
    public float timeInDecceleration;

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

        //Debug.Log("On trigger collision detected");

        // Check if we're colliding with someting we're allowed to collide with.
        if (((1 << other.gameObject.layer) & collideWith) == 0) return;

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


    #region TypeSize
    public Type GetGameType()
    {
        return origin.GetGameType();
    }

    public Size GetSize()
    {
        return origin.GetSize();
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

        // Apply the new vector.
        Debug.Log("this velocity:" + currentVelocity);
        Debug.Log("other velocity:" + other.GetComponent<Rigidbody>().velocity);

        Rigidbody thisRigidbody = rigidBody;
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        Vector3 otherVelocity = otherRigidbody.velocity;

        Vector3 temp = new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z);
        currentVelocity = new Vector3(otherVelocity.x, otherVelocity.y, otherVelocity.z);
        other.GetComponent<Rigidbody>().velocity = temp;

        rigidBody.MoveRotation(Quaternion.LookRotation(currentVelocity));
        otherRigidbody.MoveRotation(Quaternion.LookRotation(other.GetComponent<Rigidbody>().velocity));

        Debug.Log("this velocity now:" + currentVelocity);
        Debug.Log("other velocity now:" + other.GetComponent<Rigidbody>().velocity);



        //currentVelocity = colliderVelocity2 * multiplier;
        //ParticleController.GetInstance().InstantiateParticle(ParticleController.ProjectileBounce, transform.position);
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET DISADVANTAGE:" + this.gameObject);

        // Destroy this projectile.
        //ParticleController.GetInstance().InstantiateParticle(ParticleController.ObstacleDestroy, transform.position);
        Despawn();

    }

    #endregion

    public Weapon GetOrigin()
    {
        return origin;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
    }
}
