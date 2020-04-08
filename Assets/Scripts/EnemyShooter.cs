using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Shooter
{


    [Header("Main Enemy Shooter")]
    public float startDelay;
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

    private int roundsAlreadyShot;


    private void Start()
    {
        base.RunStart();

        if (homing)
        {
            SetAimToPlayer();
        }
        else
        {
            // Convert the aim field to a more appropriate unit for calculations, Vector3.
            aimVector = new Vector3(Mathf.Cos(aimDegree * Mathf.Deg2Rad), 0, Mathf.Sin(aimDegree * Mathf.Deg2Rad));
        }

        Debug.Log("starting here");
        InvokeRepeating("CalculateShot", startDelay, shotDelay);
        //StartCoroutine(ConsiderStartDelay());
        //roundsAlreadyShot = 1; // Consider this as the first round.
        //isShooting = true;
    }

    private void ShootTest()
    {
        Debug.Log("Shoot test");
    }


    private void FixedUpdate()
    {
        // If the shooter is shooting at the player, continue to update the the aim with SetAimToPlayer.
        Debug.Log("isshoting is" + isShooting);
        if (!isShooting)
        {
            return;
        }

        if (homing)
        {
            SetAimToPlayer();
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
            defaultAim = new Vector3(Mathf.Cos(aim * Mathf.Deg2Rad), 0, Mathf.Sin(aim * Mathf.Deg2Rad));
        }
    }

    /// <summary>
    /// Set shooterOn to false until the startDelay time has passed. Only used once during spawn.
    /// </summary>
    /// <returns>Couroutine WaitForSeconds.</returns>
    private IEnumerator ConsiderStartDelay()
    {
        isShooting = false;
        yield return new WaitForSeconds(startDelay);
        isShooting = true;
        yield return 0;
    }


    protected void SetAimToPlayer()
    {
        Vector3 toPlayer = PlayerController.instance.transform.position - transform.position;
        toPlayer.Normalize();
        aimVector = toPlayer;
        Vector3 to = toPlayer;
        Vector3 from = transform.position;
        float dot = Vector3.Dot(to, from);
        float det = to.x * from.y - to.y * from.x;
        aimDegree = Mathf.Atan2(det, dot);
    }

}
