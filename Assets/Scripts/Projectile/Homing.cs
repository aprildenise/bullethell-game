using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{

    public GameObject target;
    public float homingRate;
    public bool isHoming;
    public Vector3 currentVelocity;

    private Rigidbody rigidBody;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    /// <summary>
    /// If homing, alter the path of this bullet.
    /// Also, apply the acceleration and deceleration found in Update.
    /// </summary>
    private void FixedUpdate()
    {
        // If this bullet has a homingRate, then it will home and move towards the player.
        if (homingRate > 0)
        {
            // Take the cross product of the normalized vectors for the direction to the target,
            // and the direction the bullet is currently moving in, in order to find the rotation needed
            // to reach the target.
            Vector3 targetDirection = target.transform.position - rigidBody.position;
            targetDirection.Normalize();
            Vector3 currentDirection = rigidBody.velocity;
            currentDirection.Normalize();
            Vector3 rotate = Vector3.Cross(targetDirection, currentDirection);

            // Apply the rotation.
            rigidBody.angularVelocity = -rotate * homingRate;
        }
        // Continue moving the bullet in its current trajectory.
        rigidBody.velocity = currentVelocity;
    }

}
