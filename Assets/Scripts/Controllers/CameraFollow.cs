using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;



    private void FixedUpdate()
    {
        Vector3 newPosition = target.position + offset;
        newPosition.y = transform.position.y; // Lock the y-position.
        Vector3 smoothed = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothSpeed);
        transform.position = smoothed;
    }
}
