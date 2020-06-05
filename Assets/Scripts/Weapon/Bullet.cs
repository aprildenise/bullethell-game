using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ITypeSize
{
    #region Variables


    [SerializeField]
    private BulletInfo bulletInfo;
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

    [Header("Exploding")]
    public bool explodeOnContact;
    public float explosionRadius;
    [SerializeField]
    private GameObject explosionEffect;

    [Header("Spawn More Bullets")]
    public bool spawnMoreBullets;
    public float timeToSpawn;
    public GameObject spawnerPrefab;

    [Header("Despawn Over Time")]
    public bool despawnOverTime;
    public float timeToDespawn;


    // For computating speed, velocity, acceleration, and spawn.
    private Transform target;
    private Rigidbody rigidBody;
    public Vector3 currentVelocity;
    private float currentSpeed;
    private float acceleration;
    private float deceleration;
    private float timer = 0;
    private Timer spawnMoreTimer;

    // Origins of the bullet.
    private Vector3 origin;
    /// <summary>
    /// The Shooter that init this Bullet.
    /// </summary>
    private Shooter shooter; // TODO Will this loose reference when the original shooter is gone?

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
            this.explodeOnContact = bulletInfo.explodeOnContact;
            this.explosionRadius = bulletInfo.explosionRadius;
            //explosionForce = bulletInfo.explosionForce;
            explosionEffect = bulletInfo.explosionEffect;
            spawnMoreBullets = bulletInfo.spawnMoreBullets;
            timeToSpawn = bulletInfo.timeToSpawn;
            spawnerPrefab = bulletInfo.spawnerPrefab;
        }
    }

    public void SetShooter(Shooter shooter)
    {
        this.shooter = shooter;
        this.currentSpeed = shooter.speed;
    }

    public Shooter GetShooter()
    {
        return shooter;
    }


    protected virtual void RunStart()
    {
        Start();
    }

    /// <summary>
    /// Set up this bullet.
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
        deceleration = -1 * (maxSpeed / timeToMin);
        origin = this.transform.position;

        if (spawnMoreBullets)
        {
            spawnMoreTimer = gameObject.AddComponent<Timer>();
            spawnMoreTimer.SetTimer(timeToSpawn);
            spawnMoreTimer.StartTimer();
        }
    }

    /// <summary>
    /// Find the acceleration and deceleration of the bullet.
    /// </summary>
    private void Update()
    {

        if (spawnMoreBullets)
        {
            if (spawnMoreTimer.GetStatus() == Timer.Status.FINISHED)
            {
                GameObject obj = (GameObject) Instantiate(spawnerPrefab, transform.position, transform.rotation, SpawnPoint.GetSpawnPoint().transform);
                //DiscreteShooter shooter = obj.GetComponent<DiscreteShooter>();
                //shooter.BeginShooting();
                //PrefabController.GetInstance().InitPrefab("", transform.position, transform.rotation, SpawnPoint.GetSpawnPoint().transform);
                Destroy(this.gameObject);
                spawnMoreBullets = false;
            }
        }

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
        // Continue moving the bullet in its current trajectory.
        //rigidBody.velocity = currentVelocity;
    }


    /// <summary>
    /// Interact with what was just collided based on Typing and Size rules.
    /// If this is an exploding bullet, the exploding action takes priority.
    /// </summary>
    /// <param name="other"></param>
    protected void OnColliderEnter(Collider other)
    {

        Debug.Log("collision between:" + other.gameObject.name + "," + gameObject.name);
        // Check if we're colliding with someting on the same sorting layer, or in the Environment layer
        // then don't interact with it.
        if (other.gameObject.layer == this.gameObject.layer || other.gameObject.layer == LayerMask.NameToLayer("Environment")) return;

        // Explosions get first priority.
        if (explodeOnContact)
        {
            Explode();
            return;
        }

        // Check if we're interacting with a bullet.
        Bullet otherBullet = other.gameObject.GetComponent<Bullet>();
        if (otherBullet != null)
        {
            // Make sure this bullet isn't interacting with a bullet from the same Shooter.
            if (GetShooter().Equals(otherBullet.GetShooter())) return;
        }

        // Everything checks out!
        TypeSizeController.Interact(this.gameObject, other.gameObject);

    }


    /// <summary> 
    /// Calculate the damage this bullet will inflict onto a Destructable, based on
    /// the distance from the target and this bullet's shooter.
    /// </summary>
    /// <returns>Damage calculated</returns>
    public virtual float CalculateDamage(GameObject target)
    {
        float distance = Vector3.Distance(target.transform.position, origin);
        return distance * shooter.damageMultiplier;
    }

    private void Explode()
    {
        if (!explodeOnContact) return;

        // TODO instantiate effect.

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, ~(1 << LayerMask.NameToLayer("Environment")));
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

            // If this RB has a destructible, it receives damage.
            IDestructable destructable = rb.gameObject.GetComponent<IDestructable>();
            if (destructable != null)
            {
                destructable.ReceiveDamage(CalculateDamage(rb.gameObject));
                continue;
            }

            // If this is a bullet, the bullet gets destroyed.
            Bullet bullet = rb.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
            }
            //rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

    }
    


    #endregion

    #region TypeSize

    public Type GetGameType()
    {
        return shooter.GetGameType();
    }

    public Size GetSize()
    {
        return shooter.GetSize();
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
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET NEUTRAL");

        Vector3 colliderCenter = collider.GetComponent<Collider>().bounds.center;
        Vector3 otherCenter = other.GetComponent<Collider>().bounds.center;

        Vector3 colliderVelocity1 = collider.GetComponent<Rigidbody>().velocity;
        Vector3 otherVelocity1 = other.GetComponent<Rigidbody>().velocity;

        // TODO Temporarily 1
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

        Debug.Log("collider velocity i:" + currentVelocity + gameObject.name);
        Debug.Log("collider velocity f:" + colliderVelocity2 + gameObject.name);

        //collider.gameObject.GetComponent<Rigidbody>().AddForce(colliderVelocity2 *-.8f, ForceMode.Impulse);
        //currentVelocity = colliderVelocity2 * -.8f;
        //rigidBody.velocity = colliderVelocity2 * -.8f;
        
        Debug.Log("collider now:" + collider.gameObject.GetComponent<Rigidbody>().velocity + gameObject.name);

    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET DISADVANTAGE");

    }


    #endregion

    /// <summary>
    /// For testing only.
    /// </summary>
    private void OnValidate()
    {
        if (bulletInfo != null)
        {
            SetBulletInfo(bulletInfo);
        }
    }
}
