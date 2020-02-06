using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{



    #region Fields

    // Bullet options
    public GameObject prefab;
    public float speed;
    [Range(0, 360)]
    public float aim;
    public float startDelay;
    public float shotDelay;
    public bool shootAtPlayer;
    public bool continuousFire;

    [Header("Shoot in rounds")]
    public int shotsInRound;
    public float roundDelay;

    [Header("If not continuousFire")]
    public int requiredShots;
    public int requiredRounds;

    [Header("Rotation")]
    public bool constantRotation;
    public float spinAngle;

    [Header("Shoot in arrays and groups")]
    public bool equalArraySpread;
    public int arrays;
    [Range(0, 360)]
    public float arraySpread;
    public int arrayGroups;
    [Range(0, 360)]
    public float arrayGroupSpread;

    // For class calculations
    private Vector3 defaultAim;
    [HideInInspector]
    public bool shooterOn;
    private int roundsAlreadyShot;
    private int alreadyShot;
    private Rigidbody rigidBody;

    // For player's shooter
    [HideInInspector]
    public bool playerShooter = false;


    #endregion

    private void Start()
    {
        // Logic checks
        if (equalArraySpread)
        {
            if (arrayGroups > 1)
            {
                Debug.LogWarning("Cannot shoot arrays at equal spread with multiple array groups. equalArraySpread will be set to false instead.");
                equalArraySpread = false;
            }
            
        }
        else
        {
            if (arrays <= 0)
            {
                Debug.LogWarning("There must be at least one array. arrays will be set to 1 instead.");
                arrays = 1;
            }
            if (arrayGroups <= 0)
            {
                Debug.LogWarning("There must be at least one array group. arrayGroups will be set to 1 instead.");
                arrayGroups = 1;
            }
            if (arrays > 1 && arraySpread == 0)
            {
                Debug.LogWarning("There are more than 1 array, but array spread is not defined. arrays will be set to 1 instead,");
                arrays = 1;
            }
            if (arrayGroups > 1 && arrayGroupSpread == 0)
            {
                Debug.LogWarning("There are more than 1 array group, but array group spread is not defined. arrayGroups will be set to 1 instead.");
                arrayGroups = 1;
            }
        }

        if (shotsInRound % arrays != 0)
        {
            Debug.LogWarning("There are not enough shots in round for each arrays.");
        }


        // Setups
        rigidBody = GetComponent<Rigidbody>();
        if (shootAtPlayer)
        {
            SetAimToPlayer();
        }
        else
        {
            // Convert the aim field to a more appropriate unit for calculations, Vector3.
            defaultAim = new Vector3(Mathf.Cos(aim * Mathf.Deg2Rad), Mathf.Sin(aim * Mathf.Deg2Rad), 0);
        }

        // Begin shooting on spawn.
        InvokeRepeating("CalculateShot", startDelay, shotDelay);
        StartCoroutine(ConsiderStartDelay());
        roundsAlreadyShot = 1; // Consider this as the first round.
    }


    private void FixedUpdate()
    {

        // If the shooter is shooting at the player, continue to update the the aim with SetAimToPlayer.
        if (shootAtPlayer)
        {
            SetAimToPlayer();
        }

        // If the shooter is turned off, do not calculate anything below.
        if (!shooterOn)
        {
            return;
        }

        // If this is the player's shooter, shoot whenever shooterOn is set to true based on player input
        // recorded in PlayerController.
        if (playerShooter && shooterOn)
        {
            Shoot(defaultAim);
        }

        // Calculate shoot times
        if (continuousFire)
        {
            // If shooting in rounds, check if the shooter has already shot the required number of shots in a round.
            // When it has, it will cancel and invoke Shoot() to begin the next round of shots.
            if (shotsInRound > 1)
            {
                //Debug.Log("went here:" + gameObject.transform.parent.name);
                if (alreadyShot == shotsInRound)
                {
                    alreadyShot = 0; // Reset individual shot counter.
                    CancelInvoke();
                    InvokeRepeating("CalculateShot", roundDelay, shotDelay);
                    roundsAlreadyShot++; // Count this new round.
                }
            }
        }
        else // If not continuous, check if the shooter is done shooting its shots.
        {
            // If shooting in rounds, check if this shooter can stop shooting. If the shooter already completed
            // it's total number of rounds, then immediately stop shooting. 
            // If it has not, then invoke a new round.
            if (shotsInRound > 1)
            {
                if (roundsAlreadyShot > requiredRounds)
                {
                    CancelInvoke();
                }
                else if (alreadyShot >= shotsInRound)
                {
                    alreadyShot = 0; // Reset individual shot counter.
                    CancelInvoke();
                    InvokeRepeating("CalculateShot", roundDelay, shotDelay);
                    roundsAlreadyShot++; // Count this new round.
                }
            }
            else
            {
                // If not shooting in rounds, stop shooting if the shooter has already shot the total number of shots.
                if (alreadyShot == requiredShots)
                {
                    CancelInvoke();
                }
            }
        }

        // If there is a rotation, then change the default aim by spinAngle at each frame to "rotate"
        // the shot.
        if (constantRotation)
        {
            aim += spinAngle;
            defaultAim = new Vector3(Mathf.Cos(aim * Mathf.Deg2Rad), Mathf.Sin(aim * Mathf.Deg2Rad), 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newAim"></param>
    public void SetAim(float newAim)
    {
        aim = newAim;
        defaultAim = new Vector3(Mathf.Cos(aim * Mathf.Deg2Rad), Mathf.Sin(aim * Mathf.Deg2Rad), 0);
    }


    public void SetAimToPlayer()
    {
        Vector3 toPlayer = PlayerController.instance.transform.position - transform.position;
        toPlayer.Normalize();
        defaultAim = toPlayer;
        Vector3 to = toPlayer;
        Vector3 from = transform.position;
        float dot = Vector3.Dot(to, from);
        float det = to.x * from.y - to.y * from.x;
        aim =  Mathf.Atan2(det, dot);
    }


    /// <summary>
    /// Set shooterOn to false until the startDelay time has passed. Only used once during spawn.
    /// </summary>
    /// <returns>Couroutine WaitForSeconds.</returns>
    private IEnumerator ConsiderStartDelay()
    {
        shooterOn = false;
        yield return new WaitForSeconds(startDelay);
        shooterOn = true;
        yield return 0;
    }

    /// <summary>
    /// Calculate the shooting pattern based on the number of shot arrays, groups, and spread.
    /// </summary>
    private void CalculateShot()
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
                float startDegree = aim;
                for (int i = 0; i < arrays; i++)
                {
                    float newAim = startDegree + (spread * i);
                    Vector3 newAimVector = new Vector3(Mathf.Cos(newAim * Mathf.Deg2Rad), Mathf.Sin(newAim * Mathf.Deg2Rad), 0);
                    Shoot(newAimVector);
                }
            }
            else
            {
                // Determine startGroupDegree and center increment for calculations based on if there are an even number of shot
                // array groups and shot arrays. 
                float startGroupDegree = 0;
                if (arrayGroups % 2 == 0)
                {
                    startGroupDegree = aim - (((arrayGroups / 2) - 1) * arrayGroupSpread) - (arrayGroupSpread / 2);
                }
                else
                {
                    startGroupDegree = aim - (Mathf.Floor((arrayGroups / 2)) * arrayGroupSpread);
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
                        Vector3 newAim = new Vector3(Mathf.Cos(center * Mathf.Deg2Rad), Mathf.Sin(center * Mathf.Deg2Rad), 0);
                        Shoot(newAim);
                    }
                }
            }
        }
        else
        {
            Shoot(defaultAim);
        }
    }

    /// <summary>
    /// Shoot a single bullet based on the speed, and aim. Keeps track of the total number of shots already
    /// shot in alreadyShot.
    /// </summary>
    /// <param name="aim">Vector3 representing the direction the bullet will be shot towards.</param>
    private void Shoot(Vector3 aim)
    {
        GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity, gameObject.transform);
        Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();
        //Debug.Log("aim:" + aim);
        rigidBody.AddForce(aim * speed, ForceMode.Impulse);
        rigidBody.MoveRotation(Quaternion.LookRotation(aim));
        alreadyShot++;
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
                float startDegree = aim;
                for (int i = 0; i < arrays; i++)
                {
                    float newAim = startDegree + (spread * i);
                    Vector3 newAimVector = new Vector3(Mathf.Cos(newAim * Mathf.Deg2Rad), Mathf.Sin(newAim * Mathf.Deg2Rad), 0);
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
                    startGroupDegree = aim - (((arrayGroups / 2) - 1) * arrayGroupSpread) - (arrayGroupSpread / 2);
                }
                else
                {
                    startGroupDegree = aim - (Mathf.Floor((arrayGroups / 2)) * arrayGroupSpread);
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
                        Vector3 newAimVector = new Vector3(Mathf.Cos(center * Mathf.Deg2Rad), Mathf.Sin(center * Mathf.Deg2Rad), 0);
                        Gizmos.DrawLine(transform.position, (newAimVector * speed) * 20);
                    }
                }
            }
        }
        else
        {
            //Shoot(defaultAim);
            Gizmos.DrawLine(transform.position, defaultAim * 20 * speed);
        }
    }






}
