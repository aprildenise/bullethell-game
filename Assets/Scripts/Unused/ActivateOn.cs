using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Calls objects that have the IActivator interface in order to Activate() them.
/// Placed on a CHILD OBJECT of the object to be activated.
/// </summary>
public class ActivateOn : MonoBehaviour
{


    [SerializeField]
    /// <summary>
    /// Object that, within proximity, will activate this object.
    /// </summary>
    protected GameObject target;

    [SerializeField]
    /// <summary>
    /// Collider for this object, which will collider with the target.
    /// </summary>
    protected BoxCollider triggerCollider;

    [SerializeField]
    /// <summary>
    /// GameObject that implements the IActivator interface and is supposed to be "activated."
    /// Should be the PARENT of this object.
    /// </summary>
    protected GameObject toActivate;

    /// <summary>
    /// IActivator component of the toActivate object.
    /// </summary>
    protected IActivator activator;


    /// <summary>
    /// Setups this object.
    /// </summary>
    private void Start()
    {
        try
        {
            triggerCollider = GetComponent<BoxCollider>();
            activator = transform.parent.gameObject.GetComponent<IActivator>();
            toActivate = transform.parent.gameObject;
        } catch(System.NullReferenceException e)
        {
            e.ToString();
            Debug.LogError(this.gameObject.name + "is missing some objects.");
        }
    }

    //public GameObject GetParent()
    //{
    //    return parent;
    //}

    /// <summary>
    /// Used by children of this call to setup the object.
    /// </summary>
    protected void CallStart()
    {
        Start();
    }

    /// <summary>
    /// Controls when this object calls the parent's Activate().
    /// </summary>
    /// <param name="other">Collider that trggered this object. </param>
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(target))
        {
            activator.Activate();
        }
    }

}
