using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculator
{

    public static float CalculateByDistance(Vector3 origin, Vector3 target, float damageMultiplier)
    {
        float distance = Vector3.Distance(target, origin);
        return distance * damageMultiplier;
    }

    public static float CalculateByDistance(GameObject origin, Vector3 target)
    {
        float damageMultiplier = origin.GetComponent<Weapon>().damageMultiplier;
        float distance = Vector3.Distance(target, origin.transform.position);
        return distance * damageMultiplier;
    }

}
