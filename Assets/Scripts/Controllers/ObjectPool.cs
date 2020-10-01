using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    private Dictionary<string, Queue<GameObject>> poolDictionary;


    #region Singleton
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
    #endregion


    /// <summary>
    /// Add a group of objects to the pool.
    /// </summary>
    /// <param name="tag">Tag to identify this object.</param>
    /// <param name="prefab">Prefab of the object.</param>
    /// <param name="size">Number of objects in the pool.</param>
    public void AddPool(string tag, GameObject prefab, int size)
    {

        if (poolDictionary.ContainsKey(tag))
        {
            return;
        }

        // Create the pool and the objects.
        Queue<GameObject> pool = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, this.gameObject.transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        poolDictionary.Add(tag, pool);
    }

    /// <summary>
    /// Spawn a singular object from the pool.
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject SpawnFromPool(string tag, Vector3 position)
    {
        GameObject toSpawn = poolDictionary[tag].Dequeue();
        toSpawn.SetActive(true);
        toSpawn.transform.position = position;

        poolDictionary[tag].Enqueue(toSpawn);

        return toSpawn;
    }

}
