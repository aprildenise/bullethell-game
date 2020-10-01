using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : Projectile, ITimerNotification
{

    [Header("Spawn More Projectiles")]
    public float timeToSpawn;
    public GameObject spawn;

    private Timer spawnTimer;

    protected override void OnStart()
    {
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.SetTimer(timeToSpawn);
        spawnTimer.StartTimer();
    }

    /// <summary>
    /// Trigger the spawn.
    /// </summary>
    protected override void OnTrigger()
    {
        GameObject obj = Instantiate(spawn, transform.position, transform.rotation,
            SpawnPoint.GetSpawnPoint().transform);
        Destroy(this.gameObject);
    }

    public void OnTimerFinished()
    {
        OnTrigger();
    }
}
