using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ITypeSize
{
    #region Variables


    [SerializeField]
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

    [Header("Exploding")]
    public bool explodeOnContact;
    public float explosionRadius;
    //public float explosionForce;
    [SerializeField]
    private GameObject explosionEffect;

    [Header("Spawn More Bullets")]
    public bool spawnMoreBullets;
    public float timeToSpawn;
    public GameObject spawnerPrefab;

    [Header("Despawn Over Time")]
    public bool despawnOverTime;
    public float timeToDespawn;


    // For class computations
    private Transform target;
    private Rigidbody rigidBody;
    private float velocity;
    private float acceleration;
    private float deceleration;
    private float timer = 0;
    private Timer spawnMoreTimer;

    // Type and size of the weapon that shot this bullet.
    private Type shooterType;
    private Size shooterSize;
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
        this.shooterType = shooter.shooterType;
        this.shooterSize = shooter.shooterSize;
        this.baseSpeed = shooter.speed;
    }

    public Shooter GetShooter()
    {
        return shooter;
    }

    //public void SetLayer(int layer)
    //{
    //    this.layer = layer;
    //    gameObject.layer = layer;
    //}

    protected virtual void RunStart()
    {
        Start();
    }

    /// <summary>
    /// Set up this bullet.
    /// </summary>
    private void Start()
    {
        velocity = baseSpeed;
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
    /// If this is an exploding bullet, the exploding action takes priority.
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter(Collider other)
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

    public void OnAdvantage(GameObject collider, GameObject other)
    {
        Debug.Log("BULLET ADVANTAGE");
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {

        Debug.Log("BULLET NEUTRAL");

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
