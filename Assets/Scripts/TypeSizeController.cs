using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSizeController
{
    /// <summary>
    /// Determine the interaction between two objects, based on their types/sizes.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public static void Interact(GameObject collider, GameObject other)
    {

        ITypeSize incoming;
        ITypeSize receiving;
        try
        {
            incoming = collider.GetComponent<ITypeSize>();
            receiving = other.GetComponent<ITypeSize>();
            Matchup matchup = CheckMatchup(incoming, receiving);
            if (matchup == Matchup.ADVANTAGE) receiving.OnAdvantage(collider, other);
            else if (matchup == Matchup.DISADVANTAGE) receiving.OnDisadvantage(collider, other);
            else receiving.OnNeutral(collider, other);
        }
        catch(System.NullReferenceException)
        {
            // These objects don't need to interact when they collide.
            return;
        }
    }


    private static Matchup CheckMatchup(ITypeSize incoming, ITypeSize receiving)
    {
        Type type1 = incoming.GetType();
        Type type2 = receiving.GetType();
        if (type1 == Type.NONE || type2 == Type.NONE)
        {
            return Matchup.NEUTRAL;
        }
        if (type1 == Type.LIGHT)
        {
            if (type2 == Type.LIGHT) return Matchup.NEUTRAL;
            else if (type2 == Type.MECHANICA) return Matchup.ADVANTAGE;
            else return Matchup.DISADVANTAGE;
        }
        else if (type1 == Type.MECHANICA)
        {
            if (type2 == Type.LIGHT) return Matchup.DISADVANTAGE;
            else if (type2 == Type.MECHANICA) return Matchup.NEUTRAL;
            else return Matchup.ADVANTAGE;
        }
        else if (type1 == Type.DARK)
        {
            if (type2 == Type.LIGHT) return Matchup.ADVANTAGE;
            else if (type2 == Type.MECHANICA) return Matchup.DISADVANTAGE;
            else return Matchup.NEUTRAL;
        }
        else
        {
            throw new System.ArgumentException("Type of object");
        }
    }

    private enum Matchup
    {
        ADVANTAGE,
        DISADVANTAGE,
        NEUTRAL
    }

}


public enum Size
{
    LIGHT,
    MEDIUM,
    HEAVY,
    NONE
}

public enum Type
{
    LIGHT,
    MECHANICA,
    DARK,
    NONE
}
