using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

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

    // For class computations
    private float velocity;
    private float acceleration;
    private float deceleration;
    private Transform target;
    private Rigidbody rigidBody;
    private float timer = 0;

    private void Start()
    {
        GameObject parent = transform.parent.gameObject;
        if (parent.tag == "Enemy Shooter")
        {
            if (PlayerController.instance == null)
            {
                Debug.LogWarning("PlayerController can not be found by this bullet", this.gameObject);
                target = null;
            }
            else
            {
                target = PlayerController.instance.transform;
            }
        }
        Shooter shooter = parent.GetComponent<Shooter>();
        velocity = shooter.speed;
        rigidBody = GetComponent<Rigidbody>();
        acceleration = maxSpeed / timeToMax;
        deceleration = -1 * (maxSpeed / timeToMin); 
    }

    private void Update()
    {
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


    private void FixedUpdate()
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


}
