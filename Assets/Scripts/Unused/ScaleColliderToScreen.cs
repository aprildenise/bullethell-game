using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleColliderToScreen : MonoBehaviour
{

    private Vector3 size;
    public float offset;

    void Awake()
    {
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        size = new Vector3(Screen.width, Screen.height);
        Vector3 convertedSize = Camera.main.ScreenToWorldPoint(size);
        size.y = Mathf.Abs(convertedSize.z * 2) + offset;
        size.x = (size.y * (16 / 9) * 2) + offset;
        collider.size = size;
    }

    //void OnDrawGizmosSelected()
    //{
    //    // Draw a yellow cube at the transform position
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireCube(transform.position, size);
    //}
}
