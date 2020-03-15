using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActivateOnProximity : MonoBehaviour
{

    public GameObject target;
    public BoxCollider triggerCollider;
    public GameObject parent;
    public IActivator activator;
    [SerializeField]
    private float timeUntilTrigger;

    [Header("Debug and test only")]
    public bool refresh;


    private void Start()
    {
        activator = transform.parent.gameObject.GetComponent<IActivator>();
        parent = transform.parent.gameObject;
    }

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

    private void OnDrawGizmos()
    {
        string s = "Time until trigger:" + timeUntilTrigger;
        Handles.color = Color.black;
        Handles.Label(transform.position, s);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Called:" + other.gameObject.name);
        if (other.gameObject.Equals(target))
        {
            activator.activate();
        }
    }


}
