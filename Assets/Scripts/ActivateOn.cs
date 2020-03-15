using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOn : MonoBehaviour
{

    public GameObject target;
    public BoxCollider triggerCollider;
    public GameObject parent;
    public IActivator activator;

    private void Start()
    {
        activator = transform.parent.gameObject.GetComponent<IActivator>();
        parent = transform.parent.gameObject;
    }

    public GameObject GetParent()
    {
        return parent;
    }

    public void CallStart()
    {
        Start();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(target))
        {
            activator.activate();
        }
    }

}
