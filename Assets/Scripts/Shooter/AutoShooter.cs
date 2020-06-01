
using UnityEngine;

public abstract class AutoShooter : Shooter
{


    public AutoShooterInfo autoShooterInfo;

    [Header("Main Enemy Shooter")]
    public float startDelay;

    [Header("Shoot in sets")]
    public bool shootInSets;
    public int shotsPerSet;
    public float setDelay;

    public int requiredShots;
    public int requiredSets;

    [Header("Rotation")]
    public bool constantRotation;
    public float spinAngle;

    protected int sets;

    // For calculations.
    private Timer startDelayTimer;
    private Timer shotDelayTimer;
    private Timer setDelayTimer;


    /// <summary>
    /// Used by children of this class. Calls the FixedUpdate() in this class.
    /// </summary>
    protected virtual void RunFixedUpdate()
    {
        this.FixedUpdate();
    }
    
    public void SetAutoShooterInfo(AutoShooterInfo info)
    {
        shooterInfo = info.shooterInfo;
        SetShooterInfo(shooterInfo);
        autoShooterInfo = info;
        startDelay = info.startDelay;
        shootInSets = info.shootInSets;
        shotsPerSet = info.shotsPerSet;
        setDelay = info.setDelay;
        requiredShots = info.requiredShots;
        requiredSets = info.requiredSets;
        constantRotation = info.constantRotation;
        spinAngle = info.spinAngle;
        sets = info.sets;

}

    /// <summary>
    /// If this shot is homing, set the aim automatically to the player.
    /// </summary>
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


    /// <summary>
    /// Set the aim, timers, and run start of the parent through RunStart().
    /// </summary>
    protected void Awake()
    {
        RunStart();
        SetAim(aimDegree);
        if (homing)
        {
            SetAimToPlayer();
        }

        // Set the timers.
        startDelayTimer = this.gameObject.AddComponent<Timer>();
        shotDelayTimer = this.gameObject.AddComponent<Timer>();
        startDelayTimer.SetTimer(startDelay);
        shotDelayTimer.SetTimer(shotDelay, Timer.Status.FINISHED);
        startDelayTimer.StartTimer();
        shotDelayTimer.StartTimer();
        if (shootInSets)
        {
            setDelayTimer = this.gameObject.AddComponent<Timer>();
            setDelayTimer.SetTimer(setDelay);
            setDelayTimer.StartTimer();
        }

    }


    /// <summary>
    /// Being shooting. Note that this does not immediately shoot. The auto shooter is allowed to shoot IFF
    /// the start delay, shot delay, and set delay have all passed.
    /// </summary>
    public void BeginShooting()
    {
        // Check the start delay.
        if (startDelayTimer.GetStatus() == Timer.Status.RUNNING)
        {
            return;
        }
        // Check the set delay.
        if (shootInSets){
            Timer.Status check = setDelayTimer.GetStatus();
            if (check == Timer.Status.RUNNING)
            {
                return;
            }
            else if (check == Timer.Status.FINISHED)
            {
                // Restart this timer.
                Debug.Log("Passed delay");
                setDelayTimer.StartTimer();
            }
        }
        // Check the shot delay.
        Timer.Status check2 = shotDelayTimer.GetStatus();
        if (check2 == Timer.Status.RUNNING)
        {
            return;
        }
        else if (check2 == Timer.Status.FINISHED)
        {
            // Restart this timer.
            //Debug.Log("Passed shot delay");
            shotDelayTimer.StartTimer();
        }

        // All checks are good!
        base.Shoot();

    }


    /// <summary>
    /// Calculates any rotations.
    /// </summary>
    private void FixedUpdate()
    {
        // If there is a rotation, then change the default aim by spinAngle at each frame to "rotate"
        // the shot.
        if (constantRotation)
        {
            aimDegree += spinAngle;
            aimVector = new Vector3(Mathf.Cos(aimDegree * Mathf.Deg2Rad), 0, Mathf.Sin(aimDegree * Mathf.Deg2Rad));
        }
    }

    private void OnValidate()
    {
        if (autoShooterInfo != null)
        {
            SetAutoShooterInfo(autoShooterInfo);
        }
    }

}
