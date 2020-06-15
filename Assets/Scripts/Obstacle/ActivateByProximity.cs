using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Component that calls IActivator's Activate() when the target reaches within a certain distance of this
/// GameObject
/// </summary>
public class ActivateByProximity : MonoBehaviour
{

    private GameObject target;
    public float distance;

    // Components
    private IActivator activator;

    /// <summary>
    /// Set up this by finding this object's IActivator and Collider(or make one) component.
    /// </summary>
    void Start()
    {

        // Find the components.
        activator = GetComponent<IActivator>();

    }

    private void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
        if (Vector3.Distance(transform.position, target.transform.position) <= distance)
        {
            activator.Activate();
        }
    }

    public void SetTarget(GameObject o)
    {
        this.target = o;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }


}
