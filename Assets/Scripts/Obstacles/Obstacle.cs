using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IDestructable
{

    public float healthPoints;
    public BoxCollider hitBox;
    public MeshRenderer mesh;

    public void Damage(float damageReceived)
    {
        healthPoints -= damageReceived;
        if (healthPoints < 0)
        {
            Destroy(this);
        }
    }

    protected abstract void RunStart();
}
