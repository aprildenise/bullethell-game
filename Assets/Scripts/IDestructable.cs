using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public interface IDestructable {


    void ReceiveDamage(float damageReceived);
    void OnZeroHealth();
    bool HasHealth();
    void OnTriggerEnter(Collider other);


}
