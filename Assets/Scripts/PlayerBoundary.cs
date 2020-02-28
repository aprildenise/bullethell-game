using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundary : MonoBehaviour
{



    private Rigidbody rb;
    public static Vector3 moveVelocity = new Vector3(0,0,1);
    public static float speed = 1;
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
        speed = rb.velocity.magnitude;
        Debug.Log(speed);
    }


}
