using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSizeController
{
    /// <summary>
    /// Determine the interaction between two objects, based on their types/sizes.
    /// </summary>
    /// <param name="origin">GameObject that contains the Collider that triggered the interaction.</param>
    /// <param name="other">GameObject that collided with the origin's Collider.</param>
    public static void Interact(GameObject origin, GameObject other)
    {

        //Debug.Log("ORIGIN:" + origin + "OTHER:" + other.transform.gameObject);

        ITypeSize originType;
        ITypeSize otherType;
        try
        {
            originType = origin.GetComponent<ITypeSize>();
            otherType = other.GetComponent<ITypeSize>();
            Matchup matchup = CheckMatchup(originType, otherType);
            if (matchup == Matchup.ADVANTAGE)
            {
                originType.OnAdvantage(origin, other);
                //otherType.OnDisadvantage(origin, other);
            }
            else if (matchup == Matchup.DISADVANTAGE)
            {
                otherType.OnDisadvantage(origin, other);
                //otherType.OnAdvantage(origin, other);
            }
            else
            {
                originType.OnNeutral(origin, other);
                //otherType.OnNeutral(other, origin);
            }
                
        }
        catch(System.NullReferenceException)
        {
            // TODO These objects don't need to interact when they collide?
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
