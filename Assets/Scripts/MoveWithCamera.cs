using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCamera : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Player")
        {
            return;
        }
        else
        {
            Vector2 position = Camera.main.transform.position;
            transform.position = position;
        }
        
    }

    private void FixedUpdate()
    {
        if (gameObject.tag == "Player")
        {
            Vector3 position = new Vector3(transform.position.x, transform.position.y + (CameraScroller.scrollIncrement));
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            rigidBody.MovePosition(position);
        }
    }
}
