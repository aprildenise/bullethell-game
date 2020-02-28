using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{


    public bool scrollCamera; // For testing and debugging only.
    public float scrollIncrement = .05f;

    /// <summary>
    /// Update is called once per frame. Scroll the camera by the scrollIncrement at each frame.
    /// </summary>
    void Update()
    {
        if (scrollCamera)
        {

            var cameraPosition = Camera.main.gameObject.transform.position;
            cameraPosition.z += scrollIncrement;
            Camera.main.gameObject.transform.position = cameraPosition;
        }
    }
}
