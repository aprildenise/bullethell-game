using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculator
{

    public static float CalculateByDistance(Vector3 origin, Vector3 target, float damageMultiplier)
    {
        float distance = Vector3.Distance(target, origin);
        return (100 / distance) * damageMultiplier;
    }
}
