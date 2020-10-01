using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{


    public Vector3 velocity;
    public bool isMoving = true;


    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(transform.position + velocity * Time.unscaledDeltaTime);
        }
    }


}
