using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ParticleController : MonoBehaviour
{

    [SerializeField] private GameObject obstacleDamagePrefab;
    [SerializeField] private GameObject obstacleDestroyEffectPrefab;
    [SerializeField] private GameObject obstacleNegateEffectPrefab;
    [SerializeField] private GameObject projectileBounceEffectPrefab;
    [SerializeField] private GameObject playerDeathEffectPrefab;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject playerLaserCollisionEffectPrefab;


    public static string ObstacleDamage { private set; get; }
    public static string ObstacleDestroy { private set; get; }
    public static string ObstacleNegate { private set; get; }
    public static string ProjectileBounce { private set; get; }
    public static string PlayerDeath { private set; get; }
    public static string Explosion { private set; get; }
    public static string PlayerLaserCollision { private set; get; }


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
        // TODO make this efficient.
        dictionary = new Dictionary<string, GameObject>();
        ObstacleDamage = obstacleDamagePrefab.name;
        ObstacleDestroy = obstacleDestroyEffectPrefab.name;
        ObstacleNegate = obstacleNegateEffectPrefab.name;
        ProjectileBounce = projectileBounceEffectPrefab.name;
        PlayerDeath = playerDeathEffectPrefab.name;
        Explosion = explosionEffectPrefab.name;
        PlayerLaserCollision = playerLaserCollisionEffectPrefab.name;
        dictionary.Add(ObstacleDamage, obstacleDamagePrefab);
        dictionary.Add(ObstacleDestroy, obstacleDestroyEffectPrefab);
        dictionary.Add(ObstacleNegate, obstacleNegateEffectPrefab);
        dictionary.Add(ProjectileBounce, projectileBounceEffectPrefab);
        dictionary.Add(PlayerDeath, playerDeathEffectPrefab);
        dictionary.Add(Explosion, explosionEffectPrefab);
        dictionary.Add(PlayerLaserCollision, playerLaserCollisionEffectPrefab);
        
    }
    public static ParticleController GetInstance()
    {
        return instance;
    }
    #endregion

    /// <summary>
    /// Instantiate a specified particle by name.
    /// </summary>
    /// <param name="particleName"></param>
    /// <param name="position"></param>
    /// <param name="lookAt"></param>
    public void InstantiateParticle(string particleName, Vector3 position, Vector3 lookAtPosition)
    {
        GameObject o = GetParticle(particleName);
        Instantiate(o, position, o.transform.rotation, gameObject.transform);
        o.transform.LookAt(new Vector3(0f, lookAtPosition.y, 0f));
    }

    /// <summary>
    /// Instantiate a specified particle by name.
    /// </summary>
    /// <param name="particleName"></param>
    /// <param name="position"></param>
    public void InstantiateParticle(string particleName, Vector3 position)
    {
        GameObject o = GetParticle(particleName);
        Instantiate(o, position, o.transform.rotation, gameObject.transform);
    }

    private GameObject GetParticle(string particleName)
    {
        GameObject o;
        if (dictionary.TryGetValue(particleName, out o))
        {
            return o;
            //Debug.Log("Init particle:" + o.name);
        }
        else
        {
            throw new System.ArgumentException("particle name not found.");
        }
    }
}
