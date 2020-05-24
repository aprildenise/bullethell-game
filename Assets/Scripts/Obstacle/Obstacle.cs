using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IDestructable
{

    protected float healthPoints;
    protected BoxCollider hitBox;
    protected MeshRenderer mesh;

    public void ReceiveDamage(float damageReceived)
    {
        healthPoints -= damageReceived;
        if (!HasHealth())
        {
            OnZeroHealth();
        }
    }

    public bool HasHealth()
    {
        return healthPoints > 0;
    }

    public void OnZeroHealth()
    {
        Destroy(this);
    }

    protected abstract void RunStart();

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger test");
    }

}
