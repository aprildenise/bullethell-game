using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPool instance { private set; get; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
    }

    public void AddPool(string tag, GameObject prefab, int size)
    {
        Queue<GameObject> pool = new Queue<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, SpawnPoint.GetSpawnPoint().transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        poolDictionary.Add(tag, pool);
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject toSpawn = poolDictionary[tag].Dequeue();
        toSpawn.SetActive(true);
        toSpawn.transform.position = position;
        toSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(toSpawn);

        return toSpawn;
    }

}
