using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class associated with an agent so that an agent can move by following a curve.
/// </summary>
public class FollowCurve : MonoBehaviour
{

    /// <summary>
    /// Holds all the bezier curves that this agent is meant to follow.
    /// </summary>
    public BezierCurve[] curves;

    /// <summary>
    /// Allows the agent to move when true, and prevents to agent from moving when false.
    /// </summary>
    /// <remarks>
    /// Set using followCurve(bool)
    /// </remarks>
    public bool followingCurve;

    /// <summary>
    /// Sets how fast the agent will follow the curve.
    /// </summary>
    [Range(0f, 1f)]
    public float followSpeed;

    /// <summary>
    /// Iterator used when agent is moving so that the agent can get the next curve to follow.
    /// </summary>
    private int nextCurve;

    /// <summary>
    /// Iterator used to determine what coordinate the agent will be moving to.
    /// </summary>
    private float t;

    /// <summary>
    /// Makes the iterator continue to get curves and move the agent.
    /// </summary>
    private bool coroutineAllowed;

    /// <summary>
    /// Initiates the class with default values on startup.
    /// </summary>
    private void Start()
    {
        // Setups 
        followingCurve = false;
        nextCurve = 0;
        t = 0f;
        followSpeed = 0.25f;
        coroutineAllowed = true;
    }

    /// <summary>
    /// Make the agent start following a curve.
    /// </summary>
    /// <param name="follow">True if the agent should start following, false elsewise.</param>
    public void Follow(bool follow)
    {
        followingCurve = follow;
    }


    /// <summary>
    /// Check if this agent has finished following all its curves.
    /// </summary>
    /// <returns>True if it has, else if it still has more curves.</returns>
    public bool IsFinished()
    {
        return (nextCurve > curves.Length - 1);
    }

    /// <summary>
    /// Update to make the agent start/continue following a curve.
    /// </summary>
    private void Update()
    {
        if (coroutineAllowed && followingCurve)
        {
            StartCoroutine(Follow(nextCurve));
        }
    }

    /// <summary>
    /// Moves the agent along a curve.
    /// </summary>
    /// <remarks>
    /// Agent will move only in the Game View due to WaitForEndOfFrame().</remarks>
    /// <param name="curve">Index of the next curve to follow in bezierCurves</param>
    /// <returns>WaitForEndOfFrame</returns>
    private IEnumerator Follow(int curve)
    {
        // Get all the points in each bezier curve to follow.
        coroutineAllowed = false;
        Vector3 p0 = curves[curve].transform.GetChild(0).position;
        Vector3 p1 = curves[curve].transform.GetChild(1).position;
        Vector3 p2 = curves[curve].transform.GetChild(2).position;
        Vector3 p3 = curves[curve].transform.GetChild(3).position;

        // Calculate where the agent should be according to the points.
        while (t < 1)
        {

            t += Time.deltaTime * followSpeed;
            Vector3 nextPosition = Mathf.Pow(1 - t, 3) * p0
                + 3 * Mathf.Pow(1 - t, 2) * t * p1
                + 3 * (1 - t) * Mathf.Pow(t, 2) * p2
                + Mathf.Pow(t, 3) * p3;

            // Make the agent move to that position and wait.
            transform.position = nextPosition;
            yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(1);
        }

        // Check if there are more curves to follow.
        t = 0;
        nextCurve++;
        if (nextCurve > curves.Length - 1)
        {
            coroutineAllowed = false;
            followingCurve = false;
        }
        else
        {
            coroutineAllowed = true;
        }
    }

}
