﻿using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    [SerializeField] private GameObject obstacleDamagePrefab;
    [SerializeField] private GameObject obstacleDestroyEffectPrefab;
    [SerializeField] private GameObject obstacleNegateEffectPrefab;
    [SerializeField] private GameObject projectileBounceEffectPrefab;


    public static string ObstacleDamage { private set; get; }
    public static string ObstacleDestroy { private set; get; }
    public static string ObstacleNegate { private set; get; }
    public static string ProjectileBounce { private set; get; }


    private static Dictionary<string, GameObject> dictionary;
    private static ParticleController instance;

    #region Singleton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;

        // Set up the dictionary.
        dictionary = new Dictionary<string, GameObject>();
        ObstacleDamage = obstacleDamagePrefab.name;
        ObstacleDestroy = obstacleDestroyEffectPrefab.name;
        ObstacleNegate = obstacleNegateEffectPrefab.name;
        ProjectileBounce = projectileBounceEffectPrefab.name;
        dictionary.Add(ObstacleDamage, obstacleDamagePrefab);
        dictionary.Add(ObstacleDestroy, obstacleDestroyEffectPrefab);
        dictionary.Add(ObstacleNegate, obstacleNegateEffectPrefab);
        dictionary.Add(ProjectileBounce, projectileBounceEffectPrefab);
        
    }
    public static ParticleController GetInstance()
    {
        return instance;
    }
    #endregion


    public void InitiateParticle(string particleName, Vector3 position)
    {
        GameObject o;
        if (dictionary.TryGetValue(particleName, out o)){
            Instantiate(o, position, o.transform.rotation, gameObject.transform);
            Debug.Log("Init particle:" + o.name);
        }
        else
        {
            throw new System.ArgumentException("particle name not found.");
        }
    }
}
