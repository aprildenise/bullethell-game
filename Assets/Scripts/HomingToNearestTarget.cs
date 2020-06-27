using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingToNearestTarget : Homing
{

    public float radius;
    public LayerMask homeOn;

    private void Update()
    {
        if (target != null) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, homeOn);
        if (colliders.Length == 0) return;

        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach(Collider collider in colliders){
            if (Vector3.Distance(transform.position, collider.gameObject.transform.position) < distance)
            {
                closest = collider.gameObject;
            }
        }

        target = closest;
    }

}
