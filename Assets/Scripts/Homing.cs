using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Homing : MonoBehaviour
{

    public GameObject target;
    public float homingRate;
    [HideInInspector] public Vector3 currentVelocity;

    private Rigidbody rigidBody;


    protected void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    ///// <summary>
    ///// If homing, alter the path of this bullet.
    ///// Also, apply the acceleration and deceleration found in Update.
    ///// </summary>
    protected void FixedUpdate()
    {
        if (target == null) return;


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
        //currentVelocity = rigidBody.angularVelocity;

    }

}
