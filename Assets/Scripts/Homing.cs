using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Homing : MonoBehaviour
{

    public GameObject target;
    public float homingRate;
    public float moveSpeed;

    //[HideInInspector] public Vector3 currentVelocity;

    public Rigidbody bodyThatHomes;


    protected void Start()
    {
        bodyThatHomes = GetComponent<Rigidbody>();
        bodyThatHomes.velocity = Vector3.forward;
    }


    ///// <summary>
    ///// Also, apply the acceleration and deceleration found in Update.
    ///// </summary>
    protected void FixedUpdate()
    {
        if (target == null) return;


        // Take the cross product of the normalized vectors for the direction to the target,
        // and the direction the bullet is currently moving in, in order to find the rotation needed
        // to reach the target.
        Vector3 targetDirection = target.transform.position - bodyThatHomes.position;
        targetDirection.Normalize();
        Vector3 currentDirection = bodyThatHomes.velocity;
        currentDirection.Normalize();
        Vector3 rotate = Vector3.Cross(targetDirection, currentDirection);

        // Apply the rotation.
        bodyThatHomes.angularVelocity = -rotate * homingRate;
        //currentVelocity = bodyThatHomes.angularVelocity;

    }

}
