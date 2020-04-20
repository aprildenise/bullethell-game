using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour { 

    /// <summary>
    /// Shooter associated with this enemy.
    /// </summary>
    /// <remarks>
    /// Null if this Enemy should not have a shooter.
    /// </remarks>
    public Shooter shooter;

    /// <summary>
    /// Sets if this shooter is active, meaning visible and performing actions. True if it is, false elsewise.
    /// </summary>
    private bool isActive;

    /// <summary>
    /// Allows this enemy to follow a bezier curve.
    /// </summary>
    public FollowCurve followCurve;

    // string for now
    private string[] enemyActions;

    /// <summary>
    /// Initiates the class with default values on startup.
    /// </summary>
    private void Start()
    {
        if (followCurve == false)
        {
            gameObject.GetComponent<FollowCurve>();
        }

        shooter.enabled = false;
        SetActive(false);
    }

    /// <summary>
    /// Have this Enemy perform its actions once its activated.
    /// </summary>
    public void Update()
    {
        // Have this enemy shoot once it has finished its curve.
        if (followCurve)
        {
            if (followCurve.IsFinished())
            {
                if (shooter != null)
                {
                    shooter.enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// Activate on trigger and follow actions.
    /// </summary>
    public void activate()
    {
        if (!isActive)
        {
            Debug.Log("Activated");
            SetActive(true);
        }
    }

    /// <summary>
    /// Sets this enemy as active or inactive by turning of its components.
    /// </summary>
    /// <remarks>
    /// Leaves Colliders on so that it can be reactivated.
    /// </remarks>
    /// <param name="active">True of this </param>
    private void SetActive(bool active)
    {
        GetComponent<MeshRenderer>().enabled = active;
        GetComponent<BoxCollider>().enabled = active;
        isActive = active;
        SetFollowCurve(active);
    }


    private void SetFollowCurve(bool follow)
    {
        if (followCurve != false)
        {
            followCurve.Follow(follow);
        }
    }


}
