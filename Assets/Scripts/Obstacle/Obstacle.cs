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
        healthPoints = healthPoints - damageReceived;
        Debug.Log(healthPoints);
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

    public void OnAdvantage(GameObject collider, GameObject other)
    {
        //throw new System.NotImplementedException();
        
    }

    public void OnDisadvantage(GameObject collider, GameObject other)
    {
        Destroy(other);
    }

    public void OnNeutral(GameObject collider, GameObject other)
    {
        //throw new System.NotImplementedException();
    }


    #endregion
}
