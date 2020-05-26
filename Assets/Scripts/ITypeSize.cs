using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITypeSize
{

    Type GetGameType();
    Size GetSize();
    void SetType(Type type);
    void SetSize(Size size);
    void OnAdvantage(GameObject collider, GameObject other);
    void OnDisadvantage(GameObject collider, GameObject other);
    void OnNeutral(GameObject collider, GameObject other);


}
