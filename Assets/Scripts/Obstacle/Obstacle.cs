using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IDestructable, ITypeSize
{

    /// <summary>
    /// Health Points of this object.
    /// </summary>
    protected float healthPoints;

    protected Type type;


    protected BoxCollider hitBox;
    protected MeshRenderer mesh;


    protected abstract void RunStart();

    #region Destructible

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger test");
    }

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

    #endregion

    #region TypeSize

    Type ITypeSize.GetType()
    {
        return this.type;
    }

    public virtual Size GetSize()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetType(Type type)
    {
        this.type = type;
    }

    public virtual void SetSize(Size size)
    {
        throw new System.NotImplementedException();
    }

    public void OnAdvantage(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        throw new System.NotImplementedException();
    }


    #endregion
}
