using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{


    private static SpawnPoint instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public static SpawnPoint GetSpawnPoint()
    {
        return instance;
    }

}
