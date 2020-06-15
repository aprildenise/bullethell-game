using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : Projectile
{

    [Header("Spawn More Projectiles")]
    public float timeToSpawn;
    public GameObject spawn;

    private Timer spawnTimer;

    protected override void Setup()
    {
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.SetTimer(timeToSpawn);
        spawnTimer.StartTimer();
    }

    private void FixedUpdate()
    {
        if (spawnTimer.GetStatus() == Timer.Status.FINISHED)
        {
            GameObject obj = Instantiate(spawn, transform.position, transform.rotation, 
                SpawnPoint.GetSpawnPoint().transform);
            Destroy(this.gameObject);
        }
    }

    protected override void OnTrigger()
    {
        return;
    }
}
