using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IDestructable, ITypeSize
{
    #region Variables

    // Obstacle fields
    public string obstacleName;
    /// <summary>
    /// Health Points of this object.
    /// </summary>
    public float healthPoints;
    public Type obstacleType;

    // Components for this obstacle.
    protected BoxCollider hitBox;
    protected MeshRenderer mesh;

    #endregion


    #region Destructible

    public void ReceiveDamage(float damageReceived)
    {
        healthPoints -= damageReceived;
        //Debug.Log(damageReceived + "," + healthPoints);

        ParticleController.GetInstance().InstantiateParticle(ParticleController.ObstacleDamage, transform.position);
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
        ParticleController.GetInstance().InstantiateParticle(ParticleController.ObstacleDestroy, transform.position);
        Destroy(this.gameObject);
    }

    #endregion

    #region TypeSize

    public Type GetGameType()
    {
        return this.obstacleType;
    }

    public virtual Size GetSize()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetType(Type type)
    {
        this.obstacleType = type;
    }

    public virtual void SetSize(Size size)
    {
        throw new System.NotImplementedException();
    }

    
    /// <summary>
    /// On advantage, the other Gameobject gets destroyed.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public void OnAdvantage(GameObject collider, GameObject other)
    {
        Debug.Log("OBSTACLE ADVANTAGE:" + name + " destroyed (" + other + ")");
        if (other != this.gameObject) Destroy(other);
        ParticleController.GetInstance().InstantiateParticle(ParticleController.ObstacleNegate, transform.position);
        
    }

    /// <summary>
    /// On Disadvantage matchup, this Obstacle receives damage, if collider is a Bullet.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public void OnDisadvantage(GameObject collider, GameObject other)
    {

        Debug.Log("OBSTACLE DISADVANTAGE");
        //ReceiveDamage(DamageCalculator.CalculateByDistance(collider, other.transform.position));

        // Destroy weapon spawns.
        if (other.GetComponent<IWeaponSpawn>() != null) Destroy(other);
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {

        Debug.Log("OBSTACLE NEUTRAL");

        // If this is a Bullet, send it to OnDisadvantage.
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            OnDisadvantage(collider, other);
            Debug.Log("NEUTRAL GOING INTO ON DISADVANTAGE.");
        }

    }


    #endregion
}
