using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCamera : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 originalPosition = transform.position;
        Vector3 newPosition = new Vector3(originalPosition.x, originalPosition.y, cameraPosition.z);
        transform.position = newPosition;

    }

    private void FixedUpdate()
    {
        //if (gameObject.tag == "Player")
        //{
        //    Vector3 position = new Vector3(transform.position.x, transform.position.y + (CameraScroller.scrollIncrement));
        //    Rigidbody rigidBody = GetComponent<Rigidbody>();
        //    rigidBody.MovePosition(position);
        //}
    }
}
