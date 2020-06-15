using UnityEngine;

public abstract class Shooter : Weapon
{

    #region Variables

    // Attributes of this shooter.
    [SerializeField]
    protected ShooterInfo shooterInfo;
    public GameObject bulletPrefab;
    public float speed;
    public float aimDegree;
    public float shotDelay;
    public bool homing;
    [Header("Shoot in arrays and groups")]
    public bool equalArraySpread;
    public int arrays;
    [Range(0, 360)]
    public float arraySpread;
    public int arrayGroups;
    [Range(0, 360)]
    public float arrayGroupSpread;

    // For class calculations.
    protected Vector3 aimVector;
    protected Vector3 center;
    //protected bool isShooting;
    protected bool inDelay;
    protected int shots;

    #endregion
    

    #region Shooter Functions
    /// <summary>
    /// Run the Start method. Used by children of this class.
    /// </summary>
    protected virtual void RunStart()
    {
        this.Start();
    }

    /// <summary>
    /// Initialize on start up.
    /// </summary>
    private void Start()
    {

        if (shooterInfo != null)
        {
            SetShooterInfo(shooterInfo);
        }

        // Logic checks
        if (equalArraySpread)
        {
            if (arrayGroups > 1)
            {
                Debug.Log("Cannot shoot arrays at equal spread with multiple array groups. equalArraySpread will be set to false instead.");
                equalArraySpread = false;
            }

        }
        else
        {
            if (arrays <= 0)
            {
                Debug.Log("There must be at least one array. arrays will be set to 1 instead.");
                arrays = 1;
            }
            if (arrayGroups <= 0)
            {
                Debug.Log("There must be at least one array group. arrayGroups will be set to 1 instead.");
                arrayGroups = 1;
            }
            if (arrays > 1 && arraySpread == 0)
            {
                Debug.Log("There are more than 1 array, but array spread is not defined. arrays will be set to 1 instead,");
                arrays = 1;
            }
            if (arrayGroups > 1 && arrayGroupSpread == 0)
            {
                Debug.Log("There are more than 1 array group, but array group spread is not defined. arrayGroups will be set to 1 instead.");
                arrayGroups = 1;
            }
        }

    }


    /// <summary>
    /// Set the aim of this shooter, in degrees.
    /// </summary>
    /// <param name="newAim">New aim</param>
    public void SetAim(float aimDegree)
    {
        this.aimDegree = aimDegree;
        aimVector = new Vector3(Mathf.Cos(aimDegree * Mathf.Deg2Rad), 0, Mathf.Sin(aimDegree * Mathf.Deg2Rad));
    }

    /// <summary>
    /// Set the aim of this shooter, in vectors.
    /// </summary>
    /// <param name="newVector">New aim</param>
    public void SetAim(Vector3 aimVector)
    {
        this.aimVector = aimVector;
        if (aimVector.x < 0) this.aimDegree = 90f + Vector3.Angle(aimVector, Vector3.forward);
        else this.aimDegree = 90f - Vector3.Angle(aimVector, Vector3.forward);
    }



    /// <summary>
    /// Calculate the shooting pattern based on the number of shot arrays, groups, and spread.
    /// </summary>
    /// <returns>Always true</returns>
    protected void Shoot()
    {
        // If there is more than one shot array, then calculate the pattern.
        // If not, then shoot a simple shot with Shoot().
        if (arrays > 1)
        {
            // If equalArraySpread, calculate an equal spread between the arrays and spawn the arrays
            // at each of those spreads.
            if (equalArraySpread)
            {
                float spread = 360 / arrays;
                float startDegree = aimDegree;
                for (int i = 0; i < arrays; i++)
                {
                    float newAim = startDegree + (spread * i);
                    Vector3 newAimVector = new Vector3(Mathf.Cos(newAim * Mathf.Deg2Rad), 0, Mathf.Sin(newAim * Mathf.Deg2Rad));
                    InitBullet(newAimVector);
                }
            }
            else
            {
                // Determine startGroupDegree and center increment for calculations based on if there are an even number of shot
                // array groups and shot arrays. 
                float startGroupDegree = 0;
                if (arrayGroups % 2 == 0)
                {
                    startGroupDegree = aimDegree - (((arrayGroups / 2) - 1) * arrayGroupSpread) - (arrayGroupSpread / 2);
                }
                else
                {
                    startGroupDegree = aimDegree - (Mathf.Floor((arrayGroups / 2)) * arrayGroupSpread);
                }
                float centerIncrement = 0;
                if (arrays % 2 == 0)
                {
                    centerIncrement = (((arrays / 2) - 1) * arraySpread) + (arraySpread / 2);
                }
                else
                {
                    centerIncrement = (Mathf.Floor((arrays / 2)) * arraySpread);
                }

                // Loop through to spawn the shots. Begin spawning them at startGroupDegree.
                for (int i = 0; i < arrayGroups; i++)
                {
                    float arrayCenter = startGroupDegree + (arrayGroupSpread * i);
                    // Calculate the center of each array group. Spawning bullets for 
                    // each array will begin at startDegree.
                    float startDegree = arrayCenter - centerIncrement;
                    for (int j = 0; j < arrays; j++)
                    {
                        float center = startDegree + (arraySpread * j);
                        Vector3 newAim = new Vector3(Mathf.Cos(center * Mathf.Deg2Rad), 0, Mathf.Sin(center * Mathf.Deg2Rad));
                        InitBullet(newAim);
                    }
                }
            }
        }
        else
        {
            InitBullet(aimVector);
        }
    }

    /// <summary>
    /// Shoot a single bullet based on the speed, and aim. Keeps track of the total number of shots already
    /// shot in alreadyShot.
    /// </summary>
    /// <param name="aim">Vector3 representing the direction the bullet will be shot towards.</param>
    protected void InitBullet(Vector3 aim)
    {
        GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity, SpawnPoint.GetSpawnPoint().transform);
        Projectile p = projectile.GetComponent<Projectile>();
        p.origin = this;
        p.currentVelocity = aim * speed ;

        // Fire the bullet.
        Rigidbody rigidBody = p.GetComponent<Rigidbody>();
        rigidBody.AddForce(aim * speed, ForceMode.Impulse);
        rigidBody.MoveRotation(Quaternion.LookRotation(aim));
        shots++;
    }


    /// <summary>
    /// For testing only. Uses the code from CalculateShot() in order to draw lines showing where the 
    /// bullets will be fired.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // If there is more than one shot array, then calculate the pattern.
        // If not, then shoot a simple shot with Shoot().
        if (arrays > 1)
        {
            // If equalArraySpread, calculate an equal spread between the arrays and spawn the arrays
            // at each of those spreads.
            if (equalArraySpread)
            {
                float spread = 360 / arrays;
                float startDegree = aimDegree;
                for (int i = 0; i < arrays; i++)
                {
                    float newAim = startDegree + (spread * i);
                    Vector3 newAimVector = new Vector3(Mathf.Cos(newAim * Mathf.Deg2Rad), 0, Mathf.Sin(newAim * Mathf.Deg2Rad));
                    Gizmos.DrawLine(transform.position, (newAimVector * speed) * 20);
                    //Shoot(newAimVector);
                }
            }
            else
            {
                // Determine startGroupDegree and center increment for calculations based on if there are an even number of shot
                // array groups and shot arrays. 
                float startGroupDegree = 0;
                if (arrayGroups % 2 == 0)
                {
                    startGroupDegree = aimDegree - (((arrayGroups / 2) - 1) * arrayGroupSpread) - (arrayGroupSpread / 2);
                }
                else
                {
                    startGroupDegree = aimDegree - (Mathf.Floor((arrayGroups / 2)) * arrayGroupSpread);
                }
                float centerIncrement = 0;
                if (arrays % 2 == 0)
                {
                    centerIncrement = (((arrays / 2) - 1) * arraySpread) + (arraySpread / 2);
                }
                else
                {
                    centerIncrement = (Mathf.Floor((arrays / 2)) * arraySpread);
                }

                // Loop through to spawn the shots. Begin spawning them at startGroupDegree.
                for (int i = 0; i < arrayGroups; i++)
                {
                    float arrayCenter = startGroupDegree + (arrayGroupSpread * i);
                    // Calculate the center of each array group. Spawning bullets for 
                    // each array will begin at startDegree.
                    float startDegree = arrayCenter - centerIncrement;
                    //Debug.Log(startGroupDegree);
                    for (int j = 0; j < arrays; j++)
                    {
                        float center = startDegree + (arraySpread * j);
                        Vector3 newAimVector = new Vector3(Mathf.Cos(center * Mathf.Deg2Rad), 0, Mathf.Sin(center * Mathf.Deg2Rad));
                        Gizmos.DrawLine(transform.position, (newAimVector * speed) * 20);
                    }
                }
            }
        }
        else
        {
            //Shoot(defaultAim);
            Gizmos.DrawLine(transform.position, aimVector * 20 * speed);
        }
    }

    public void SetShooterInfo(ShooterInfo shooterInfo)
    {
        this.shooterInfo = shooterInfo;
        weaponName = shooterInfo.shooterName;
        damageMultiplier = shooterInfo.damageMultiplier;
        bulletPrefab = shooterInfo.prefab;
        speed = shooterInfo.speed;
        aimDegree = shooterInfo.aimDegree;
        shotDelay = shooterInfo.shotDelay;
        homing = shooterInfo.homing;
        equalArraySpread = shooterInfo.equalArraySpread;
        arrays = shooterInfo.arrays;
        arraySpread = shooterInfo.arraySpread;
        arrayGroups = shooterInfo.arrayGroups;
        arrayGroupSpread = shooterInfo.arrayGroupSpread;
        SetType(shooterInfo.shooterType);
        SetSize(shooterInfo.shooterSize);
    }

    #endregion

    #region TypeSize

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

    #endregion

    /// <summary>
    /// For testing only.
    /// </summary>
    protected void OnValidate()
    {
        if (shooterInfo != null)
        {
            SetShooterInfo(this.shooterInfo);
        }
    }
}