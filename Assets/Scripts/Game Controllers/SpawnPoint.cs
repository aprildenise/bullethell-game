using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public bool despawnTriggersOnOutOfBounds;

    private new BoxCollider collider;
    private static SpawnPoint instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        collider = GetComponent<BoxCollider>();
    }

    public static SpawnPoint GetSpawnPoint()
    {
        return instance;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!despawnTriggersOnOutOfBounds) return;
        Destroy(other.gameObject);
    }

}
