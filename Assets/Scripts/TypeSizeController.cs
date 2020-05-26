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
        Type type1 = incoming.GetGameType();
        Type type2 = receiving.GetGameType();
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


    public static bool Equals(Type type1, Type type2)
    {
        return type1 == type2;
    }

    public static bool Equals(Size size1, Size size2)
    {
        return size1 == size2;
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
    LIGHT = 0,
    MEDIUM = 1,
    HEAVY = 2,
    NONE = 3
}

public enum Type
{
    LIGHT = 0,
    MECHANICA = 1,
    DARK = 2,
    NONE = 3
}
