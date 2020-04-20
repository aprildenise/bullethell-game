using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Calls objects that have the IActivator interface in order to Activate() them.
/// Placed on a CHILD OBJECT of the object to be activated.
/// Note that this does not necessarily activate by a timer. An object is "activated"
/// when the Player comes close enough to it. However, you can make an object "activate"
/// at a certain time by pushing it to a specific point in the world.
/// </summary>
public class ActivateOnTime : ActivateOn
{
    [SerializeField]
    /// <summary>
    /// Sets the time the parent object will activate. When this is set, this object will move
    /// to the appropriate location in the world in order to make this activate on that time.
    /// </summary>
    private float timeToActivate;
    //private float colliderSizes;

    /// <summary>
    /// Calls CallStart() of ActivateOn
    /// </summary>
    void Start()
    {
        base.CallStart();
    }


    /// <summary>
    /// Moves the parent object to a position such that it will actually activate by
    /// the time defined in timeUntilTrigger.
    /// </summary>
    private void CalculatePosition()
    {

        // Push the parent to a distance where which this trigger will activate at.
        if (timeToActivate > 0)
        {
            // Consider the sizes of the colliders.
            float colliderSizes = Mathf.Abs(triggerCollider.size.z / 2) + Mathf.Abs(target.GetComponent<BoxCollider>().size.z / 2);

            // Calculate the new distance.
            float distance = (PlayerBoundary.speed * timeToActivate) + colliderSizes;
            Transform parent = base.toActivate.transform;
            Vector3 newPosition = new Vector3(parent.position.x, parent.position.y, distance);
            parent.position = newPosition;
        }
    }


    /// <summary>
    /// Called when the editor is open to move the parent object.
    /// </summary>
    private void OnValidate()
    {
        CalculatePosition();
    }
}
