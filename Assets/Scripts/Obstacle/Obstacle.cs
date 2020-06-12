using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IDestructable, ITypeSize
{


    #region Variables

    // Obstacle fields.
    [SerializeField]
    protected ObstacleInfo obstacleInfo;
    protected string obstacleName;
    /// <summary>
    /// Health Points of this object.
    /// </summary>
    protected float healthPoints;
    protected Type obstacleType;

    // Components for this obstacle.
    protected BoxCollider hitBox;
    protected MeshRenderer mesh;

    #endregion

    /// <summary>
    /// Calls Start() of the parent. To be used by the child.
    /// </summary>
    protected abstract void RunStart();

    /// <summary>
    /// Set the ObstacleInfo along with all the fields within it.
    /// </summary>
    /// <param name="obstacleInfo">ObstacleInfo to set.</param>
    protected void SetObstacleInfo(ObstacleInfo obstacleInfo)
    {
        this.obstacleInfo = obstacleInfo;
        obstacleName = obstacleInfo.obstableName;
        healthPoints = obstacleInfo.healthPoints;
        obstacleType = obstacleInfo.obstacleType;
    }


    #region Destructible

    //public void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("trigger test");
    //}

    public void ReceiveDamage(float damageReceived)
    {
        healthPoints -= damageReceived;
        Debug.Log(damageReceived + "," + healthPoints);
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
        Debug.Log("ADVANTAGE:" + name + " destroyed (" + other + ")");
        Destroy(other);
        
    }

    /// <summary>
    /// On Disadvantage matchup, this Obstacle receives damage, if collider is a Bullet.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="other"></param>
    public void OnDisadvantage(GameObject collider, GameObject other)
    {

        Debug.Log("OBSTACLE DISADVANTAGE");
        ReceiveDamage(DamageCalculator.CalculateByDistance(collider, other.transform.position));

        // Destroy weapon spawns.
        if (other.GetComponent<IWeaponSpawn>() != null) Destroy(collider);
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        // If this is a Bullet, send it to OnDisadvantage.
        Bullet bullet = collider.GetComponent<Bullet>();
        if (bullet != null)
        {
            OnDisadvantage(collider, other);
            Debug.Log("NEUTRAL GOING INTO ON DISADVANTAGE.");
        }

    }


    #endregion
}
