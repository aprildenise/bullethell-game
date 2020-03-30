using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTime : ActivateOn
{

    public float timeToActivate;
    private float colliderSizes;

    // Start is called before the first frame update
    void Start()
    {
        base.CallStart();
    }


    private void CalculatePosition()
    {

        // Push the parent to a distance where which this trigger will activate at.
        if (timeToActivate > 0)
        {
            // Consider the sizes of the colliders.
            float colliderSizes = Mathf.Abs(triggerCollider.size.z / 2) + Mathf.Abs(target.GetComponent<BoxCollider>().size.z / 2);

            // Calculate the new distance.
            float distance = (PlayerBoundary.speed * timeToActivate) + colliderSizes;
            Transform parent = base.GetParent().transform;
            Vector3 newPosition = new Vector3(parent.position.x, parent.position.y, distance);
            parent.position = newPosition;
        }
    }


    private void OnValidate()
    {
        CalculatePosition();
    }
}
