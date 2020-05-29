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
    //public float radiusOfCollider;

    // Components
    private new SphereCollider collider;
    private IActivator activator;

    /// <summary>
    /// Set up this by finding this object's IActivator and Collider(or make one) component.
    /// </summary>
    void Start()
    {

        // Find the components.
        activator = GetComponent<IActivator>();
        try
        {
            collider = gameObject.GetComponent<SphereCollider>();
        } catch (System.NullReferenceException)
        {
            collider = gameObject.AddComponent<SphereCollider>();

        }
        collider.isTrigger = true;

    }

    public void SetTarget(GameObject o)
    {
        this.target = o;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(target))
        {
            activator.Activate();
        }
    }
}
