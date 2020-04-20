using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Calls objects that have the IActivator interface in order to Activate() them.
/// Placed on a CHILD OBJECT of the object to be activated.
/// Note that this does not necessarily activate by a timer. An object is "activated"
/// when the Player comes close enough to it. However, you can make an object "activate"
/// at a certain time by pushing it to a specific point in the world.
/// </summary>
public class ActivateByTime : MonoBehaviour
{

    /// <summary>
    /// Object that, within proximity, will activate this object.
    /// </summary>
    public GameObject target;

    [SerializeField]
    /// <summary>
    /// Collider for this object, which will collider with the target.
    /// </summary>
    private BoxCollider triggerCollider;

    /// <summary>
    /// GameObject that implements the IActivator interface and is supposed to be "activated."
    /// Should be the PARENT of this object.
    /// </summary>
    private GameObject toActivate;


    /// <summary>
    /// IActivator component of the toActivate object.
    /// </summary>
    private IActivator activator;

    /// <summary>
    /// Sets the time the parent object will activate. When this is set, this object will move
    /// to the appropriate location in the world in order to make this activate on that time.
    /// </summary>
    [SerializeField]
    private float timeUntilTrigger;

    [Header("Debug and test only")]
    [SerializeField]
    private bool refresh;

    /// <summary>
    /// Set up this object's variables on start up.
    /// </summary>
    private void Start()
    {
        activator = transform.parent.gameObject.GetComponent<IActivator>();
        toActivate = transform.parent.gameObject;
    }

    /// <summary>
    /// Moves the parent object to a position such that it will actually activate by
    /// the time defined in timeUntilTrigger.
    /// </summary>
    private void CalculateTriggerTime()
    {
        // Calculate the position of this activator and the target based on the max bounds of their colliders
        Vector3 parentPosition = triggerCollider.bounds.min;
        Vector3 targetPosition = target.GetComponent<BoxCollider>().bounds.max;

        //Debug.Log(parentPosition);
        //Debug.Log(targetPosition);

        // Set the y and x to 0 so that the distance is calculated 1-dimentionally.
        parentPosition.x = 0;
        parentPosition.y = 0;
        parentPosition.z = Mathf.Abs(parentPosition.z);
        targetPosition.x = 0;
        targetPosition.y = 0;
        targetPosition.z = Mathf.Abs(targetPosition.z);

        // Calculate time
        float distance = Vector3.Distance(parentPosition, targetPosition);
        //Debug.Log("distance:" + distance);
        timeUntilTrigger = distance / (PlayerBoundary.speed);

    }

    private void OnValidate()
    {
        CalculateTriggerTime();
    }

    /// <summary>
    /// To test!
    /// </summary>
    private void OnDrawGizmos()
    {
        string s = "Time until trigger:" + timeUntilTrigger;
        Handles.color = Color.black;
        Handles.Label(transform.position, s);
    }

    /// <summary>
    /// Controls when this object calls the parent's Activate().
    /// </summary>
    /// <param name="other">Collider that trggered this object. </param>
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Called:" + other.gameObject.name);
        if (other.gameObject.Equals(target))
        {
            activator.Activate();
        }
    }


}
